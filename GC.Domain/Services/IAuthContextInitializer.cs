using System;
using System.Threading.Tasks;

namespace GC.Domain.Services
{
    public interface IAuthContextInitializer
    {
        IAuthContext GetCurrentContext();

        Task InitAuthContext(string userLogin);
    }
}
