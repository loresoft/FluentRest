using System.Text.Json.Serialization;

namespace FluentRest.Tests.Google.Maps.Models;


public class Geometry
{

    [JsonPropertyName("location")]
    public Location Location { get; set; }

    [JsonPropertyName("location_type")]
    public string LocationType { get; set; }

    [JsonPropertyName("viewport")]
    public Viewport Viewport { get; set; }
}
