using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FluentRest
{
    /// <summary>
    /// Fluent extension methods for <see cref="HttpClient"/>.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Sends a GET request using specified fluent <paramref name="builder" /> as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="httpClient" /> or <paramref name="builder" /> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Action<QueryBuilder> builder)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var requestMessage = new HttpRequestMessage { Method = HttpMethod.Get };

            var fluentBuilder = new QueryBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }
        
        /// <summary>
        /// Sends a POST request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var requestMessage = new HttpRequestMessage { Method = HttpMethod.Post };

            var fluentBuilder = new FormBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }
        
        /// <summary>
        /// Sends a PUT request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var requestMessage = new HttpRequestMessage { Method = HttpMethod.Put };

            var fluentBuilder = new FormBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }
        
        /// <summary>
        /// Sends a PATCH request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var requestMessage = new HttpRequestMessage { Method = FormBuilder.HttpPatch };

            var fluentBuilder = new FormBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }
        
        /// <summary>
        /// Sends a DELETE request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var requestMessage = new HttpRequestMessage { Method = HttpMethod.Delete };

            var fluentBuilder = new FormBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }
        
        /// <summary>
        /// Sends a request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Action<SendBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            // build request
            var requestMessage = new HttpRequestMessage();

            var fluentBuilder = new SendBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }
        

        private static async Task<HttpResponseMessage> SendAsync(HttpClient httpClient, HttpRequestMessage requestMessage)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            if (requestMessage == null)
                throw new ArgumentNullException(nameof(requestMessage));

            if (requestMessage.Headers.UserAgent.Count == 0)
            {
                // user-agent header required
                var headerValue = new ProductInfoHeaderValue(ThisAssembly.AssemblyProduct, ThisAssembly.AssemblyVersion);
                requestMessage.Headers.UserAgent.Add(headerValue);
            }

            var urlBuilder = requestMessage.GetUrlBuilder();
            requestMessage.RequestUri = urlBuilder.ToUri();

            var completionOption = requestMessage.GetCompletionOption();
            var cancellationToken = requestMessage.GetCancellationToken();

            var httpResponse = await httpClient
                .SendAsync(requestMessage, completionOption, cancellationToken)
                .ConfigureAwait(false);

            return httpResponse;
        }


        private async Task<HttpContent> GetContent(HttpRequestMessage fluentRequest)
        {
            if (fluentRequest.Method == HttpMethod.Get)
                return null;

            var contentData = fluentRequest.GetFormData();
            if (contentData is HttpContent httpContent)
                return httpContent;

            if (contentData is string stringContent)
                return new StringContent(stringContent, Encoding.UTF8, fluentRequest.ContentType ?? "application/json");

            if (contentData != null)
                return await Serializer.SerializeAsync(contentData).ConfigureAwait(false);

            if (contentData == null && (fluentRequest.FormData == null || fluentRequest.FormData.Count == 0))
                return new StringContent(String.Empty, Encoding.UTF8, fluentRequest.ContentType ?? "application/json");

            // convert NameValue to KeyValuePair
            var formData = new List<KeyValuePair<string, string>>();
            foreach (var pair in fluentRequest.FormData)
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
