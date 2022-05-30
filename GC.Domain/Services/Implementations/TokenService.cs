using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GC.Domain.Configurations;
using Microsoft.IdentityModel.Tokens;

namespace GC.Domain.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IAuthorizationConfiguration authorizationConfiguration;

        public TokenService(IAuthorizationConfiguration authorizationConfiguration)
        {
            this.authorizationConfiguration = authorizationConfiguration;
        }

        public Task<List<Claim>> ReadClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.authorizationConfiguration.EncryptionKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken securityToken);

                var claims = (securityToken as JwtSecurityToken).Claims;

                return Task.FromResult(claims.ToList());
            }
            catch
            {
                return Task.FromResult<List<Claim>>(null);
            }
        }

        public Task<string> GenerateToken(ClaimsIdentity withClaims, TimeSpan lifetime)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.authorizationConfiguration.EncryptionKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = withClaims,
                Expires = DateTime.UtcNow.Add(lifetime),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}
