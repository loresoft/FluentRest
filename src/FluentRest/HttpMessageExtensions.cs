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

            var propertyValue = requestMessage.Properties.GetOrAdd(FluentProperties.RequestUrlBuilder, k => new UrlBuilder(requestMessage.RequestUri));
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

            var propertyValue = requestMessage.Properties[FluentProperties.RequestUrlBuilder] = urlBuilder;
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
    }
}