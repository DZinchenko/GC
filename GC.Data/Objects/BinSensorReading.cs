using System;
namespace GC.Data.Objects
{
    public class BinSensorReading
    {
        public int Id { get; set; }

        public int BinSensorId { get; set; }

        public double Depth { get; set; }

        public DateTime Time { get; set; }
    }
}
