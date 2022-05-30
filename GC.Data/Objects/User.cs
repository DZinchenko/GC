using System;
namespace GC.Data.Objects
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public UserRole Role { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }
    }

    public enum UserRole
    {
        Admin = 1,
        Driver = 2
    }
}
