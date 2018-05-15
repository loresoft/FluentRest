using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FluentRest
{
    /// <summary>
    /// Extension method for <see cref="HttpRequestMessage"/>
    /// </summary>
    public static class HttpMessageExtensions
    {
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

            var propertyValue = requestMessage.Properties.GetOrAdd(FluentProperties.RequestUrlBuilder, k =>
                requestMessage.RequestUri == null
                    ? new UrlBuilder()
                    : new UrlBuilder(requestMessage.RequestUri)
            );

            return propertyValue as UrlBuilder;
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

            requestMessage.Properties[FluentProperties.RequestUrlBuilder] = urlBuilder;
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

            requestMessage.Properties.TryGetValue(FluentProperties.RequestContentData, out var propertyValue);
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

            requestMessage.Properties[FluentProperties.RequestContentData] = contentData;
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

            var propertyValue = requestMessage.Properties.GetOrAdd(FluentProperties.RequestFormData, k => new Dictionary<string, ICollection<string>>());
            return propertyValue as Dictionary<string, ICollection<string>>;
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

            requestMessage.Properties.TryGetValue(FluentProperties.HttpCompletionOption, out var propertyValue);
            return (HttpCompletionOption)(propertyValue ?? HttpCompletionOption.ResponseContentRead);
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

            requestMessage.Properties[FluentProperties.HttpCompletionOption] = completionOption;
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

            requestMessage.Properties.TryGetValue(FluentProperties.CancellationToken, out var propertyValue);
            return (CancellationToken)(propertyValue ?? CancellationToken.None);
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

            requestMessage.Properties[FluentProperties.CancellationToken] = cancellationToken;
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

            var propertyValue = requestMessage.Properties.GetOrAdd(FluentProperties.ContentSerializer, k => ContentSerializer.Current);
            return propertyValue as IContentSerializer;
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

            requestMessage.Properties[FluentProperties.ContentSerializer] = contentSerializer ?? ContentSerializer.Current;
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

            var serializer = responseMessage.RequestMessage.GetContentSerializer();

            if (ensureSuccess)
                responseMessage.EnsureSuccessStatusCode();

            var data = await serializer
                .DeserializeAsync<TData>(responseMessage.Content)
                .ConfigureAwait(false);

            return data;
        }

    }
}