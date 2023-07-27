using System.Text.Json.Serialization;

namespace FluentRest.Tests.Google.Maps.Models;


public class GeocodeResult
{

    [JsonPropertyName("address_components")]
    public AddressComponent[] AddressComponents { get; set; }

    [JsonPropertyName("formatted_address")]
    public string FormattedAddress { get; set; }

    [JsonPropertyName("geometry")]
    public Geometry Geometry { get; set; }

    [JsonPropertyName("place_id")]
    public string PlaceId { get; set; }

    [JsonPropertyName("types")]
    public string[] Types { get; set; }
}
