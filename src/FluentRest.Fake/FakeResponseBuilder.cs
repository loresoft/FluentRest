using System;
using System.Collections.Generic;
using System.Net;

namespace FluentRest.Fake;

/// <summary>
/// A fluent fake response builder for a <see cref="FakeResponseContainer"/>.
/// </summary>
public class FakeResponseBuilder : FakeContainerBuilder<FakeResponseBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FakeResponseBuilder"/> class.
    /// </summary>
    /// <param name="container">The container to build.</param>
    public FakeResponseBuilder(FakeResponseContainer container) : base(container)
    {
    }

    /// <summary>
    /// Sets the response HTTP status code
    /// </summary>
    /// <param name="value">The response HTTP status code.</param>
    /// <returns>
    /// A fluent fake response builder.
    /// </returns>
    public FakeResponseBuilder StatusCode(HttpStatusCode value)
    {
        Container.ResponseMessage.StatusCode = value;
        return this;
    }

    /// <summary>
    /// Sets the response HTTP reason phrase.
    /// </summary>
    /// <param name="value">The response HTTP reason phrase..</param>
    /// <returns>
    /// A fluent fake response builder.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public FakeResponseBuilder ReasonPhrase(string value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        Container.ResponseMessage.ReasonPhrase = value;
        return this;
    }

    /// <summary>
    /// Sets HTTP response header with the specified <paramref name="name"/> and <paramref name="value"/>.
    /// </summary>
    /// <param name="name">The response header name.</param>
    /// <param name="value">The response header value.</param>
    /// <returns>A fluent fake response builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public FakeResponseBuilder Header(string name, string value)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));

        if (value == null)
            Container.ResponseMessage.ResponseHeaders.Remove(name);
        else
            Container.ResponseMessage.ResponseHeaders[name] = new List<string>(new[] { value });

        return this;
    }

    /// <summary>
    /// Start a fluent fake content builder.
    /// </summary>
    /// <param name="builder">The fluent fake content builder.</param>
    /// <returns>
    /// A fluent fake response builder.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public FakeResponseBuilder Content(Action<FakeContentBuilder> builder)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        var contentBuilder = new FakeContentBuilder(Container);
        builder(contentBuilder);

        return this;
    }
}
