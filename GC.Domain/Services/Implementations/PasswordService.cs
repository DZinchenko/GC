using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace GC.Domain.Services.Implementations
{
    public class PasswordService : IPasswordService
    {
        public Task<bool> CheckPassword(HashAndSalt password, string plainPassword)
        {
            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                   password: plainPassword,
                   salt: Convert.FromBase64String(password.Salt),
                   prf: KeyDerivationPrf.HMACSHA256,
                   iterationCount: 100000,
                   numBytesRequested: 32));

            return Task.FromResult(hashedPassword == password.Hash);
        }

        public Task<HashAndSalt> GeneratePassword(string plainPassword)
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[16];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: plainPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32));

            return Task.FromResult(new HashAndSalt()
            {
                Hash = hashed,
                Salt = Convert.ToBase64String(salt)
            });
        }
    }
}
