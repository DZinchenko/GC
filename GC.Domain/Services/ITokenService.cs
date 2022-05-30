using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GC.Domain.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ClaimsIdentity withClaims, TimeSpan lifetime);

        Task<List<Claim>> ReadClaims(string token);
    }
}
