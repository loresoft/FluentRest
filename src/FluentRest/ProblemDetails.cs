using System.Text.Json.Serialization;

namespace FluentRest;

/// <summary>
/// A machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807.
/// </summary>
[JsonConverter(typeof(ProblemDetailsConverter))]
public class ProblemDetails
{
    /// <summary>
    /// The content-type for a problem json response
    /// </summary>
    public const string ContentType = "application/problem+json";

    /// <summary>
    /// A URI reference that identifies the problem type.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// A short, human-readable summary of the problem type.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// The HTTP status code generated by the origin server for this occurrence of the problem.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("status")]
    public int? Status { get; set; }

    /// <summary>
    /// A human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("detail")]
    public string Detail { get; set; }

    /// <summary>
    /// A URI reference that identifies the specific occurrence of the problem.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("instance")]
    public string Instance { get; set; }

    /// <summary>
    /// Problem type definitions MAY extend the problem details object with additional members.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object> Extensions { get; } = new Dictionary<string, object>(StringComparer.Ordinal);
}
