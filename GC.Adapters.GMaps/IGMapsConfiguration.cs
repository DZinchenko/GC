using System;
namespace GC.Adapters.GMaps
{
    public interface IGMapsConfiguration
    {
        string DistanceMatrixEnpointURL { get; }

        string ApiKey { get; }
    }
}
