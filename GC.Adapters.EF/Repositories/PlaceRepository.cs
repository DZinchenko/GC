using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GC.Data.Objects;
using GC.Domain.Services.Repositories;

namespace GC.Adapters.EF.Repositories
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly IDBContextFactory dBContextFactory;

        public PlaceRepository(IDBContextFactory dBContextFactory)
        {
            this.dBContextFactory = dBContextFactory;
        }

        public async Task<List<Place>> CreatePlaces(List<Place> places)
        {
            using var dbContext = this.dBContextFactory.Get();

            await dbContext.AddRangeAsync(places);
            await dbContext.SaveChangesAsync();

            return places;
        }

        public async Task<List<BinSensor>> CreateBinSensors(List<BinSensor> binSensors)
        {
            using var dbContext = this.dBContextFactory.Get();

            await dbContext.AddRangeAsync(binSensors);
            await dbContext.SaveChangesAsync();

            return binSensors;
        }
    }
}
