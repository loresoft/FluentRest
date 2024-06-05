// Ignore Spelling: Serializer Deserialize

namespace FluentRest;

/// <summary>
/// Extension method for <see cref="HttpRequestMessage"/>
/// </summary>
public static class HttpMessageExtensions
{
    public static TValue GetOrAddOption<TValue>(this HttpRequestMessage requestMessage, string key, Func<string, TValue> valueFactory)
    {
#if NET5_0_OR_GREATER
        var optionKey = new HttpRequestOptionsKey<TValue>(key);
        if (requestMessage.Options.TryGetValue(optionKey, out var value))
            return value;

        value = valueFactory(key);
        requestMessage.Options.Set(optionKey, value);
        return value;
#else
        if (requestMessage.Properties.TryGetValue(key, out var propertyValue))
            return (TValue)propertyValue;

        propertyValue = valueFactory(key);
        requestMessage.Properties.Add(key, propertyValue);

        return (TValue)propertyValue;
#endif
    }

    public static bool TryGetOption<TValue>(this HttpRequestMessage requestMessage, string key, out TValue value)
    {
#if NET5_0_OR_GREATER
        var optionKey = new HttpRequestOptionsKey<TValue>(key);
        return requestMessage.Options.TryGetValue(optionKey, out value);
#else
        var found = requestMessage.Properties.TryGetValue(key, out var propertyValue);
        value  = found ? (TValue)propertyValue : default;
        return found;
#endif
    }

    public static void SetOption<TValue>(this HttpRequestMessage requestMessage, string key, TValue value)
    {
#if NET5_0_OR_GREATER
        var optionKey = new HttpRequestOptionsKey<TValue>(key);
        requestMessage.Options.Set(optionKey, value);
#else
        requestMessage.Properties[key] = value;
#endif
    }

    /// <summary>
    /// Gets the <see cref="UrlBuilder"/> from the specified <paramref name="requestMessage" /> properties dictionary.
    /// </summary>
    /// <param name="requestMessage">The request message containing the property.</param>
    /// <returns>
    /// The <see cref="UrlBuilder"/> to modify the request message URI.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="requestMessage"/> is <see langword="null"/></exception>
    public static UrlBuilder GetUrlBuilder(this HttpRequestMessage requestMessage)
    {
        if (requestMessage == null)
            throw new ArgumentNullException(nameof(requestMessage));

        var propertyValue = requestMessage.GetOrAddOption(FluentProperties.RequestUrlBuilder, k =>
            requestMessage.RequestUri == null
                ? new UrlBuilder()
                : new UrlBuilder(requestMessage.RequestUri)
        );

        return propertyValue;
    }

    /// <summary>
    /// Sets the <see cref="UrlBuilder" /> on the specified <paramref name="requestMessage" /> properties dictionary.
    /// </summary>
    /// <param name="requestMessage">The request message containing the property.</param>
    /// <param name="urlBuilder">The URL bulder to set on the properties dictionary.</param>
    /// <exception cref="ArgumentNullException"><paramref name="requestMessage" /> is <see langword="null" /></exception>
    public static void SetUrlBuilder(this HttpRequestMessage requestMessage, UrlBuilder urlBuilder)
    {
        if (requestMessage == null)
            throw new ArgumentNullException(nameof(requestMessage));

        requestMessage.SetOption(FluentProperties.RequestUrlBuilder, urlBuilder);
    }


    /// <summary>
    /// Gets the content data from the specified <paramref name="requestMessage" /> properties dictionary.
    /// </summary>
    /// <param name="requestMessage">The request message containing the property.</param>
    /// <returns>
    /// The content data to send for the request message.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="requestMessage"/> is <see langword="null"/></exception>
    public static object GetContentData(this HttpRequestMessage requestMessage)
    {
        if (requestMessage == null)
            throw new ArgumentNullException(nameof(requestMessage));

        requestMessage.TryGetOption<object>(FluentProperties.RequestContentData, out var propertyValue);
        return propertyValue;
    }

    /// <summary>
    /// Sets the content data on the specified <paramref name="requestMessage" /> properties dictionary.
    /// </summary>
    /// <param name="requestMessage">The request message containing the property.</param>
    /// <param name="contentData">The content data to send for the request message..</param>
    /// <exception cref="ArgumentNullException"><paramref name="requestMessage" /> is <see langword="null" /></exception>
    public static void SetContentData(this HttpRequestMessage requestMessage, object contentData)
    {
        if (requestMessage == null)
            throw new ArgumentNullException(nameof(requestMessage));

        requestMessage.SetOption(FluentProperties.RequestContentData, contentData);
    }


    /// <summary>
    /// Gets the form data property from the specified <paramref name="requestMessage" /> properties dictionary.
    /// </summary>
    /// <param name="requestMessage">The request message containing the property.</param>
    /// <returns>
    /// The dictionary of for data to send in the request message.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="requestMessage"/> is <see langword="null"/></exception>
    public static Dictionary<string, ICollection<string>> GetFormData(this HttpRequestMessage requestMessage)
    {
        if (requestMessage == null)
            throw new ArgumentNullException(nameof(requestMessage));

        var propertyValue = requestMessage.GetOrAddOption(FluentProperties.RequestFormData, k => new Dictionary<string, ICollection<string>>());
        return propertyValue;
    }


    /// <summary>
    /// Gets the completion option property from the specified <paramref name="requestMessage" /> properties dictionary.
    /// </summary>
    /// <param name="requestMessage">The request message containing the property.</param>
    /// <returns>
    /// The <see cref="HttpCompletionOption"/> to use when sending the request message.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="requestMessage"/> is <see langword="null"/></exception>
    public static HttpCompletionOption GetCompletionOption(this HttpRequestMessage requestMessage)
    {
        if (requestMessage == null)
            throw new ArgumentNullException(nameof(requestMessage));

        if (requestMessage.TryGetOption<HttpCompletionOption>(FluentProperties.HttpCompletionOption, out var value))
            return value;

        return HttpCompletionOption.ResponseContentRead;
    }

    /// <summary>
    /// Sets the completion option property on the specified <paramref name="requestMessage" /> properties dictionary.
    /// </summary>
    /// <param name="requestMessage">The request message containing the property.</param>
    /// <param name="completionOption">The <see cref="HttpCompletionOption"/> to use when sending the request message.</param>
    /// <exception cref="ArgumentNullException"><paramref name="requestMessage"/> is <see langword="null"/></exception>
    public static void SetCompletionOption(this HttpRequestMessage requestMessage, HttpCompletionOption completionOption)
    {
        if (requestMessage == null)
            throw new ArgumentNullException(nameof(requestMessage));

        requestMessage.SetOption(FluentProperties.HttpCompletionOption, completionOption);
    }


    /// <summary>
    /// Gets the cancellation token property from the specified <paramref name="requestMessage" /> properties dictionary.
    /// </summary>
    /// <param name="requestMessage">The request message containing the property.</param>
    /// <returns>
    /// The <see cref="CancellationToken"/> to use when sending the request message.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="requestMessage"/> is <see langword="null"/></exception>
    public static CancellationToken GetCancellationToken(this HttpRequestMessage requestMessage)
    {
        if (requestMessage == null)
            throw new ArgumentNullException(nameof(requestMessage));

        if (requestMessage.TryGetOption<CancellationToken>(FluentProperties.CancellationToken, out var propertyValue))
            return propertyValue;

        return CancellationToken.None;
    }

    /// <summary>
    /// Sets the cancellation token property on the specified <paramref name="requestMessage" /> properties dictionary.
    /// </summary>
    /// <param name="requestMessage">The request message containing the property.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use when sending the request message.</param>
    /// <exception cref="ArgumentNullException"><paramref name="requestMessage"/> is <see langword="null"/></exception>
    public static void SetCancellationToken(this HttpRequestMessage requestMessage, CancellationToken cancellationToken)
    {
        if (requestMessage == null)
            throw new ArgumentNullException(nameof(requestMessage));

        requestMessage.SetOption(FluentProperties.CancellationToken, cancellationToken);
    }


    /// <summary>
    /// Gets the content serializer property from the specified <paramref name="requestMessage" /> properties dictionary.
    /// </summary>
    /// <param name="requestMessage">The request message containing the property.</param>
    /// <returns>
    /// The <see cref="IContentSerializer"/> to use when serializing content to send in the request message.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="requestMessage"/> is <see langword="null"/></exception>
    public static IContentSerializer GetContentSerializer(this HttpRequestMessage requestMessage)
    {
        if (requestMessage == null)
            throw new ArgumentNullException(nameof(requestMessage));

        var propertyValue = requestMessage.GetOrAddOption(FluentProperties.ContentSerializer, k => ContentSerializer.Current);
        return propertyValue;
    }

    /// <summary>
    /// Sets the content serializer property on the specified <paramref name="requestMessage" /> properties dictionary.
    /// </summary>
    /// <param name="requestMessage">The request message containing the property.</param>
    /// <param name="contentSerializer">The <see cref="IContentSerializer"/> to use when serializing content to send in the request message.</param>
    /// <exception cref="ArgumentNullException"><paramref name="requestMessage"/> is <see langword="null"/></exception>
    public static void SetContentSerializer(this HttpRequestMessage requestMessage, IContentSerializer contentSerializer)
    {
        if (requestMessage == null)
            throw new ArgumentNullException(nameof(requestMessage));

        requestMessage.SetOption(FluentProperties.ContentSerializer, contentSerializer ?? ContentSerializer.Current);
    }


    /// <summary>
    /// Synchronizes the specified request message with the fluent properties.
    /// </summary>
    /// <param name="requestMessage">The request message.</param>
    public static void Synchronize(this HttpRequestMessage requestMessage)
    {
        var urlBuilder = requestMessage.GetUrlBuilder();
        requestMessage.RequestUri = urlBuilder.ToUri();
    }


    /// <summary>
    /// Deserialize the HTTP response message asynchronously.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <param name="responseMessage">The response message to deserialize.</param>
    /// <param name="ensureSuccess">Throw an exception if the HTTP response was unsuccessful.</param>
    /// <returns>
    /// The data object deserialized from the HTTP response message.
    /// </returns>
    /// <exception cref="HttpRequestException">Response status code does not indicate success.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="responseMessage"/> is <see langword="null"/></exception>
    public static async Task<TData> DeserializeAsync<TData>(this HttpResponseMessage responseMessage, bool ensureSuccess = true)
    {
        if (responseMessage == null)
            throw new ArgumentNullException(nameof(responseMessage));


        if (ensureSuccess)
            await responseMessage.EnsureSuccessStatusCode(true);

        var serializer = responseMessage.RequestMessage.GetContentSerializer();
        var data = await serializer
            .DeserializeAsync<TData>(responseMessage.Content)
            .ConfigureAwait(false);

        return data;
    }

    /// <summary>
    /// Throws an exception if the IsSuccessStatusCode property for the HTTP response is false.
    /// </summary>
    /// <param name="responseMessage">The response message.</param>
    /// <param name="includeContent">if set to <c>true</c> the response content is included in the exception.</param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException">The HTTP response is unsuccessful.</exception>
    public static async Task EnsureSuccessStatusCode(this HttpResponseMessage responseMessage, bool includeContent)
    {
        if (responseMessage.IsSuccessStatusCode)
            return;

        // will throw if respose is a problem json
        await CheckResponseForProblem(responseMessage).ConfigureAwait(false);

        var message = $"Response status code does not indicate success: {responseMessage.StatusCode} ({responseMessage.ReasonPhrase});";

        if (!includeContent)
            throw new HttpRequestException(message);

        var contentString = await responseMessage.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(contentString))
            throw new HttpRequestException(message);


        // add response content body to message for easier debugging
        message += Environment.NewLine + contentString;

        var exception = new HttpRequestException(message);
        exception.Data.Add("Response", contentString);

        throw exception;
    }

    private static async Task CheckResponseForProblem(HttpResponseMessage responseMessage)
    {
        string mediaType = responseMessage.Content?.Headers?.ContentType?.MediaType;
        if (!string.Equals(mediaType, ProblemDetails.ContentType, StringComparison.OrdinalIgnoreCase))
            return;

        var serializer = responseMessage.RequestMessage.GetContentSerializer();

        var problem = await serializer
            .DeserializeAsync<ProblemDetails>(responseMessage.Content)
            .ConfigureAwait(false);

        throw new ProblemException(problem);
    }
}
