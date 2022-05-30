using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GC.Data.Objects;
using GC.Domain.Services.Repositories;

namespace GC.Domain.RequestHandlers
{
    public class GetDriversRequestHandler
    {
        private readonly IUserRepository userRepository;

        public GetDriversRequestHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<GetDriversResponce> Process()
        {
            return new GetDriversResponce() { Drivers = await this.userRepository.GetDrivers() };
        }
    }

    public class GetDriversResponce
    {
        public List<User> Drivers { get; set; }
    }
}
