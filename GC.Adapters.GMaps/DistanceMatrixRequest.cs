using System;
using System.Collections.Generic;

namespace GC.Adapters.GMaps
{
    public class DistanceMatrixRequest
    {
        public List<string> Origins { get; set; }

        public List<string> Destinations { get; set; }

        public string Units { get; } = "metric";

        public string Key { get; set; }
    }
}
