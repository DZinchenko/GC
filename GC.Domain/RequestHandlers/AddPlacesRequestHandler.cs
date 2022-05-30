using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GC.Data.Objects;
using GC.Domain.Services;
using GC.Domain.Services.Repositories;

namespace GC.Domain.RequestHandlers
{
    public class AddPlacesRequestHandler
    {
        private readonly IPlaceRepository placeRepository;
        private readonly IMapsService mapsService;

        public AddPlacesRequestHandler(IPlaceRepository placeRepository, IMapsService mapsService)
        {
            this.placeRepository = placeRepository;
            this.mapsService = mapsService;
        }

        public async Task Process(AddPlacesRequest request)
        {
            var bins = request.Bins?.Select(b => new Bin
            {
                Latitude = b.Latitude,
                Longitude = b.Longitude,
                MaxDepth = b.MaxDepth,
                Volume = b.Volume
            } as Place).ToList() ?? new List<Place>();

            var dumps = request.Dumps?.Select(b => new Dump
            {
                Latitude = b.Latitude,
                Longitude = b.Longitude
            } as Place).ToList() ?? new List<Place>();

            var stations = request.Stations?.Select(b => new Station
            {
                Latitude = b.Latitude,
                Longitude = b.Longitude
            } as Place).ToList() ?? new List<Place>();

            var allPlaces = await this.placeRepository.GetAllPlaces();
            var places = await this.placeRepository.CreatePlaces(bins.Concat(dumps).Concat(stations).ToList());

            bins = places.Where(x => x is Bin).ToList();

            if(bins.Any())
            {
                var sensors = new List<BinSensor>();
                for (int i = 0; i < bins.Count; i++)
                {
                    sensors.Add(new BinSensor
                    {
                        BinId = bins[i].Id,
                        DevEUI = request.Bins[i].SensorDevEUI
                    });
                }
                await this.placeRepository.CreateBinSensors(sensors);
            }

            var distances = new List<Distance>();
            foreach (var place in places)
            {
                var thisLoc = new List<MapsLocationDTO> { new MapsLocationDTO(place) };
                var otherLoc = allPlaces.Select(pl => new MapsLocationDTO(pl)).ToList();

                var distancesFrom = (await this.mapsService.GetDistances(new MapsDistanceRequestDTO
                {
                    StartLocations = thisLoc,
                    DestinationLocations = otherLoc
                })).Distances.First();

                var distancesTo = (await this.mapsService.GetDistances(new MapsDistanceRequestDTO
                {
                    StartLocations = otherLoc,
                    DestinationLocations = thisLoc
                })).Distances.Select(x => x.First());

                for(int i = 0; i < distancesFrom.Count(); i++)
                {
                    distances.Add(new Distance { Place1Id = place.Id, Place2Id = allPlaces[i].Id, distanceKm = distancesFrom[i] });
                }

                for (int i = 0; i < distancesTo.Count(); i++)
                {
                    distances.Add(new Distance { Place1Id = allPlaces[i].Id, Place2Id = place.Id, distanceKm = distancesFrom[i] });
                }
            }

            await this.placeRepository.CreateDistances(distances);
        }
    }

    public class AddPlacesRequest
    {
        public List<BinRequestPart> Bins { get; set; }

        public List<DumpStationRequestPart> Dumps { get; set; }

        public List<DumpStationRequestPart> Stations { get; set; }

        public class BinRequestPart
        {
            public double Latitude { get; set; }

            public double Longitude { get; set; }

            public double MaxDepth { get; set; }

            public double Volume { get; set; }

            public int SensorDevEUI { get; set; }
        }

        public class DumpStationRequestPart
        {
            public double Latitude { get; set; }

            public double Longitude { get; set; }
        }
    }
}
