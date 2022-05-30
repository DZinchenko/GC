using System;
using GC.Adapters.EF;
using GC.Domain.Configurations;
using Microsoft.Extensions.Configuration;

namespace GC.API
{
    public class AppSettingsConfiguration :
        IAuthorizationConfiguration,
        IEFConfiguration
    {
        private IConfiguration configuration;

        public AppSettingsConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string ConnectionString => this.configuration["EF:ConnectionString"];

        public string EncryptionKey => this.configuration["Auth:EncryptionKey"];

        public int TokenLifetimeMinutes => int.Parse(this.configuration["Auth:TokenLifetimeMinutes"]);
    }
}
