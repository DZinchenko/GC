using System;
namespace GC.Domain.Configurations
{
    public interface IDomainConfiguration
    {
        double MinRemovalFillLevel { get; }
        int[] PathRefreshTimesHour { get; }
    }
}
