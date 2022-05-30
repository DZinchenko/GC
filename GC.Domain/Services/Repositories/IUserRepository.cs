using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GC.Data.Objects;

namespace GC.Domain.Services.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUser(int id);

        Task<User> GetUser(string login);

        Task<User> UpdateUser(User user);

        Task<User> AddUser(User user);

        Task<bool> IsLoginInUse(string login);

        Task<List<User>> GetDrivers();
    }
}
