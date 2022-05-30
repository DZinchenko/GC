using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GC.Data.Objects;

namespace GC.Domain.Services
{
    public interface IMapsService
    {
        public Task<MapsDistanceResponseDTO> GetDistances(MapsDistanceRequestDTO requestDTO);
    }

    public class MapsLocationDTO
    {
        public MapsLocationDTO(Place place)
        {
            this.Latitude = place.Latitude;
            this.Longitude = place.Longitude;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }

    public class MapsDistanceRequestDTO
    {
        public List<MapsLocationDTO> StartLocations { get; set; }

        public List<MapsLocationDTO> DestinationLocations { get; set; }
    }

    public class MapsDistanceResponseDTO
    {
        public List<List<double>> Distances { get; set; }
    }
}
