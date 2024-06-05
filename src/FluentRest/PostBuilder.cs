using System.Net.Http.Headers;
using System.Text;

namespace FluentRest;

/// <summary>
/// A fluent form post builder.
/// </summary>
/// <typeparam name="TBuilder">The type of the builder.</typeparam>
public abstract class PostBuilder<TBuilder> : QueryBuilder<TBuilder>
    where TBuilder : PostBuilder<TBuilder>
{
    internal static readonly HttpMethod HttpPatch = new HttpMethod("PATCH");

    /// <summary>
    /// Initializes a new instance of the <see cref="PostBuilder{TBuilder}"/> class.
    /// </summary>
    /// <param name="requestMessage">The fluent HTTP request being built.</param>
    protected PostBuilder(HttpRequestMessage requestMessage) : base(requestMessage)
    {
    }


    /// <summary>
    /// Appends the specified <paramref name="name"/> and <paramref name="value"/> to the form post body.
    /// </summary>
    /// <param name="name">The form parameter name.</param>
    /// <param name="value">The form parameter value.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder FormValue(string name, string value)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));

        var v = value ?? string.Empty;

        var formData = RequestMessage.GetFormData();
        var list = formData.GetOrAdd(name, n => new List<string>());
        list.Add(v);

        return this as TBuilder;
    }

    /// <summary>
    /// Appends the specified <paramref name="name" /> and <paramref name="value" /> to the form post body.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="name">The form parameter name.</param>
    /// <param name="value">The form parameter value.</param>
    /// <returns>
    /// A fluent request builder.
    /// </returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder FormValue<TValue>(string name, TValue value)
    {
        var v = value != null ? value.ToString() : string.Empty;
        return FormValue(name, v);
    }

    /// <summary>
    /// Appends the specified key value pairs to the form post body.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="data">The form key value parameters.</param>
    /// <returns>
    /// A fluent request builder.
    /// </returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="data" /> is <see langword="null" />.</exception>
    public TBuilder FormValue<TValue>(IEnumerable<KeyValuePair<string, TValue>> data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        foreach (var pair in data)
            FormValue(pair.Key, pair.Value);

        return this as TBuilder;
    }

    /// <summary>
    /// Appends the specified key value pairs to the form post body.
    /// </summary>
    /// <param name="data">The form key value parameters.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="data" /> is <see langword="null" />.</exception>
    public TBuilder FormValue(IEnumerable<KeyValuePair<string, string>> data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        foreach (var pair in data)
            FormValue(pair.Key, pair.Value);

        return this as TBuilder;
    }

    /// <summary>
    /// Appends the specified <paramref name="name"/> and <paramref name="value"/> to the form post body if the specified <paramref name="condition"/> is true.
    /// </summary>
    /// <param name="condition">If condition is true, form data will be added; otherwise ignore form data.</param>
    /// <param name="name">The form parameter name.</param>
    /// <param name="value">The form parameter value.</param>
    /// <returns>A fluent request builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder FormValueIf(Func<bool> condition, string name, string value)
    {
        if (condition == null || !condition())
            return this as TBuilder;

        return FormValue(name, value);
    }

    /// <summary>
    /// Appends the specified <paramref name="name" /> and <paramref name="value" /> to the form post body if the specified <paramref name="condition" /> is true.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="condition">If condition is true, form data will be added; otherwise ignore form data.</param>
    /// <param name="name">The form parameter name.</param>
    /// <param name="value">The form parameter value.</param>
    /// <returns>
    /// A fluent request builder.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public TBuilder FormValueIf<TValue>(Func<bool> condition, string name, TValue value)
    {
        if (condition == null || !condition())
            return this as TBuilder;

        return FormValue(name, value);
    }

    /// <summary>
    /// Sets the contents of the HTTP message with the <see cref="HttpContent"/> value.
    /// </summary>
    /// <param name="content">The <see cref="HttpContent"/> to be sent.</param>
    /// <returns>A fluent request builder.</returns>
    /// <remarks>Setting the content of the request overrides any calls to FormValue.</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="content"/> is <see langword="null" />.</exception>
    public TBuilder Content(HttpContent content)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));

        RequestMessage.Content = content;
        if (RequestMessage.Method == HttpMethod.Get)
            RequestMessage.Method = HttpMethod.Post;

        return this as TBuilder;
    }

    /// <summary>
    /// Sets the raw post body to the serialized content of the specified <paramref name="data"/> object.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <param name="data">The data to be serialized.</param>
    /// <returns>A fluent request builder.</returns>
    /// <remarks>Setting the content of the request overrides any calls to FormValue.</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null" />.</exception>
    public TBuilder Content<TData>(TData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        // handle special case where string gets sent here
        if (data is string stringContent)
            return Content(stringContent, DetectContentType(stringContent), Encoding.UTF8);

        // handle case where HttpContent get here
        if (data is HttpContent httpContent)
            return Content(httpContent);

        // save for later serialization
        RequestMessage.SetOption(FluentProperties.RequestContentData, data);

        if (RequestMessage.Method == HttpMethod.Get)
            RequestMessage.Method = HttpMethod.Post;

        return this as TBuilder;
    }

    private string DetectContentType(string content)
    {
        for (int i = 0; i < content.Length; i++)
        {
            if (Char.IsWhiteSpace(content[i]))
                continue;

            switch (content[i])
            {
                case '{':
                case '[':
                    return "application/json";
                case '<':
                    return "text/xml";
            }
        }

        // default to JSON since this is a REST library
        return "application/json";
    }

    /// <summary>
    /// Sets the raw post body to the contents of the stream.
    /// </summary>
    /// <param name="data">The data to be used for the post body.</param>
    /// <param name="contentType">The content media type.</param>
    /// <returns>A fluent request builder.</returns>
    /// <remarks>Setting the content of the request overrides any calls to FormValue.</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="contentType"/> is <see langword="null" />.</exception>
    public TBuilder Content(Stream data, string contentType)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (contentType == null)
            throw new ArgumentNullException(nameof(contentType));

        var content = new StreamContent(data);
        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        RequestMessage.Content = content;
        if (RequestMessage.Method == HttpMethod.Get)
            RequestMessage.Method = HttpMethod.Post;

        return this as TBuilder;
    }

    /// <summary>
    /// Sets the raw post body to the contents of the byte array.
    /// </summary>
    /// <param name="data">The data to be used for the post body.</param>
    /// <param name="contentType">The content media type.</param>
    /// <returns>A fluent request builder.</returns>
    /// <remarks>Setting the content of the request overrides any calls to FormValue.</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="contentType"/> is <see langword="null" />.</exception>
    public TBuilder Content(byte[] data, string contentType)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (contentType == null)
            throw new ArgumentNullException(nameof(contentType));

        var content = new ByteArrayContent(data);
        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        RequestMessage.Content = content;
        if (RequestMessage.Method == HttpMethod.Get)
            RequestMessage.Method = HttpMethod.Post;

        return this as TBuilder;
    }

    /// <summary>
    /// Sets the raw post body to the contents of the string value.
    /// </summary>
    /// <param name="data">The string value to be used for the post body.</param>
    /// <param name="contentType">The content media type.</param>
    /// <param name="encoding">The encoding to use for the content.</param>
    /// <returns>A fluent request builder.</returns>
    /// <remarks>Setting the content of the request overrides any calls to FormValue.</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="contentType"/> is <see langword="null" />.</exception>
    public TBuilder Content(string data, string contentType, Encoding encoding = null)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (contentType == null)
            throw new ArgumentNullException(nameof(contentType));

        var stringContent = new StringContent(data, encoding ?? Encoding.UTF8, contentType);

        RequestMessage.Content = stringContent;
        if (RequestMessage.Method == HttpMethod.Get)
            RequestMessage.Method = HttpMethod.Post;

        return this as TBuilder;
    }
}
