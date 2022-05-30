using System;
using System.Threading.Tasks;
using GC.Data.Objects;
using GC.Domain.Services.Repositories;

namespace GC.Domain.RequestHandlers
{
    public class AssignCarRequestHandler
    {
        private readonly ICarRepository carRepository;

        public AssignCarRequestHandler(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }

        public async Task Process(GetCarsRequest request)
        {
            var assignment = new CarAssignment
            {
                AssignmentTime = DateTime.UtcNow,
                CarId = request.CarId,
                UserId = request.UserId
            };

            await this.carRepository.CreateCarAssignment(assignment);
        }
    }

    public class GetCarsRequest
    {
        public int CarId { get; set; }

        public int UserId { get; set; }
    }
}
