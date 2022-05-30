using System;

namespace GC.Adapters.EF
{
    public interface IEFConfiguration
    {
        public string ConnectionString { get; }
    }
}
