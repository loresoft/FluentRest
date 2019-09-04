using System.Text.Json.Serialization;

namespace FluentRest.Tests.Google.Maps.Models
{

    public class AddressComponent
    {

        [JsonPropertyName("long_name")]
        public string LongName { get; set; }

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonPropertyName("types")]
        public string[] Types { get; set; }
    }

}
