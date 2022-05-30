using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GC.Data.Objects;
using GC.Domain.Services.Repositories;

namespace GC.Domain.RequestHandlers
{
    public class GetCarsRequestHandler
    {
        private readonly ICarRepository carRepository;

        public GetCarsRequestHandler(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }

        public async Task<GetCarsResponce> Process()
        {
            return new GetCarsResponce() { Drivers = await this.carRepository.GetCars() };
        }
    }

    public class GetCarsResponce
    {
        public List<Car> Drivers { get; set; }
    }
}
