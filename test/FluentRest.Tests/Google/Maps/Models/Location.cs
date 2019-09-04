using System.Text.Json.Serialization;

namespace FluentRest.Tests.Google.Maps.Models
{

    public class Location
    {

        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }

}
