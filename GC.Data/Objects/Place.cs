using System;
namespace GC.Data.Objects
{
    public abstract class Place
    {
        public int Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }

    public class Bin : Place
    {
        public double MaxDepth { get; set; }

        public double Volume { get; set; }
    }

    public class Station : Place { }
    public class Dump : Place { }
}
