using System.Text.Json.Serialization;

namespace FluentRest.Tests.Google.Maps.Models;


public class GeocodeResponse
{

    [JsonPropertyName("results")]
    public GeocodeResult[] Results { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }
}
