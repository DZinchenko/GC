using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GC.Data.Objects;

namespace GC.Domain.Services.Repositories
{
    public interface IPlaceRepository
    {
        public Task<List<Place>> CreatePlaces(List<Place> places);
        public Task<List<Place>> GetAllPlaces();
        public Task<List<BinSensor>> CreateBinSensors(List<BinSensor> binSensors);
        public Task<List<Distance>> CreateDistances(List<Distance> distances);
        public Task<Dictionary<(int,int), Distance>> GetDistancesWithPlaces(List<int> placeIds);
        public Task<BinSensorReading> CreateBinSensorReading(BinSensorReading reading);
        public Task<Dictionary<int,BinSensorReading>> GetLastBinSensorReadingsForBins(List<int> binIds);
    }
}
