using System;
using System.Threading.Tasks;
using GC.Data.Objects;

namespace GC.Domain.Services
{
    public interface IPasswordService
    {
        public Task<bool> CheckPassword(HashAndSalt password, string plainPassword);

        public Task<HashAndSalt> GeneratePassword(string plainPassword);
    }

    public class HashAndSalt
    {
        public string Hash { get; init; }
        public string Salt { get; init; }

        public static HashAndSalt FromUser(User user)
        {
            return new HashAndSalt()
            {
                Hash = user.PasswordHash,
                Salt = user.PasswordSalt
            };
        }
    }
}
