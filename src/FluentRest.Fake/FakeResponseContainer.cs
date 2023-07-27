using System;
using System.Collections.Generic;

namespace FluentRest.Fake;

/// <summary>
/// A fake response container used to store fake responses.
/// </summary>
public class FakeResponseContainer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FakeResponseContainer"/> class.
    /// </summary>
    public FakeResponseContainer()
    {
        ResponseMessage = new FakeResponseMessage();
        ResponseMessage.ReasonPhrase = "OK";
        ResponseMessage.StatusCode = System.Net.HttpStatusCode.OK;
        ResponseMessage.ResponseHeaders["Status"] = new List<string> { "200 OK" };
    }

    /// <summary>
    /// The callback used to serialize the fake response data objects into a byte array.
    /// If not specified, the default settings for the System.Text.Json.JsonSerializer will be used.
    ///</summary>
    public SerializeResponseContentCallback SerializeResponseContentCallback { get; set; }

    /// <summary>
    /// Gets or sets the request URI.
    /// </summary>
    /// <value>
    /// The request URI.
    /// </value>
    public Uri RequestUri { get; set; }

    /// <summary>
    /// Gets or sets the response message.
    /// </summary>
    /// <value>
    /// The response message.
    /// </value>
    public FakeResponseMessage ResponseMessage { get; set; }

    /// <summary>
    /// Gets or sets the content of the HTTP response.
    /// </summary>
    /// <value>
    /// The content of the HTTP response.
    /// </value>
    public byte[] HttpContent { get; set; }
}
