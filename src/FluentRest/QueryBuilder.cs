namespace FluentRest;

/// <summary>
/// A fluent query builder.
/// </summary>
public class QueryBuilder : QueryBuilder<QueryBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryBuilder" /> class.
    /// </summary>
    /// <param name="requestMessage">The fluent HTTP request being built.</param>
    public QueryBuilder(HttpRequestMessage requestMessage) : base(requestMessage)
    {
    }
}

/// <summary>
/// A fluent query builder.
/// </summary>
/// <typeparam name="TBuilder">The type of the builder.</typeparam>
public abstract class QueryBuilder<TBuilder> : RequestBuilder<TBuilder>
    where TBuilder : QueryBuilder<TBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryBuilder{TBuilder}"/> class.
    /// </summary>
    /// <param name="requestMessage">The fluent HTTP request being built.</param>
    protected QueryBuilder(HttpRequestMessage requestMessage) : base(requestMessage)
    {
    }

    /// <summary>
    /// Start a fluent header builder.
    /// </summary>
    /// <param name="builder">The builder factory.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public TBuilder Header(Action<HeaderBuilder> builder)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        var headerBuilder = new HeaderBuilder(RequestMessage);
        builder(headerBuilder);

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets HTTP header with the specified <paramref name="name"/> and <paramref name="value"/>.
    /// </summary>
    /// <param name="name">The header name.</param>
    /// <param name="value">The header value.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder Header(string name, string? value)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        if (value is null)
            RequestMessage.Headers.Remove(name);
        else
            RequestMessage.Headers.Add(name, value);

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets HTTP header with the specified <paramref name="name"/> and <paramref name="value"/> if the specified <paramref name="condition"/> is true.
    /// </summary>
    /// <param name="condition">If condition is true, header will be added; otherwise ignore header.</param>
    /// <param name="name">The header name.</param>
    /// <param name="value">The header value.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder HeaderIf(Func<bool> condition, string name, string? value)
    {
        if (condition is null || !condition())
            return (TBuilder)this;

        return Header(name, value);
    }

    /// <summary>
    /// Sets HTTP header with the specified <paramref name="name"/> and <paramref name="value"/> if the specified <paramref name="condition"/> is true.
    /// </summary>
    /// <param name="condition">If condition is true, header will be added; otherwise ignore header.</param>
    /// <param name="name">The header name.</param>
    /// <param name="value">The header value.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder HeaderIf(bool condition, string name, string? value)
    {
        if (!condition)
            return (TBuilder)this;

        return Header(name, value);
    }


    /// <summary>
    /// Sets the base URI address used when sending requests.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="path" /> is <see langword="null" />.</exception>
    public TBuilder BaseUri(Uri path)
    {
        if (path is null)
            throw new ArgumentNullException(nameof(path));

        var urlBuilder = new UrlBuilder(path);
        RequestMessage.SetUrlBuilder(urlBuilder);

        RequestMessage.Synchronize();


        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the base URI address used when sending requests.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="path" /> is <see langword="null" />.</exception>
    public TBuilder BaseUri(string path)
    {
        if (path is null)
            throw new ArgumentNullException(nameof(path));

        var uri = new Uri(path, UriKind.Absolute);
        return BaseUri(uri);
    }


    /// <summary>
    /// Sets the base URI from the specified <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The full Uri path.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="path" /> is <see langword="null" />.</exception>
    public TBuilder FullUri(Uri path)
    {
        if (path is null)
            throw new ArgumentNullException(nameof(path));

        return BaseUri(path);
    }

    /// <summary>
    /// Sets the base URI from the specified <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The full Uri path.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="path" /> is <see langword="null" />.</exception>
    public TBuilder FullUri(string path)
    {
        if (path is null)
            throw new ArgumentNullException(nameof(path));

        var u = new Uri(path, UriKind.Absolute);
        return BaseUri(u);
    }


    /// <summary>
    /// Appends the specified <paramref name="path"/> to the BaseUri of the request.
    /// </summary>
    /// <param name="path">The path to append.</param>
    /// <returns>A fluent request builder.</returns>
    public TBuilder AppendPath(Uri? path)
    {
        if (path is null)
            return (TBuilder)this;

        var urlBuilder = RequestMessage.GetUrlBuilder();
        urlBuilder.AppendPath(path);

        RequestMessage.Synchronize();

        return (TBuilder)this;
    }

    /// <summary>
    /// Appends the specified <paramref name="path" /> to the BaseUri of the request.
    /// </summary>
    /// <param name="path">The path to append.</param>
    /// <returns>
    /// A fluent request builder.
    /// </returns>
    public TBuilder AppendPath(string? path)
    {
        if (path is null)
            return (TBuilder)this;

        var urlBuilder = RequestMessage.GetUrlBuilder();
        urlBuilder.AppendPath(path);

        RequestMessage.Synchronize();

        return (TBuilder)this;
    }

    /// <summary>
    /// Appends the specified <paramref name="path" /> to the BaseUri of the request.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="path">The path to append.</param>
    /// <returns>A fluent request builder.</returns>
    public TBuilder AppendPath<TValue>(TValue? path)
    {
        if (path is null)
            return (TBuilder)this;

        var urlBuilder = RequestMessage.GetUrlBuilder();
        urlBuilder.AppendPath(path);

        RequestMessage.Synchronize();

        return (TBuilder)this;
    }

    /// <summary>
    /// Appends the specified <paramref name="paths"/> to the BaseUri of the request.
    /// </summary>
    /// <param name="paths">The paths to append.</param>
    /// <returns>A fluent request builder.</returns>
    public TBuilder AppendPaths(IEnumerable<string>? paths)
    {
        if (paths is null)
            return (TBuilder)this;

        var urlBuilder = RequestMessage.GetUrlBuilder();
        urlBuilder.AppendPaths(paths);

        RequestMessage.Synchronize();

        return (TBuilder)this;
    }

    /// <summary>
    /// Appends the specified <paramref name="paths"/> to the BaseUri of the request.
    /// </summary>
    /// <param name="paths">The paths to append.</param>
    /// <returns>A fluent request builder.</returns>
    public TBuilder AppendPaths(params string[]? paths)
    {
        if (paths is null)
            return (TBuilder)this;

        var urlBuilder = RequestMessage.GetUrlBuilder();
        urlBuilder.AppendPaths(paths);

        RequestMessage.Synchronize();

        return (TBuilder)this;
    }

    /// <summary>
    /// Appends the specified <paramref name="path" /> to the BaseUri of the request.
    /// </summary>
    /// <param name="condition">If condition is true, append path will be added; otherwise ignore path.</param>
    /// <param name="path">The path to append.</param>
    /// <returns>
    /// A fluent request builder.
    /// </returns>
    public TBuilder AppendPathIf(Func<bool> condition, string? path)
    {
        if (path is null)
            return (TBuilder)this;

        if (condition is null || !condition())
            return (TBuilder)this;

        return AppendPath(path);
    }

    /// <summary>
    /// Appends the specified <paramref name="path" /> to the BaseUri of the request.
    /// </summary>
    /// <param name="condition">If condition is true, append path will be added; otherwise ignore path.</param>
    /// <param name="path">The path to append.</param>
    /// <returns>
    /// A fluent request builder.
    /// </returns>
    public TBuilder AppendPathIf(bool condition, string? path)
    {
        if (path is null)
            return (TBuilder)this;

        if (!condition)
            return (TBuilder)this;

        return AppendPath(path);
    }

    /// <summary>
    /// Appends the specified <paramref name="path" /> to the BaseUri of the request.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="condition">If condition is true, append path will be added; otherwise ignore path.</param>
    /// <param name="path">The path to append.</param>
    /// <returns>A fluent request builder.</returns>
    public TBuilder AppendPathIf<TValue>(Func<bool> condition, TValue? path)
    {
        if (path is null)
            return (TBuilder)this;

        if (condition is null || !condition())
            return (TBuilder)this;

        return AppendPath(path);
    }

    /// <summary>
    /// Appends the specified <paramref name="path" /> to the BaseUri of the request.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="condition">If condition is true, append path will be added; otherwise ignore path.</param>
    /// <param name="path">The path to append.</param>
    /// <returns>A fluent request builder.</returns>
    public TBuilder AppendPathIf<TValue>(bool condition, TValue? path)
    {
        if (path is null)
            return (TBuilder)this;

        if (!condition)
            return (TBuilder)this;

        return AppendPath(path);
    }


    /// <summary>
    /// Appends the specified <paramref name="name"/> and <paramref name="value"/> to the request Uri.
    /// </summary>
    /// <param name="name">The query parameter name.</param>
    /// <param name="value">The query parameter value.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder QueryString(string name, string? value)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));


        var urlBuilder = RequestMessage.GetUrlBuilder();
        urlBuilder.AppendQuery(name, value);

        RequestMessage.Synchronize();

        return (TBuilder)this;

    }

    /// <summary>
    /// Appends the specified <paramref name="name" /> and <paramref name="value" /> to the request Uri.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="name">The query parameter name.</param>
    /// <param name="value">The query parameter value.</param>
    /// <returns>
    /// A fluent request builder.
    /// </returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder QueryString<TValue>(string name, TValue? value)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        var v = value != null ? value.ToString() : string.Empty;
        return QueryString(name, v);
    }

    /// <summary>
    /// Appends the specified <paramref name="name" /> and <paramref name="values" /> to the request Uri.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="name">The query parameter name.</param>
    /// <param name="values">The query parameter values.</param>
    /// <returns>
    /// A fluent request builder.
    /// </returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder QueryStrings<TValue>(string name, IEnumerable<TValue>? values)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        if (values is null)
            return (TBuilder)this;

        foreach (var value in values)
        {
            var v = value != null ? value.ToString() : string.Empty;
            QueryString(name, v);
        }

        return (TBuilder)this;
    }


    /// <summary>
    /// Appends the specified <paramref name="name"/> and <paramref name="value"/> to the request Uri if the specified <paramref name="condition"/> is true.
    /// </summary>
    /// <param name="condition">If condition is true, query string will be added; otherwise ignore query string.</param>
    /// <param name="name">The query parameter name.</param>
    /// <param name="value">The query parameter value.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder QueryStringIf(Func<bool> condition, string name, string? value)
    {
        if (condition is null || !condition())
            return (TBuilder)this;

        return QueryString(name, value);
    }

    /// <summary>
    /// Appends the specified <paramref name="name"/> and <paramref name="value"/> to the request Uri if the specified <paramref name="condition"/> is true.
    /// </summary>
    /// <param name="condition">If condition is true, query string will be added; otherwise ignore query string.</param>
    /// <param name="name">The query parameter name.</param>
    /// <param name="value">The query parameter value.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder QueryStringIf(bool condition, string name, string? value)
    {
        if (!condition)
            return (TBuilder)this;

        return QueryString(name, value);
    }

    /// <summary>
    /// Appends the specified <paramref name="name" /> and <paramref name="value" /> to the request Uri if the specified <paramref name="condition" /> is true.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="condition">If condition is true, query string will be added; otherwise ignore query string.</param>
    /// <param name="name">The query parameter name.</param>
    /// <param name="value">The query parameter value.</param>
    /// <returns>
    /// A fluent request builder.
    /// </returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder QueryStringIf<TValue>(Func<bool> condition, string name, TValue? value)
    {
        if (condition is null || !condition())
            return (TBuilder)this;

        return QueryString(name, value);
    }

    /// <summary>
    /// Appends the specified <paramref name="name" /> and <paramref name="value" /> to the request Uri if the specified <paramref name="condition" /> is true.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="condition">If condition is true, query string will be added; otherwise ignore query string.</param>
    /// <param name="name">The query parameter name.</param>
    /// <param name="value">The query parameter value.</param>
    /// <returns>
    /// A fluent request builder.
    /// </returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder QueryStringIf<TValue>(bool condition, string name, TValue? value)
    {
        if (!condition)
            return (TBuilder)this;

        return QueryString(name, value);
    }
}
