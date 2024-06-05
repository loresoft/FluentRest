namespace FluentRest;

/// <summary>
/// Provides a fluent interface for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.
/// </summary>
public interface IFluentClient : IDisposable
{
    /// <summary>
    /// Gets the <see cref="HttpClient"/> used to send request.
    /// </summary>
    /// <value>
    /// The <see cref="HttpClient"/> used to send request.
    /// </value>
    HttpClient HttpClient { get; }

    /// <summary>
    /// Gets or sets the serializer used to convert to and from <see cref="HttpContent"/>.
    /// </summary>
    /// <value>
    /// The serializer used to convert to and from <see cref="HttpContent"/>.
    /// </value>
    IContentSerializer ContentSerializer { get; set; }
}
