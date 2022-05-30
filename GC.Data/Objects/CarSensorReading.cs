using System;
namespace GC.Data.Objects
{
    public class CarSensorReading
    {
        public int Id { get; set; }

        public int CarSensorId { get; set; }

        public double Depth { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime Time { get; set; }
    }
}
