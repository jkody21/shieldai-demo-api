using System;

namespace ShieldAI.Service.Data.Model {
    public class FlightLog
    {
        public long FlightLogId { get; set; }

        public int DroneId { get; set; }

        public int DroneGeneration { get; set; }

        public DateTime BeginOn { get; set; }

        public DateTime EndOn { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string MapPath { get; set; }
    }
}
