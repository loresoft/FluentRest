using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentRest.Fake.Tests;

public class EchoResult
{
    public EchoResult()
    {
        Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        QueryString = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        Form = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    [JsonPropertyName("headers")]
    public Dictionary<string, string> Headers { get; set; }

    [JsonPropertyName("args")]
    public Dictionary<string, string> QueryString { get; set; }

    [JsonPropertyName("form")]
    public Dictionary<string, string> Form { get; set; }

    [JsonPropertyName("data")]
    public string Data { get; set; }

    [JsonPropertyName("json")]
    public JsonElement? Json { get; set; }

    [JsonPropertyName("origin")]
    public string Origin { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("authenticated")]
    public bool? Authenticated { get; set; }

    [JsonPropertyName("user")]
    public string User { get; set; }
}
