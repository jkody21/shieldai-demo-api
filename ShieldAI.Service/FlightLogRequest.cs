using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ShieldAI.Service
{
    public class FlightLogRequest
    {
        public int? DroneId { get; set; }

        public int? DroneGeneration { get; set; }

        public double? Latitude { get; set; }
        
        public double? Longitude { get; set; }

        public double? Distance { get; set; }

        public string Address { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public int? DurationLow { get; set; }

        public int? DurationHigh { get; set; }


        internal bool CanCreateBoundingBox {
            get {
                return Latitude.HasValue && Longitude.HasValue && Distance.HasValue;
            }
        }

        public double? MinLongitude { get; set; }

        public double? MaxLongitude { get; set; }

        public double? MinLatitude { get; set; }

        public double? MaxLatitude { get; set; }
    }
}
