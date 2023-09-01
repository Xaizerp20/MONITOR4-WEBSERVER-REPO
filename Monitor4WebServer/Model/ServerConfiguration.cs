namespace Monitor4WebServer.Model
{
    public class ServerConfiguration
    {
            public string HotelName { get; set; } = null!;
            public string MqttBroker { get; set; } = null!;
            public string MqttPort { get; set; } = null!;
            public string MqttClientName { get; set; } = null!;
            public string MqttTopic { get; set; } = null!;
            public int TurnosQuantity { get; set; }
            public int RoomsQuantity { get; set; }
            public string EmailHost { get; set; } = null!;
            public string EmailPort { get; set; } = null!;
            public string EmailRemitente { get; set; } = null!;
            public string EmailPassword { get; set; } = null!;
            public List<string> EmailDestinatarios { get; set; } = null!;
            public Dictionary<int, TimeSpan?> TimeTurnos { get; set; } = null!;
            public Dictionary<string, string> BoardConfig { get; set; } = null!;

        public ServerConfiguration()
        {

        }

    }
}
