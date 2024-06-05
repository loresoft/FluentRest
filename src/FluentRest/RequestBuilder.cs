namespace FluentRest;

/// <summary>
/// A fluent request builder.
/// </summary>
/// <typeparam name="TBuilder">The type of the builder.</typeparam>
public abstract class RequestBuilder<TBuilder>
    where TBuilder : RequestBuilder<TBuilder>
{
    /// <summary>
    /// Gets the fluent HTTP request being built.
    /// </summary>
    /// <value>
    /// The fluent HTTP request being built.
    /// </value>
    public HttpRequestMessage RequestMessage { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestBuilder{TBuilder}"/> class.
    /// </summary>
    /// <param name="requestMessage">The fluent HTTP request being built.</param>
    protected RequestBuilder(HttpRequestMessage requestMessage)
    {
        RequestMessage = requestMessage;
    }

    /// <summary>
    /// Sets the cancellation token to cancel the request operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A fluent request builder.</returns>
    public TBuilder CancellationToken(CancellationToken cancellationToken)
    {
        RequestMessage.SetCancellationToken(cancellationToken);
        return this as TBuilder;
    }

    /// <summary>
    /// Sets when the operation should complete (as soon as a response is available or after reading the whole response content).
    /// </summary>
    /// <param name="completionOption">When the operation should complete.</param>
    /// <returns>A fluent request builder.</returns>
    public TBuilder CompletionOption(HttpCompletionOption completionOption)
    {
        RequestMessage.SetCompletionOption(completionOption);
        return this as TBuilder;
    }


    /// <summary>
    /// Sets a state value on the request.
    /// </summary>
    /// <param name="key">The state key.</param>
    /// <param name="value">The state value.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentException">Argument is null or empty</exception>
    public TBuilder State(string key, object value)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Argument is null or empty", nameof(key));

        RequestMessage.SetOption(key, value);
        return this as TBuilder;
    }
}
