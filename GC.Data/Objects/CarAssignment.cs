using System;
namespace GC.Data.Objects
{
    public class CarAssignment
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CarId { get; set; }

        public DateTime AssignmentTime { get; set; }
    }
}
