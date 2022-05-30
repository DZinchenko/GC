using System;
namespace GC.Adapters.EF
{
    public class DbContextFactory : IDBContextFactory
    {
        private readonly IEFConfiguration configuration;

        public DbContextFactory(IEFConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public CGDbContext Get()
        {
            return new CGDbContext(this.configuration);
        }
    }
}
