using System;
using System.Collections.Generic;
using System.Text;

namespace ShieldAI.Service
{

    public class FlightLogMetrics
    {
        public int FlightCount { get; set; }
        public int HightestDuration { get; set; }
        public int LowestDuration { get; set; }
        public DateTime FirstFlight { get; set; }
        public DateTime MostRecentFlight { get; set; }
        public string LaziestDroneName { get; set; }
        public int LaziestDroneId { get; set; }
        public int LaziestDroneMissions { get; set; }
        public string BusiestDroneName { get; set; }
        public int BusiestDroneId { get; set; }
        public int BusiestDroneMissions { get; set; }
        public int BusiestGeneration { get; set; }
        public int BusiestGenerationMissions { get; set; }
    }
}
