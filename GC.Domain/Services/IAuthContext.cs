using System;
using GC.Data.Objects;

namespace GC.Domain.Services
{
    public interface IAuthContext
    {
        public User CurrentUser { get; }

        public bool IsAuthorizationPerformed { get; }
    }
}
