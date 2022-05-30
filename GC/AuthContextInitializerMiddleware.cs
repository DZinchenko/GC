using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GC.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace GC.API
{
    public class AuthContextInitializerMiddleware : IMiddleware
    {
        private readonly IAuthContextInitializer authContextInitializer;

        public AuthContextInitializerMiddleware(
            IAuthContextInitializer authContextInitializer
            )
        {
            this.authContextInitializer = authContextInitializer;
        }


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var emailClaim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            await this.authContextInitializer.InitAuthContext(emailClaim);

            await next(context);
        }
    }
}
