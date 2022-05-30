using System;
using GC.Data.Objects;

namespace GC.Domain.Services.Implementations
{
    public class AuthContext : IAuthContext
    {
        public User CurrentUser { get; init; }

        public bool IsAuthorizationPerformed { get; init; }
    }
}
