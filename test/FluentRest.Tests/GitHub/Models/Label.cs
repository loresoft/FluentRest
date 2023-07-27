using System.Text.Json.Serialization;

namespace FluentRest.Tests.GitHub.Models;


public class Label
{

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }
}
