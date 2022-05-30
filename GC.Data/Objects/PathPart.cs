using System;
namespace GC.Data.Objects
{
    public class PathPart
    {
        public int Id { get; set; }

        public int InPathId { get; set; }

        public int PathId { get; set; }

        public int PlaceId { get; set; }

        public DateTime? CompletionTime { get; set; }
    }
}
