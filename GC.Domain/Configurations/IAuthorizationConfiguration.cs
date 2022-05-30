using System;
namespace GC.Domain.Configurations
{
    public interface IAuthorizationConfiguration
    {
        string EncryptionKey { get; }

        int TokenLifetimeMinutes { get; }
    }
}
