using System.Text.Json.Serialization;

namespace FluentRest.Tests.Google.Maps.Models
{

    public class Viewport
    {

        [JsonPropertyName("northeast")]
        public Location Northeast { get; set; }

        [JsonPropertyName("southwest")]
        public Location Southwest { get; set; }
    }

}
