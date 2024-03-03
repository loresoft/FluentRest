using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentRest;

/// <summary>
/// JsonSerializerContext for <see cref="ProblemDetails"/>
/// </summary>
/// <seealso cref="System.Text.Json.Serialization.JsonSerializerContext" />
/// <seealso cref="System.Text.Json.Serialization.Metadata.IJsonTypeInfoResolver" />
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(JsonElement))]
public partial class ProblemDetailsJsonContext : JsonSerializerContext;
