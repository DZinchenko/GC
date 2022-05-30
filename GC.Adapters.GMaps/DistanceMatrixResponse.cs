using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GC.Adapters.GMaps
{
    public class DistanceMatrixResponse
    {
        [JsonPropertyName("destination_addresses")]
        public List<string> Destinations { get; set; }


        [JsonPropertyName("origin_addresses")]
        public List<string> Origins { get; set; }


        [JsonPropertyName("rows")]
        public List<DistanceMatrixResponceRow> Rows { get; set; }


        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class DistanceMatrixResponceRow
    {
        [JsonPropertyName("elements")]
        public List<DistanceMatrixResponceElement> Elements { get; set; }
    }

    public class DistanceMatrixResponceElement
    {
        [JsonPropertyName("distance")]
        public DistanceMatrixResponceElementValue Distance { get; set; }

        [JsonPropertyName("duration")]
        public DistanceMatrixResponceElementValue Duration { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class DistanceMatrixResponceElementValue
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("value")]
        public int Value { get; set; }
    }
}
