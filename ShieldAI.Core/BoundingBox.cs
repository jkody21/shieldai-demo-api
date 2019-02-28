using System;
using System.Collections.Generic;
using System.Text;

namespace ShieldAI.Core
{
    public class BoundingBox
    {
        public double StartLongitude { get; set; }
        public double StartLatitude { get; set; }

        public double MinimumLongitude { get; set; }
        public double MaximumLongitude { get; set; }
        public double MinimumLatitude { get; set; }
        public double MaximumLatitude { get; set; }


        public double GetLowestLongitude {
            get { return Math.Min(MinimumLongitude, MaximumLongitude); }
        }

        public double GetHighestLongitude {
            get { return Math.Max(MinimumLongitude, MaximumLongitude); }
        }

        public double GetLowestLatitude {
            get { return Math.Min(MinimumLatitude, MaximumLatitude); }
        }

        public double GetHighestLatitude {
            get { return Math.Max(MinimumLatitude, MaximumLatitude); }
        }

    }
}
