using System;
namespace GC.Adapters.EF
{
    public interface IDBContextFactory
    {
        CGDbContext Get();
    }
}
