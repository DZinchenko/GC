using System;
using System.Security.Claims;
using GC.Data.Objects;
using GC.Domain.Services;
using GC.Domain.Configurations;
using System.Threading.Tasks;
using GC.Domain.Services.Repositories;
using FluentValidation.Results;
using System.Collections.Generic;
using FluentValidation;

namespace GC.Domain.RequestHandlers
{
    public class LoginRequestHandler
    {
        private readonly IAuthContextInitializer authContextInitializer;
        private readonly IAuthorizationConfiguration authorizationConfiguration;
        private readonly ITokenService tokenService;
        private readonly IPasswordService passwordService;
        private readonly IUserRepository userRepository;

        public LoginRequestHandler(
            IAuthContextInitializer authContextInitializer,
            IAuthorizationConfiguration authorizationConfiguration,
            ITokenService tokenService,
            IPasswordService passwordService,
            IUserRepository userRepository)
        {
            this.authContextInitializer = authContextInitializer;
            this.authorizationConfiguration = authorizationConfiguration;
            this.tokenService = tokenService;
            this.passwordService = passwordService;
            this.userRepository = userRepository;
        }

        public async Task<LoginResponse> Process(LoginRequest request)
        {
            var user = await this.userRepository.GetUser(request.Login);

            var credentialsMatch = await this.passwordService.CheckPassword(HashAndSalt.FromUser(user), request.Password);

            if (!credentialsMatch)
            {
                throw new ValidationException(new List<ValidationFailure>() {
                    new ValidationFailure(nameof(request.Password), "Password does not match an email")
                });
            }

            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            });
            var lifetime = new TimeSpan(0, authorizationConfiguration.TokenLifetimeMinutes, 0);
            var responce = new LoginResponse { Token = await this.tokenService.GenerateToken(claimsIdentity, lifetime) };

            await this.authContextInitializer.InitAuthContext(user.Login);

            return responce;
        }
    }

    public class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}
