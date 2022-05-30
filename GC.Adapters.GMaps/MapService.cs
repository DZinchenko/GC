using System;
using System.Threading.Tasks;
using GC.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using GC.API;
using System.Text.Json;

namespace GC.Adapters.GMaps
{
    public class MapService : IMapsService
    {
        private readonly IGMapsConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;

        public MapService(IGMapsConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<MapsDistanceResponseDTO> GetDistances(MapsDistanceRequestDTO requestDTO)
        {
            var request = new DistanceMatrixRequest
            {
                Origins = requestDTO.StartLocations.Select(loc => LocationToString(loc)).ToList(),
                Destinations = requestDTO.DestinationLocations.Select(loc => LocationToString(loc)).ToList(),
                Key = this.configuration.ApiKey
            };

            var requestUri = new Uri(this.configuration.DistanceMatrixEnpointURL + "?" + StringTools.GetQueryString(request));

            var client = this.httpClientFactory.CreateClient();

            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri));

            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responce = JsonSerializer.Deserialize<DistanceMatrixResponse>(responseString);
                return new MapsDistanceResponseDTO
                {
                    Distances = responce.Rows.Select(r => r.Elements.Select(el => (double)el.Distance.Value).ToList()).ToList()
                };
            }
            else
            {
                throw new Exception("Bad responce from GMaps DistanceMatrix api.");
            }
        }

        private string LocationToString(MapsLocationDTO locationDTO)
        {
            return $"{locationDTO.Latitude} {locationDTO.Longitude}";
        }
    }
}
