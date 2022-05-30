using System;
using System.Threading.Tasks;
using GC.Data.Objects;
using GC.Domain.Services.Repositories;

namespace GC.Domain.Services.Implementations
{
    public class AuthContextInitializer: IAuthContextInitializer
    {
        private IAuthContext context = new AuthContext() { IsAuthorizationPerformed = false };
        private readonly IUserRepository userAuthorizationRepository;

        public AuthContextInitializer(IUserRepository userAuthorizationRepository)
        {
            this.userAuthorizationRepository = userAuthorizationRepository;
        }

        public IAuthContext GetCurrentContext()
        {
            return this.context;
        }

        public async Task InitAuthContext(string userLogin)
        {
            var user = await this.userAuthorizationRepository.GetUser(userLogin);

            this.context = new AuthContext()
            {
                CurrentUser = user,
                IsAuthorizationPerformed = true
            };
        }
    }
}
