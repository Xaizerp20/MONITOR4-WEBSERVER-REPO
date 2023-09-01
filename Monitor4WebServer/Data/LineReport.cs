namespace Monitor4WebServer.Data
{
    public class LineReport
    {
        public string subtext;
        public int longitud;
        public string texto;
        DateTime DateTimeNulo = new DateTime(1, 1, 1, 0, 0, 0);
        public int Id { get; set; }

        public int ID_HABITACION { get; set; }
        public string NombreLim { get; set; }
        public string NombreCheck { get; set; }
        public string NombreMantenimiento { get; set; }

        public int IdLim { get; set; }
        public int IdCheck { get; set; }
        public int IdMant { get; set; }

        public string Turno { get; set; }
        public string TurnoSalida { get; set; }

        public DateTime Entrada { get; set; }//hora y fecha de entrada a ocupada
        public DateTime Salida { get; set; }//hora y fecha de salida ocupacion

        public DateTime EntradaMant { get; set; }//hora y fecha de entrada a mantenimineto
        public DateTime SalidaMant { get; set; }//hora y fecha de salida del mantenimiento

        public DateTime EntradaAveria { get; set; }
        public DateTime SalidaAveria { get; set; }

        public DateTime Limpieza { get; set; } //hora y fecha de salida limpieza
        public DateTime Check { get; set; }//hora y fecha de salida del chequeo

        public TimeSpan TiempoOcupacion { get; set; }
        public TimeSpan TiempoOcupacionMant { get; set; }
        public TimeSpan TiempoLimpieza { get; set; }
        public TimeSpan TiempoCheck { get; set; }
        public TimeSpan TiempoAveria { get; set; }

        public string strTiempoOcupacion { get; set; }
        public string strTiempoOcupacionMant { get; set; }
        public string strTiempoLimpieza { get; set; }
        public string strTiempoCheck { get; set; }
        public string strTiempoAveria { get; set; }




        //hacer un constructor con valores conocidos de inicio
        public LineReport()
        {
            DateTime q = new DateTime(1, 1, 1, 0, 0, 0);//crea fecha falsa inicial
            NombreLim = "";
            NombreCheck = "";
            NombreMantenimiento = "";
            IdLim = 0;
            IdCheck = 0;
            IdMant = 0;
            Turno = "";
            TurnoSalida = "";
            Entrada = q;
            Salida = q;
            EntradaMant = q;
            SalidaMant = q;
            EntradaAveria = q;
            SalidaAveria = q;
            Limpieza = q;
            Check = q;
            strTiempoOcupacion = "";
            strTiempoOcupacionMant = "";
            strTiempoLimpieza = "";
            strTiempoCheck = "";
            strTiempoAveria = "";
            TiempoOcupacion = new TimeSpan();
            TiempoOcupacionMant = new TimeSpan();
            TiempoLimpieza = new TimeSpan();
            TiempoCheck = new TimeSpan();
            TiempoAveria = new TimeSpan();
        }


    }
}
