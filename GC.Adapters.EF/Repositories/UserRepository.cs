using System;
using System.Linq;
using System.Threading.Tasks;
using GC.Data.Objects;
using GC.Domain.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GC.Adapters.EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDBContextFactory dBContextFactory;

        public UserRepository(IDBContextFactory dBContextFactory)
        {
            this.dBContextFactory = dBContextFactory;
        }

        public async Task<User> AddUser(User user)
        {
            if (user.Id != 0)
            {
                throw new InvalidOperationException();
            }

            using var dbContext = dBContextFactory.Get();

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUser(int id)
        {
            using var dbContext = dBContextFactory.Get();
            return await dbContext.Users.SingleOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User> GetUser(string login)
        {
            using var dbContext = dBContextFactory.Get();
            return await dbContext.Users.SingleOrDefaultAsync(user => user.Login == login);
        }

        public async Task<User> UpdateUser(User user)
        {
            if (user.Id == 0)
            {
                throw new InvalidOperationException();
            }

            using var dbContext = dBContextFactory.Get();
            dbContext.Users.Attach(user).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> IsLoginInUse(string login)
        {
            using var dbContext = dBContextFactory.Get();

            return await dbContext.Users.AnyAsync(x => x.Login == login);
        }
    }
}
