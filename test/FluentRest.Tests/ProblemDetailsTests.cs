using System.Text.Json;
using System.Text.Json.Serialization;

using FluentAssertions;

using Xunit;

namespace FluentRest.Tests;

public class ProblemDetailsTests
{
    [Fact]
    public void DeserializeBadRequestValidation()
    {
        var options = CreateOptions();

        var json = @"{
    ""type"": ""https://tools.ietf.org/html/rfc9110#section-15.5.1"",
    ""title"": ""One or more validation errors occurred."",
    ""status"": 400,
    ""errors"": {
        ""caseManagerIds"": [
            ""The value 'System.Collections.Generic.List`1[System.Int32]' is not valid.""
        ]
    },
    ""traceId"": ""00-894a84e8e9621fb8fcabcb2911e6d51c-5a4777d830f30cff-00""
}";
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(json, options);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Status.Should().Be(400);
    }



    [Fact]
    public void DeserializeServerError()
    {
        var options = CreateOptions();

        var json = @"{
    ""type"": ""https://tools.ietf.org/html/rfc9110#section-15.5.1"",
    ""title"": ""One or more errors occurred."",
    ""status"": 500,
    ""traceId"": ""00-894a84e8e9621fb8fcabcb2911e6d51c-5a4777d830f30cff-00""
}";
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(json, options);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more errors occurred.");
        problemDetails.Status.Should().Be(500);
    }

    [Fact]
    public void DeserializeServerErrorExtra()
    {
        var options = CreateOptions();

        var json = @"{
    ""type"": ""https://tools.ietf.org/html/rfc9110#section-15.5.1"",
    ""title"": ""One or more errors occurred."",
    ""status"": 500,
    ""exception"": ""this is an exception"",
    ""traceId"": ""00-894a84e8e9621fb8fcabcb2911e6d51c-5a4777d830f30cff-00""
}";
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(json, options);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Be("One or more errors occurred.");
        problemDetails.Status.Should().Be(500);
    }

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        options.TypeInfoResolverChain.Add(ProblemDetailsJsonContext.Default);
        return options;
    }
}
