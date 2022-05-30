using System;
using Microsoft.AspNetCore.Mvc;
using GC.Domain.RequestHandlers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GC.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthorizationController : ControllerBase
    {
        private readonly LoginRequestHandler loginRequestHandler;

        public AuthorizationController(LoginRequestHandler loginRequestHandler)
        {
            this.loginRequestHandler = loginRequestHandler;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<LoginResponse> Login([FromBody] LoginRequest loginRequest)
        {
            return await this.loginRequestHandler.Process(loginRequest);
        }
    }
}
