using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GC.Adapters.EF
{
    public class CGDBContextFactory : IDesignTimeDbContextFactory<CGDbContext>
    {
        public CGDbContext CreateDbContext(string[] args)
        {
            return new CGDbContext(new DesignTimeEFConfiguration());
        }
    }

    public class DesignTimeEFConfiguration : IEFConfiguration
    {
        private IConfiguration configuration;

        public DesignTimeEFConfiguration()
        {
            var path = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName, @"GC/appsettings.json");
            this.configuration = new ConfigurationBuilder()
                    .AddJsonFile(path)
                    .Build();
        }

        public string ConnectionString
        {
            get
            {
                return this.configuration["EF:ConnectionString"];
            }
        }
    }
}
