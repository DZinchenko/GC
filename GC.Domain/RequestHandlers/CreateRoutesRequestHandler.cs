using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GC.Data.Objects;
using GC.Domain.Configurations;
using GC.Domain.Services.Repositories;

namespace GC.Domain.RequestHandlers
{
    public class CreatePathsRequestHandler
    {
        private readonly IPathRepository pathRepository;
        private readonly IPlaceRepository placeRepository;
        private readonly IUserRepository userRepository;
        private readonly ICarRepository carRepository;
        private readonly IDomainConfiguration domainConfiguration;

        public CreatePathsRequestHandler(
            IPathRepository pathRepository,
            IPlaceRepository placeRepository,
            IUserRepository userRepository,
            ICarRepository carRepository,
            IDomainConfiguration domainConfiguration)
        {
            this.pathRepository = pathRepository;
            this.placeRepository = placeRepository;
            this.userRepository = userRepository;
            this.carRepository = carRepository;
            this.domainConfiguration = domainConfiguration;
        }

        public async Task Process()
        {
            var places = await this.placeRepository.GetAllPlaces();
            var bins = places.Where(pl => pl is Bin).Select(pl => pl as Bin);
            var readings = await this.placeRepository.GetLastBinSensorReadingsForBins(bins.Select(b => b.Id).ToList());
            bins = bins
                .Where(b =>
                {
                    return readings[b.Id].Depth / b.MaxDepth > this.domainConfiguration.MinRemovalFillLevel;
                });

            places = places.Where(pl => pl is Bin ? bins.Contains(pl) : true).ToList();
            var distances = await this.placeRepository.GetDistancesWithPlaces(places.Select(pl => pl.Id).ToList());

            var cars = await this.carRepository.GetAssignedCars();

            var bestPathLen = double.MaxValue;
            var bestPath = new List<Place>();
            var currCarInd = 0;

            void DFS(double lengh,
                     double tankFill,
                     IEnumerable<Place> visited)
            {
                var gotAllBins = visited.Where(pl => pl is Bin).Intersect(bins).Count() == bins.Count();

                foreach(var place in places)
                {
                    var addLen = distances[(visited.Last().Id, place.Id)].distanceKm;
                    double addFill = 0;

                    if (gotAllBins && ((tankFill == 0 && place is not Station) || (tankFill > 0 && place is not Dump)))
                    {
                        continue;
                    }
                    else if (gotAllBins && place is Station)
                    {
                        if (lengh < bestPathLen)
                        {
                            bestPathLen = lengh;
                            bestPath = visited.ToList();
                        }
                    }

                    if (place is Bin)
                    {
                        if (visited.Contains(place))
                        {
                            continue;
                        }
                        else
                        {
                            var bin = place as Bin;
                            addFill = bin.Volume * readings[bin.Id].Depth / bin.MaxDepth;

                            if (tankFill + addFill > cars[currCarInd].Volume)
                            {
                                continue;
                            }
                        }
                    }
                    else if (place is Dump)
                    {
                        addFill = -tankFill;
                    }
                    else
                    {
                        currCarInd = currCarInd + 1 < cars.Count() - 1 ? currCarInd + 1 : 0;
                    }

                    DFS(lengh + addLen, tankFill + addFill, visited.Append(place));
                }
            }

            DFS(0, 0, new List<Place> { places.First(pl => pl is Station)});

            var pathPlacesGrouped = new List<List<Place>>();
            var currPathGroupInd = 0;
            foreach(var place in bestPath)
            {
                if (place is Station)
                {
                    if(pathPlacesGrouped[currPathGroupInd]?.Any() ?? false)
                    {
                        currPathGroupInd++;
                        pathPlacesGrouped[currPathGroupInd - 1].Add(place);
                    }

                    pathPlacesGrouped[currPathGroupInd] = new List<Place>();
                }
                pathPlacesGrouped[currPathGroupInd].Add(place);
            }

            var newPaths = new List<Path>();
            for(int i = 0; i <= currPathGroupInd; i++)
            {
                newPaths.Add(new Path()
                {
                    UserId = (await this.carRepository.GetCarAssignmentForCar(cars[cars.Count() % i].Id)).UserId
                });
            }

            newPaths = await this.pathRepository.CreatePaths(newPaths);

            var newPathParts = new List<PathPart>();
            for (int i = 0; i <= currPathGroupInd; i++)
            {
                for (int j = 0; j < pathPlacesGrouped[i].Count(); j++)
                {
                    newPathParts.Add(new PathPart
                    {
                        InPathId = j,
                        PathId = newPaths[i].Id,
                        PlaceId = pathPlacesGrouped[i][j].Id
                    });
                }
            }

            await this.pathRepository.CreatePathParts(newPathParts);
        }
    }
}
