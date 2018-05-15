using System;
using System.Net.Http;
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

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, httpClient.BaseAddress);

            var fluentBuilder = new QueryBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await FluentDispatcher.SendAsync(httpClient, requestMessage).ConfigureAwait(false);

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

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, httpClient.BaseAddress);

            var fluentBuilder = new FormBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await FluentDispatcher.SendAsync(httpClient, requestMessage).ConfigureAwait(false);

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

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, httpClient.BaseAddress);

            var fluentBuilder = new FormBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await FluentDispatcher.SendAsync(httpClient, requestMessage).ConfigureAwait(false);

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

            var requestMessage = new HttpRequestMessage(FormBuilder.HttpPatch, httpClient.BaseAddress);

            var fluentBuilder = new FormBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await FluentDispatcher.SendAsync(httpClient, requestMessage).ConfigureAwait(false);

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

            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, httpClient.BaseAddress);

            var fluentBuilder = new FormBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await FluentDispatcher.SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends a request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, Action<SendBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            // build request
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, httpClient.BaseAddress);

            var fluentBuilder = new SendBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await FluentDispatcher.SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }
    }
}
