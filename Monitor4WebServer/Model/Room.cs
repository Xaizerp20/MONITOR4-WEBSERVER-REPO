using System.Globalization;
using System.Diagnostics;
using Syncfusion.Blazor.Diagrams;
using System.Reflection.Metadata;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Threading;
using Monitor4WebServer.Data;
using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.Blazor.Kanban.Internal;
using System;
using Monitor4WebServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Toolbelt.Blazor.SpeechSynthesis;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using static System.Runtime.InteropServices.JavaScript.JSType;
using LiteDB;
using static MudBlazor.Icons;
using static Monitor4WebServer.Pages.Index;

namespace Monitor4WebServer.Model
{

    public enum Estado
    {
        Init = 1,
        Error = 0,
        Libre = 4,
        Cerrada = 6,
        Ocupada = 3,
        Abierta = 7,
        Limpieza = 2,
        Mantenimiento = 5,
    }

    public enum CardId
    {
        Init = 1,
        Error = 0,
        Libre = 4,
        Cerrada = 6,
        Ocupada = 3,
        Abierta = 7,
        Limpieza = 2,
        Mantenimiento = 5,
    }



    public class Room
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public Estado CurrentState { get; set; } = Estado.Init;
        public Estado NewState { get; set; } = Estado.Init;
        public string Time { get; set; }



        private List<Stopwatch> RelogDecode = new List<Stopwatch>(10);
        private Timer timerError;


        public static int freeRooms;
        public static int busyRooms;

        public bool speechSynthesisRequested { get; set; } = false;
        public Estado oldState { get; set; } = Estado.Init;


        //private  LineReportDbService lineReportDB;

        //relojes
        //1 - Libre
        //2 - Cerrada
        //3 - Ocupada
        //4 - Abierta
        //5 - Cleaning
        //6 - Maintenance
        //7 - Check
        //8 - TimeAveria
        //9 - Fin_Trama
        //0 - Bit_Trama

        /*
        int ESTADO_ERROR = 0;
        int ESTADO_INIT = 1;
        int ESTADO_LIMPIEZA = 2;
        int ESTADO_OCUPADA = 3;
        int ESTADO_LIBRE = 4;
        int ESTADO_MANTENIMIENTO = 5;
        int ESTADO_CERRADA = 6;
        int ESTADO_ABIERTA = 7;
        */




        public Room(int Id)
        {


            this.Id = Id;
            this.CurrentState = Estado.Init;

            /*
            var optionsBuilder = new DbContextOptionsBuilder<LineReportContext>();
            optionsBuilder.UseSqlite(@"Data Source=C:\Users\Informatica\Desktop\MONITOR4DBSQLITE\LineReport.db");
            var context = new LineReportContext(optionsBuilder.Options);    
            lineReportDB = new LineReportDbService(context);
            */

            //create room timers
            for (int i = 0; i <= 10; i++)
            {
                RelogDecode.Add(new Stopwatch());
            }

            //timerError = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            //TIMER FOR ERROR
            timerError = new Timer();
            timerError.Elapsed += TimerCallback;
            timerError.Interval = TimeSpan.FromSeconds(20).TotalMilliseconds;
            timerError.AutoReset = true;
            StartTimerError(); //init timer
        }




        public void UpdateRoom(TURNO _turno, int cardId, LiteDatabase db, ILiteCollection<LineReport> col, LineReport findReportLine)
        {

            // Coloca aquí las acciones que deseas realizar para cada transición
            // Por ejemplo:
            switch (this.CurrentState)
            {
                case Estado.Libre: //si la habitacion esta libre
                    Console.WriteLine("la habitacion esta libre.");

                    if (this.oldState == Estado.Cerrada)
                    {
                        Console.WriteLine("la habitacion ha sido abierta y esta libre.");
                    }
                    else if (this.oldState == Estado.Mantenimiento)
                    {

                        #region ACTUALIZACON Y CIERRE DE DB AL PASAR DE MANTENIMINETO A LIBRE
                        findReportLine.SalidaMant = DateTime.Now;

                        TimeSpan maintainceStart = findReportLine.EntradaMant.TimeOfDay;
                        TimeSpan maintainceFinish = findReportLine.SalidaMant.TimeOfDay;
                        TimeSpan elpasedTime = maintainceFinish - maintainceStart;

                        findReportLine.TiempoOcupacionMant = elpasedTime;
                        findReportLine.strTiempoOcupacionMant = elpasedTime.ToString(@"hh\:mm\:ss");


                        col.Update(findReportLine);


                        #endregion

                        Console.WriteLine("la habitacion salio de Mantenimiento y esta libre.");
                    }
                    else if (this.oldState == Estado.Limpieza)
                    {

                        #region ACTUALIZACON Y CIERRE DE DB AL PASAR DE LIMPIEZA A LIBRE

                        TimeSpan cleanStart = findReportLine.Limpieza.TimeOfDay;
                        TimeSpan cleanFinish = DateTime.Now.TimeOfDay;
                        TimeSpan elpasedTime = cleanFinish - cleanStart;

                        findReportLine.TiempoLimpieza = elpasedTime;
                        findReportLine.strTiempoLimpieza = elpasedTime.ToString(@"hh\:mm\:ss");


                        col.Update(findReportLine);


                        #endregion

                        Console.WriteLine("la habitacion fue limpiada y esta libre.");
                    }
                    else if (this.oldState == Estado.Error)
                    {
                        #region ACTUALIZACON Y CIERRE DE DB AL SALIR DE ERORR

                        TimeSpan errorStart = findReportLine.EntradaAveria.TimeOfDay;
                        TimeSpan errorFinish = DateTime.Now.TimeOfDay;
                        TimeSpan elpasedTimeError = errorFinish - errorStart;

                        findReportLine.TiempoAveria = elpasedTimeError;
                        findReportLine.strTiempoAveria = elpasedTimeError.ToString(@"hh\:mm\:ss");


                        col.Update(findReportLine);


                        #endregion
                    }
                    break;

                case Estado.Cerrada: //si la habitacion esta cerrada
                    Console.WriteLine("La habitación está Cerrada.");

                    if (this.oldState == Estado.Libre)
                    {
                        Console.WriteLine("la habitacion paso de libre a cerrada.");
                    }
                    else if (this.oldState == Estado.Error)
                    {
                        #region ACTUALIZACON Y CIERRE DE DB AL SALIR DE ERORR

                        TimeSpan errorStart = findReportLine.EntradaAveria.TimeOfDay;
                        TimeSpan errorFinish = DateTime.Now.TimeOfDay;
                        TimeSpan elpasedTimeError = errorFinish - errorStart;

                        findReportLine.TiempoAveria = elpasedTimeError;
                        findReportLine.strTiempoAveria = elpasedTimeError.ToString(@"hh\:mm\:ss");


                        col.Update(findReportLine);


                        #endregion
                    }
                    break;


                case Estado.Ocupada:
                    Console.WriteLine("La habtacion esta ocupada");

                    if (this.oldState == Estado.Cerrada)
                    {
                        #region NUEVA LINEA DE REPORTE DE AL SALIR DE ERORR          

                        //create report Line

                        col.Insert(new LineReport
                        {
                            ID_HABITACION = this.Id,
                            Entrada = DateTime.Now,
                            Turno = "TARDE",
                        });


                        #endregion

                        Console.WriteLine("la habitacion paso de cerrada a ocupada");
                    }
                    else if (this.oldState == Estado.Abierta)
                    {
                        Console.WriteLine("la habitacion paso de abierta a ocupada");
                    }
                    else if (this.oldState == Estado.Error)
                    {
                        #region ACTUALIZACON Y CIERRE DE DB AL SALIR DE ERORR

                        TimeSpan errorStart = findReportLine.EntradaAveria.TimeOfDay;
                        TimeSpan errorFinish = DateTime.Now.TimeOfDay;
                        TimeSpan elpasedTimeError = errorFinish - errorStart;

                        findReportLine.TiempoAveria = elpasedTimeError;
                        findReportLine.strTiempoAveria = elpasedTimeError.ToString(@"hh\:mm\:ss");


                        col.Update(findReportLine);


                        #endregion
                    }
                    break;

                case Estado.Abierta:
                    Console.WriteLine("La habitación esta abierta");

                    if (this.oldState == Estado.Ocupada)
                    {
                        Console.WriteLine("La habitación fue abierta y paso de Ocupada a Abierta");
                    }
                    else if (this.oldState == Estado.Error)
                    {
                        #region ACTUALIZACON Y CIERRE DE DB AL SALIR DE ERORR

                        TimeSpan errorStart = findReportLine.EntradaAveria.TimeOfDay;
                        TimeSpan errorFinish = DateTime.Now.TimeOfDay;
                        TimeSpan elpasedTimeError = errorFinish - errorStart;

                        findReportLine.TiempoAveria = elpasedTimeError;
                        findReportLine.strTiempoAveria = elpasedTimeError.ToString(@"hh\:mm\:ss");


                        col.Update(findReportLine);


                        #endregion
                    }

                    break;

                case Estado.Limpieza:
                    Console.WriteLine("La habitación esta en proceso de Limpieza");

                    if (this.oldState == Estado.Abierta)
                    {

                        #region ACTUALIZACON Y CIERRE DE DB AL PASAR POR EL PROCESO DE OCUPADA HASTA LIMPIEZA

                    

                        if (findReportLine != null)
                        {
                            findReportLine.NombreLim = $"Tarjeta ID: {cardId}";
                            findReportLine.IdLim = cardId;
                            findReportLine.Limpieza = DateTime.Now;
                            findReportLine.Salida = DateTime.Now;

                            TimeSpan stampEnter = findReportLine.Entrada.TimeOfDay;
                            TimeSpan stampLeave = findReportLine.Salida.TimeOfDay;
                            TimeSpan elpasedTime = stampLeave - stampEnter;

                            findReportLine.TiempoOcupacion = elpasedTime;
                            findReportLine.strTiempoOcupacion = elpasedTime.ToString(@"hh\:mm\:ss");


                            col.Update(findReportLine); //update data



                        }
                        #endregion


                        Console.WriteLine("La habitación paso de Abierta a Limpieza");
                    }
                    else if (this.oldState == Estado.Error)
                    {
                        #region ACTUALIZACON Y CIERRE DE DB AL SALIR DE ERORR

                        TimeSpan errorStart = findReportLine.EntradaAveria.TimeOfDay;
                        TimeSpan errorFinish = DateTime.Now.TimeOfDay;
                        TimeSpan elpasedTimeError = errorFinish - errorStart;

                        findReportLine.TiempoAveria = elpasedTimeError;
                        findReportLine.strTiempoAveria = elpasedTimeError.ToString(@"hh\:mm\:ss");


                        col.Update(findReportLine);


                        #endregion
                    }
                    break;


                case Estado.Mantenimiento:

                    Console.WriteLine("La habitación esta en proceso de Mantenimiento");

                    if (this.oldState == Estado.Libre)
                    {

                        #region NUEVA LINEA DE REPORTE DE LIBRE A MANTENIMIENTO

                        //create report Line

                        col.Insert(new LineReport
                        {
                            ID_HABITACION = this.Id,
                            NombreMantenimiento = $"Tarjeta ID: {cardId}",
                            IdMant = cardId,
                            EntradaMant = DateTime.Now,
                            Turno = "TARDE",
                        });


                        #endregion

                        Console.WriteLine("La habitación estaba libre y paso a Mantenimiento");

                    }
                    else if (this.oldState == Estado.Limpieza)
                    {

                        #region ACTUALIZACON Y CIERRE DE DB AL PASAR DE LIMPIEZA A MANTENIMIENTO          

                        findReportLine.NombreMantenimiento = $"Tarjeta ID: {cardId}";
                        findReportLine.IdMant = cardId;


                        findReportLine.EntradaMant = DateTime.Now;

                        TimeSpan cleanStart = findReportLine.Limpieza.TimeOfDay;
                        TimeSpan cleanFinish = DateTime.Now.TimeOfDay;
                        TimeSpan elpasedTime = cleanFinish - cleanStart;

                        findReportLine.TiempoLimpieza = elpasedTime;
                        findReportLine.strTiempoLimpieza = elpasedTime.ToString(@"hh\:mm\:ss");


                        col.Update(findReportLine);

                        #endregion

                        Console.WriteLine("La habitación fue limpiada y paso a Mantenimiento");
                    }
                    else if (this.oldState == Estado.Error)
                    {
                        #region ACTUALIZACON Y CIERRE DE DB AL SALIR DE ERORR

                        col = db.GetCollection<LineReport>("linereport");
                        

                        TimeSpan errorStart = findReportLine.EntradaAveria.TimeOfDay;
                        TimeSpan errorFinish = DateTime.Now.TimeOfDay;
                        TimeSpan elpasedTimeError = errorFinish - errorStart;

                        findReportLine.TiempoAveria = elpasedTimeError;
                        findReportLine.strTiempoAveria = elpasedTimeError.ToString(@"hh\:mm\:ss");


                        col.Update(findReportLine);


                        #endregion
                    }
                    break;

                case Estado.Error:

                    //NO INTERESA EL ESTADO ANTERIOR

                    #region NUEVA LINEA DE REPORTE AL MARCAR ESTADO DE ERROR
                   

                        //create report Line
                        var NewLineReport = (new LineReport
                        {
                            ID_HABITACION = this.Id,
                            EntradaAveria = DateTime.Now,
                            Turno = "TARDE", //TODO: ARREGLAR TURNO
                        });

                        col.Insert(NewLineReport);

                    
                    #endregion
                    break;

                default:
                    Console.WriteLine("Error: La habitacion no responde ");
                    break;
            }
        }



        /*
        //save db data on state change
        public async Task DbStateChange(int roomOldState)
        {
            if (this.StateId == 3) //OCUPADA
            {


                await lineReportDB.AddLineReport(new LineReport
                {
                    ID_HABITACION = room,
                    Entrada = DateTime.Now,
                    Turno = "TARDE",
                });


            }

            else if (this.StateId == 2 && roomOldState == 7) //ABIERTA A LIMPIEZA
            {
                //LineReport? findReportLine = (await Localdb.GetAllDataAsync()).LastOrDefault(x => x.ID_HABITACION == room);

                findReportLine.NombreLim = $"Tarjeta ID: {cardId}";
                findReportLine.IdLim = cardId;
                findReportLine.Limpieza = DateTime.Now;
                findReportLine.Salida = DateTime.Now;

                TimeSpan stampEnter = findReportLine.Entrada.TimeOfDay;
                TimeSpan stampLeave = findReportLine.Salida.TimeOfDay;
                TimeSpan elpasedTime = stampLeave - stampEnter;

                findReportLine.TiempoOcupacion = elpasedTime;
                findReportLine.strTiempoOcupacion = elpasedTime.ToString(@"hh\:mm\:ss");


                if (findReportLine != null)
                {
                    //    await Localdb.UpdateDataAsync(findReportLine);

                }
            }

            else if (this.StateId == 4 && roomOldState == 2) //LIMPIEZA A LIBRE
            {
                //LineReport? findReportLine = (await Localdb.GetAllDataAsync()).LastOrDefault(x => x.ID_HABITACION == room);

                TimeSpan cleanStart = findReportLine.Limpieza.TimeOfDay;
                TimeSpan cleanFinish = DateTime.Now.TimeOfDay;
                TimeSpan elpasedTime = cleanFinish - cleanStart;

                findReportLine.TiempoLimpieza = elpasedTime;
                findReportLine.strTiempoLimpieza = elpasedTime.ToString(@"hh\:mm\:ss");


                // await Localdb.UpdateDataAsync(findReportLine);




            }

            else if (this.StateId == 5 && roomOldState == 2) //LIMPIEZA A MANTENIMIENTO
            {
                //LineReport? findReportLine = (await Localdb.GetAllDataAsync()).LastOrDefault(x => x.ID_HABITACION == room);

                findReportLine.NombreMantenimiento = $"Tarjeta ID: {cardId}";
                findReportLine.IdMant = cardId;


                findReportLine.EntradaMant = DateTime.Now;

                TimeSpan cleanStart = findReportLine.Limpieza.TimeOfDay;
                TimeSpan cleanFinish = DateTime.Now.TimeOfDay;
                TimeSpan elpasedTime = cleanFinish - cleanStart;

                findReportLine.TiempoLimpieza = elpasedTime;
                findReportLine.strTiempoLimpieza = elpasedTime.ToString(@"hh\:mm\:ss");


                // await Localdb.UpdateDataAsync(findReportLine);



            }

            else if (this.StateId == 4 && roomOldState == 5) //MANTENIMIENTO A LIBRE
            {
                Console.WriteLine("Habitaciones: " + room);
                LineReport? findReportLines = (await lineReportDB.GetAllLineReports()).LastOrDefault(x => x.ID_HABITACION == room);

                Console.WriteLine(findReportLines.ID_HABITACION);

                findReportLines.SalidaMant = DateTime.Now;

                TimeSpan maintainceStart = findReportLines.EntradaMant.TimeOfDay;
                TimeSpan maintainceFinish = findReportLines.SalidaMant.TimeOfDay;
                TimeSpan elpasedTime = maintainceFinish - maintainceStart;

                findReportLines.TiempoOcupacionMant = elpasedTime;
                findReportLines.strTiempoOcupacionMant = elpasedTime.ToString(@"hh\:mm\:ss");


                //  await Localdb.UpdateDataAsync(findReportLine);


            }

            else if (this.StateId == 5 && roomOldState == 4) //LIBRE A MANTENIMIENTO
            {

                await lineReportDB.AddLineReport(new LineReport
                {
                    ID_HABITACION = room,
                    NombreMantenimiento = $"Tarjeta ID: {cardId}",
                    IdMant = cardId,
                    EntradaMant = DateTime.Now,
                    Turno = "TARDE",
                });

            }

        }
        */


        //update actual time by state room
        public void UpdateTime()
        {

            Stopwatch UpWatch = new Stopwatch();

            UpWatch = RelogDecode[(int)this.CurrentState];

            Time = System.String.Format("{0:00}:{1:00}:{2:00}", UpWatch.Elapsed.Hours, UpWatch.Elapsed.Minutes, UpWatch.Elapsed.Seconds);

        }

        //init timer
        public void StartTimer(Estado state)
        {

            RelogDecode[((int)state)].Start();

        }

        //restar timers
        public void RestartAllTimers()
        {
            RelogDecode.ForEach(r =>
            {
                r.Reset();
            });

        }

        private void StartTimerError()
        {
            timerError.Start();
        }

        public void ResetTimerError()
        {

            timerError.Stop();
            timerError.Interval = TimeSpan.FromSeconds(180).TotalMilliseconds;
            timerError.Start();
        }

        private void TimerCallback(object sender, ElapsedEventArgs e)
        {
            // Actualizar el parámetro
            this.StateId = 0;
            this.NewState = Estado.Error;
            RelogDecode[this.StateId].Start();
        }

        private async Task CreateReportLine()
        {
            /*
            await lineReportDB.AddLineReport(new LineReport
            {
                ID_HABITACION = this.Id,
                Entrada = DateTime.Now,
                Turno = "TARDE",
            });
            */
        }

        public async Task PlaySpeechAsync(IJSRuntime _jsRuntime, string textToSpeech)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("startSpeechSynthesis", textToSpeech);
            }

            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
            }

        }

        public async void TextToSpeechAsync(SpeechSynthesis SpeechSynthesis, SpeechSynthesisUtterance utterancet, string text)
        {
            utterancet.Text = text;

            try
            {
                await SpeechSynthesis.SpeakAsync(utterancet); // 👈 Speak!
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }


        }

    }

}

