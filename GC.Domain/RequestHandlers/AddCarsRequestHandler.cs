using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GC.Data.Objects;
using GC.Domain.Services.Repositories;

namespace GC.Domain.RequestHandlers
{
    public class AddCarsRequestHandler
    {
        private readonly ICarRepository carRepository;

        public AddCarsRequestHandler(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }

        public async Task Process(AddCarsRequest request)
        {
            if (request.Cars?.Any() ?? false)
            {
                var cars = await this.carRepository.CreateCars(
                    request.Cars.Select(x => new Car
                    {
                        MaxDepth = x.MaxDepth,
                        Name = x.Name,
                        NumberPlate = x.NumberPlate,
                        Volume = x.Volume
                    }
                    ).ToList()
                );

                var sensors = new List<CarSensor>();
                for (int i = 0; i < cars.Count; i++)
                {
                    sensors.Add(new CarSensor
                    {
                        CarId = cars[i].Id,
                        DevEUI = request.Cars[i].SensorDevEUI
                    });
                }

                await this.carRepository.CreateCarSensors(sensors);
            }
        }
    }

    public class AddCarsRequest
    {
        public List<CarRequestPart> Cars { get; set; }

        public class CarRequestPart
        {
            public string NumberPlate { get; set; }

            public string Name { get; set; }

            public double MaxDepth { get; set; }

            public double Volume { get; set; }

            public int SensorDevEUI { get; set; }
        }
    }
}
