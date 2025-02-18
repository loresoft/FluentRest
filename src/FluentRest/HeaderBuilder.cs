using System.Net.Http.Headers;

namespace FluentRest;

/// <summary>
/// Fluent header builder
/// </summary>
public class HeaderBuilder : HeaderBuilder<HeaderBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderBuilder"/> class.
    /// </summary>
    /// <param name="requestMessage">The fluent HTTP request being built.</param>
    public HeaderBuilder(HttpRequestMessage requestMessage) : base(requestMessage)
    {
    }
}

/// <summary>
/// Fluent header builder
/// </summary>
/// <typeparam name="TBuilder">The type of the builder.</typeparam>
public abstract class HeaderBuilder<TBuilder> : RequestBuilder<TBuilder>
        where TBuilder : HeaderBuilder<TBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderBuilder{TBuilder}"/> class.
    /// </summary>
    /// <param name="requestMessage">The fluent HTTP request being built.</param>
    protected HeaderBuilder(HttpRequestMessage requestMessage) : base(requestMessage)
    {
    }

    /// <summary>
    /// Append the media-type to the Accept header for an HTTP request.
    /// </summary>
    /// <param name="mediaType">The media-type header value.</param>
    /// <param name="quality">The quality associated with the header value.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder Accept(string mediaType, double? quality = null)
    {

        var header = quality.HasValue
            ? new MediaTypeWithQualityHeaderValue(mediaType, quality.Value)
            : new MediaTypeWithQualityHeaderValue(mediaType);

        RequestMessage.Headers.Accept.Add(header);

        return (TBuilder)this;
    }

    /// <summary>
    /// Append the value to the Accept-Charset header for an HTTP request.
    /// </summary>
    /// <param name="value">The header value.</param>
    /// <param name="quality">The quality associated with the header value.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder AcceptCharset(string value, double? quality = null)
    {
        var header = quality.HasValue
            ? new StringWithQualityHeaderValue(value, quality.Value)
            : new StringWithQualityHeaderValue(value);

        RequestMessage.Headers.AcceptCharset.Add(header);

        return (TBuilder)this;
    }

    /// <summary>
    /// Append the value to the Accept-Encoding header for an HTTP request.
    /// </summary>
    /// <param name="value">The header value.</param>
    /// <param name="quality">The quality associated with the header value.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder AcceptEncoding(string value, double? quality = null)
    {
        var header = quality.HasValue
            ? new StringWithQualityHeaderValue(value, quality.Value)
            : new StringWithQualityHeaderValue(value);

        RequestMessage.Headers.AcceptEncoding.Add(header);

        return (TBuilder)this;
    }

    /// <summary>
    /// Append the value to the Accept-Language header for an HTTP request.
    /// </summary>
    /// <param name="value">The header value.</param>
    /// <param name="quality">The quality associated with the header value.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder AcceptLanguage(string value, double? quality = null)
    {
        var header = quality.HasValue
            ? new StringWithQualityHeaderValue(value, quality.Value)
            : new StringWithQualityHeaderValue(value);

        RequestMessage.Headers.AcceptLanguage.Add(header);

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the value of the Authorization header for an HTTP request.
    /// </summary>
    /// <param name="scheme">The scheme to use for authorization.</param>
    /// <param name="parameter">The credentials containing the authentication information.</param>
    /// <returns>A fluent header builder.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public TBuilder Authorization(string scheme, string? parameter = null)
    {
        if (scheme is null)
            throw new ArgumentNullException(nameof(scheme));

        var header = new AuthenticationHeaderValue(scheme, parameter);

        RequestMessage.Headers.Authorization = header;

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the value of the Cache-Control header for an HTTP request.
    /// </summary>
    /// <param name="value">The header value.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder CacheControl(string? value)
    {
        var header = CacheControlHeaderValue.Parse(value);

        RequestMessage.Headers.CacheControl = header;

        return (TBuilder)this;
    }

    /// <summary>
    /// Append the value of the Expect header for an HTTP request.
    /// </summary>
    /// <param name="name">The header name.</param>
    /// <param name="value">The header value.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder Expect(string name, string? value = null)
    {
        var header = new NameValueWithParametersHeaderValue(name, value);

        RequestMessage.Headers.Expect.Add(header);

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the value of the From header for an HTTP request.
    /// </summary>
    /// <param name="value">The header value.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder From(string? value)
    {
        RequestMessage.Headers.From = value;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the value of the Host header for an HTTP request.
    /// </summary>
    /// <param name="value">The header value.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder Host(string? value)
    {
        RequestMessage.Headers.Host = value;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the value of the If-Modified-Since header for an HTTP request.
    /// </summary>
    /// <param name="modifiedDate">The modified date.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder IfModifiedSince(DateTimeOffset? modifiedDate)
    {
        RequestMessage.Headers.IfModifiedSince = modifiedDate;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the value of the If-Unmodified-Since header for an HTTP request.
    /// </summary>
    /// <param name="modifiedDate">The modified date.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder IfUnmodifiedSince(DateTimeOffset? modifiedDate)
    {
        RequestMessage.Headers.IfUnmodifiedSince = modifiedDate;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the value of the Proxy-Authorization header for an HTTP request.
    /// </summary>
    /// <param name="scheme">The scheme to use for authorization.</param>
    /// <param name="parameter">The credentials containing the authentication information.</param>
    /// <returns>A fluent header builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="scheme"/> is <see langword="null"/></exception>
    public TBuilder ProxyAuthorization(string scheme, string? parameter = null)
    {
        if (scheme is null)
            throw new ArgumentNullException(nameof(scheme));

        var header = new AuthenticationHeaderValue(scheme, parameter);
        RequestMessage.Headers.ProxyAuthorization = header;

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the value of the Range header for an HTTP request.
    /// </summary>
    /// <param name="from">The position at which to start sending data.</param>
    /// <param name="to">The position at which to stop sending data.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder Range(long? from, long? to)
    {
        var header = new RangeHeaderValue(from, to);
        RequestMessage.Headers.Range = header;

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the value of the Referrer header for an HTTP request.
    /// </summary>
    /// <param name="uri">The header URI.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder Referrer(Uri? uri)
    {
        RequestMessage.Headers.Referrer = uri;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the value of the Referrer header for an HTTP request.
    /// </summary>
    /// <param name="value">The header value.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder Referrer(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return (TBuilder)this;

        var uri = new Uri(value);
        RequestMessage.Headers.Referrer = uri;

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the value of the User-Agent header for an HTTP request.
    /// </summary>
    /// <param name="value">The header value.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder UserAgent(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return (TBuilder)this;

        var header = ProductInfoHeaderValue.Parse(value);
        RequestMessage.Headers.UserAgent.Add(header);

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the value of the X-HTTP-Method-Override header for an HTTP request.
    /// </summary>
    /// <param name="method">The HTTP method.</param>
    /// <returns>A fluent header builder.</returns>
    public TBuilder MethodOverride(HttpMethod? method)
    {
        RequestMessage.Headers.Add(HttpRequestHeaders.MethodOverride, method?.ToString());
        return (TBuilder)this;
    }
}
