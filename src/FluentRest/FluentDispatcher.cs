using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FluentRest
{
    /// <summary>
    /// A class to send an HTTP request from a fluently built request message
    /// </summary>
    public static class FluentDispatcher
    {
        /// <summary>
        /// Sends a request using specified fluent <paramref name="requestMessage" /> as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient" /> used to send request.</param>
        /// <param name="requestMessage">The request message.</param>
        /// <returns>The HTTP response message return after request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="httpClient"/> or <paramref name="requestMessage"/> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> SendAsync(HttpClient httpClient, HttpRequestMessage requestMessage)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            if (requestMessage == null)
                throw new ArgumentNullException(nameof(requestMessage));

            requestMessage = await PrepareRequest(requestMessage).ConfigureAwait(false);

            var completionOption = requestMessage.GetCompletionOption();
            var cancellationToken = requestMessage.GetCancellationToken();

            var httpResponse = await httpClient
                .SendAsync(requestMessage, completionOption, cancellationToken)
                .ConfigureAwait(false);

            return httpResponse;
        }


        private static async Task<HttpRequestMessage> PrepareRequest(HttpRequestMessage requestMessage)
        {

            // add serializer media type
            var serializer = requestMessage.GetContentSerializer();
            var acceptHeader = new MediaTypeWithQualityHeaderValue(serializer.ContentType);
            requestMessage.Headers.Accept.Add(acceptHeader);

            if (requestMessage.Headers.UserAgent.Count == 0)
            {
                // user-agent header required
                var headerValue = new ProductInfoHeaderValue(ThisAssembly.AssemblyProduct, ThisAssembly.AssemblyVersion);
                requestMessage.Headers.UserAgent.Add(headerValue);
            }

            // set request uri from builder
            var urlBuilder = requestMessage.GetUrlBuilder();
            requestMessage.RequestUri = urlBuilder.ToUri();

            // set content from serializer
            var httpContent = await GetContent(requestMessage).ConfigureAwait(false);
            requestMessage.Content = httpContent;

            return requestMessage;
        }

        private static async Task<HttpContent> GetContent(HttpRequestMessage requestMessage)
        {
            if (requestMessage.Method == HttpMethod.Get)
                return null;

            // don't do anything if already has content
            if (requestMessage.Content != null)
                return requestMessage.Content;

            var contentData = requestMessage.GetContentData();
            if (contentData is HttpContent httpContent)
                return httpContent;

            var serializer = requestMessage.GetContentSerializer();
            if (contentData is string stringContent)
                return new StringContent(stringContent, Encoding.UTF8, serializer.ContentType);

            // content data overrides form data
            if (contentData != null)
                return await serializer.SerializeAsync(contentData).ConfigureAwait(false);

            var formDictionary = requestMessage.GetFormData();

            if (formDictionary == null || formDictionary.Count == 0)
                return new StringContent(string.Empty, Encoding.UTF8, serializer.ContentType);

            // convert NameValue to KeyValuePair
            var formData = new List<KeyValuePair<string, string>>();
            foreach (var pair in formDictionary)
            {
                var key = pair.Key;
                var values = pair.Value.ToList();
                formData.AddRange(values.Select(value => new KeyValuePair<string, string>(key, value)));
            }

            var formContent = new FormUrlEncodedContent(formData);
            return formContent;

        }
    }
}