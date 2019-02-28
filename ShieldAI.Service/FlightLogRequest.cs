using System;
using System.Collections.Generic;
using System.Text;

namespace ShieldAI.Service
{
    public class FlightLogRequest
    {
        public int DroneId { get; set; }

        public double? Latitude { get; set; }
        
        public double? Longitude { get; set; }

        public double? Distance { get; set; }
    }
}
