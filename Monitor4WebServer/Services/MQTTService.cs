using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Adapter;
using MQTTnet.Client;
using MQTTnet.Exceptions;
using MQTTnet.Formatter;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Monitor4WebServer.Services
{
    public class MQTTService
    {
        IMqttClient mqttClient;
        private string broker { get; set; }
        private string clientId { get; set; }
        private string topic { get; set; }
        
        public static string resMessage { get; set; }

        public async Task Connect_Client(string brokerIp, int brokerPort, string clientId, Action<string> callback = null)
        {
            var mqttFactory = new MqttFactory();

            var options = new MqttClientOptionsBuilder()
            .WithTcpServer(brokerIp, brokerPort)
            .WithClientId(clientId)
            .WithProtocolVersion(MqttProtocolVersion.V500)
            .Build();

           mqttClient = mqttFactory.CreateMqttClient();

           
            //firts connection
            mqttClient.ConnectedAsync += (async e =>
            {
                Console.WriteLine("MQTT connected");
                Console.WriteLine("");
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("TestMqtt/Room").Build());
            });


            //reconection
            mqttClient.DisconnectedAsync += (async e =>
            {
                Console.WriteLine("Disconnected from MQTT broker. Trying to reconnect...");


                if (!mqttClient.IsConnected)
                {
                    try
                    {
                        await ConnectMqttAsync(options);

                        // Resubscribe to the topic after reconnection
                        await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("zemiMonitor/Room").Build());

                        Console.WriteLine("Reconnected to MQTT broker successfully.");
                    }
                    catch (SocketException ex)
                    {
                        // Captura y maneja la excepción aquí
                        Console.WriteLine("Error de socket: " + ex.Message);
                        // Puedes realizar acciones adicionales, como cerrar conexiones o reiniciar el proceso.
                    }
                    catch (MqttCommunicationTimedOutException)
                    {
                        Console.WriteLine("Tiempo de reconexion MQTT Excedido");
                        return;
                    }
                    catch (MqttConnectingFailedException)
                    {
                        Console.WriteLine("Conexion Mqtt Fallida");
                        return;
                    }
                    catch (MqttCommunicationException)
                    {
                        Console.WriteLine("Error intentando Conectar");
                        return;
                    }
                    catch (Exception ex)
                    {
                        // Handle any exception that may occur during reconnection
                        Console.WriteLine($"Failed to reconnect to MQTT broker. Exception: {ex}");

                        // Wait for a while before attempting reconnection again
                        await Task.Delay(TimeSpan.FromSeconds(5)); // Wait 5 seconds before retrying
                    }
                }

                
            });



            if (!mqttClient.IsConnected)
            {
                await ConnectMqttAsync(options);
            }
          
        }



        public async Task ConnectMqttAsync(MqttClientOptions options)
        {
            try
            {
                await mqttClient.ConnectAsync(options, CancellationToken.None);
                Console.WriteLine(" MQTT Connection successfully.");
            }
            catch (SocketException ex)
            {
                // Captura y maneja la excepción aquí
                Console.WriteLine("Error de socket: " + ex.Message);
                // Puedes realizar acciones adicionales, como cerrar conexiones o reiniciar el proceso.
            }
            catch (MqttCommunicationTimedOutException)
            {
                Console.WriteLine("Tiempo de reconexion MQTT Excedido");
                return;
            }
            catch (MqttConnectingFailedException)
            {
                Console.WriteLine("Conexion Mqtt Fallida");
                return;
            }
            catch (MqttCommunicationException)
            {
                Console.WriteLine("Error intentando Conectar");
                return;
            }
            catch (Exception ex)
            {
                // Handle any exception that may occur during reconnection
                Console.WriteLine($"Failed to reconnect to MQTT broker. Exception: {ex}");

                // Wait for a while before attempting reconnection again
                await Task.Delay(TimeSpan.FromSeconds(5)); // Wait 5 seconds before retrying
            }
        }
            
           
        //Method to suscribes topic on MQTT
        public async Task SubscribeAsync(string topic)
        {
            try
            {
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
            }
            catch (MqttCommunicationException ex)
            {
                Console.WriteLine("Client MQTT Desconectado: " + ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

        }

        //Method to receive message by MQTT
        public void StartReceivingMessages(string HotelName, Action<string> messageHandler)
        {
            mqttClient.ApplicationMessageReceivedAsync += (async e =>
            {

                string cadena = e.ApplicationMessage.Topic;
                string reformatName = HotelName.Replace(" ", "");

                int stringLegnth = reformatName.Length + 14;
                string subcadena = cadena.Substring(0, stringLegnth);
              
                string stringCompare = $"{reformatName}/MONITOR4/Room";

                Console.WriteLine(cadena);

                if (subcadena == stringCompare)
                {

                    var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                    /*
                    Console.WriteLine("Received MQTT message");
                    Console.WriteLine($" - Topic = {e.ApplicationMessage.Topic}");
                    Console.WriteLine($" - Payload = {payload}");
                    Console.WriteLine($" - QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                    Console.WriteLine($" - Retain = {e.ApplicationMessage.Retain}");
                    Console.WriteLine("");
                    */

                    messageHandler(payload);
                }
            });

        }

        //Method to send MQTT messages
        public async Task sendMessage(string message, string topic)
        {

            Console.WriteLine("Publish MQTT message");
            Console.WriteLine($" - Topic: {topic}");
            Console.WriteLine($" - Payload: {message}");
            Console.WriteLine("");

            var applicationMessage = new MqttApplicationMessageBuilder()
              .WithTopic(topic)
              .WithPayload(message)
              .Build();

            try
            {

                await mqttClient.PublishAsync(applicationMessage);
            }
            catch(MqttCommunicationException ex)
            {
                Console.WriteLine("Client MQTT Desconectado: " + ex.Message);
                return;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

        }

        //Check MMQTT connection
        public bool checkConnection()
        {
            return mqttClient.IsConnected;
        }

        //Reconect MQTT !Dont use
        public async Task ReconectMqtt()
        {
            while (!checkConnection())
            {
                await mqttClient.ReconnectAsync();
            }
        }

    }
}
