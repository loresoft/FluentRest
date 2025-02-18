namespace FluentRest;

/// <summary>
/// A fluent send request builder.
/// </summary>
public class SendBuilder : PostBuilder<SendBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SendBuilder"/> class.
    /// </summary>
    /// <param name="requestMessage">The fluent HTTP request being built.</param>
    public SendBuilder(HttpRequestMessage requestMessage) : base(requestMessage)
    {
    }

    /// <summary>
    /// Sets HTTP request method.
    /// </summary>
    /// <param name="method">The header request method.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="method" /> is <see langword="null" />.</exception>
    public SendBuilder Method(HttpMethod method)
    {
        if (method is null)
            throw new ArgumentNullException(nameof(method));

        RequestMessage.Method = method;

        return this;
    }

    /// <summary>
    /// Sets HTTP request method to POST.
    /// </summary>
    /// <returns>A fluent request builder.</returns>
    public SendBuilder Post()
    {
        return Method(HttpMethod.Post);
    }

    /// <summary>
    /// Sets HTTP request method to PUT.
    /// </summary>
    /// <returns>A fluent request builder.</returns>
    public SendBuilder Put()
    {
        return Method(HttpMethod.Put);
    }

    /// <summary>
    /// Sets HTTP request method to PATCH.
    /// </summary>
    /// <returns>A fluent request builder.</returns>
    public SendBuilder Patch()
    {
        return Method(HttpPatch);
    }

    /// <summary>
    /// Sets HTTP request method to DELETE.
    /// </summary>
    /// <returns>A fluent request builder.</returns>
    public SendBuilder Delete()
    {
        return Method(HttpMethod.Delete);
    }
}
