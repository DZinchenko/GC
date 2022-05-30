using System;
using System.Threading.Tasks;
using GC.Domain.RequestHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GC.API.Controllers
{
    [ApiController]
    [Route("places")]
    public class PlacesController
    {
        private readonly AddPlacesRequestHandler addPlacesRequestHandler;

        public PlacesController(AddPlacesRequestHandler addPlacesRequestHandler)
        {
            this.addPlacesRequestHandler = addPlacesRequestHandler;
        }

        [HttpPost]
        [Route("add")]
        [AllowAnonymous]
        public async Task AddPlaces([FromBody] AddPlacesRequest addPlacesRequest)
        {
            await this.addPlacesRequestHandler.Process(addPlacesRequest);
        }
    }
}
