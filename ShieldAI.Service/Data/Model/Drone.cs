using System;
using System.Collections.Generic;
using System.Text;

namespace ShieldAI.Service.Data.Model
{
    public class Drone
    {
        public int DroneId { get; set; }

        public string Name { get; set; }

        public int CurrentGeneration { get; set; }

        public bool IsActive { get; set; }
    }
}
