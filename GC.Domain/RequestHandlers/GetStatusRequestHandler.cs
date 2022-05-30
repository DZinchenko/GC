using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GC.Data.Objects;
using GC.Domain.Services;
using GC.Domain.Services.Repositories;

namespace GC.Domain.RequestHandlers
{
    public class GetStatusRequestHandler
    {
        private readonly IPathRepository pathRepository;
        private readonly ICarRepository carRepository;
        private readonly IAuthContext authContext;

        public GetStatusRequestHandler(
            IPathRepository pathRepository,
            IAuthContext authContext,
            ICarRepository carRepository)
        {
            this.pathRepository = pathRepository;
            this.authContext = authContext;
            this.carRepository = carRepository;
        }

        public async Task<GetStatusResponce> Process()
        {
            if (this.authContext.CurrentUser == null)
            {
                throw new Exception("An attempt to get status without authorized user");
            }

            var path = await this.pathRepository.GetLastPathForDriver(this.authContext.CurrentUser.Id);

            return new GetStatusResponce
            {
                UserName = this.authContext.CurrentUser.Name,
                AssignedCar = await this.carRepository.GetAssignedCarForUser(this.authContext.CurrentUser.Id),
                PathParts = path == null ? null : await this.pathRepository.GetPathPartsForPath(path.Id);
            }
        }
    }

    public class GetStatusResponce
    {
        public string UserName { get; set; }

        public Car AssignedCar { get; set; }

        public List<PathPart> PathParts { get; set; }
    }
}
