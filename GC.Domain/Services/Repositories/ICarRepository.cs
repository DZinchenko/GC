using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GC.Data.Objects;

namespace GC.Domain.Services.Repositories
{
    public interface ICarRepository
    {
        public Task<List<Car>> CreateCars(List<Car> cars);
        public Task<List<Car>> GetCars();
        public Task<Car> GetCar(int id);
        public Task<List<CarSensor>> CreateCarSensors(List<CarSensor> sensors);
        public Task<CarAssignment> CreateCarAssignment(CarAssignment assignment);
        public Task<List<Car>> GetAssignedCars();
        public Task<Car> GetAssignedCarForUser(int userId);
        public Task<CarAssignment> GetCarAssignmentForCar(int carId);
    }
}
