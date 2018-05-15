using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentRest
{
    /// <summary>
    ///
    /// </summary>
    public static class FluentClientExtensions
    {
        /// <summary>
        /// Sends a GET request using specified fluent <paramref name="builder" /> as an asynchronous operation.
        /// </summary>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="fluentClient" /> or <paramref name="builder" /> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> GetAsync(this IFluentClient fluentClient, Action<QueryBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var httpClient = fluentClient.HttpClient;
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, httpClient.BaseAddress);
            requestMessage.SetContentSerializer(fluentClient.ContentSerializer);

            var fluentBuilder = new QueryBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await FluentDispatcher.SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends a GET request using specified fluent <paramref name="builder"/> as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fluentClient" /> or <paramref name="builder" /> is <see langword="null" />.</exception>
        public static async Task<TResponse> GetAsync<TResponse>(this IFluentClient fluentClient, Action<QueryBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.GetAsync(builder).ConfigureAwait(false);
            var data = await response.DeserializeAsync<TResponse>().ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a POST request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fluentClient" /> or <paramref name="builder" /> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> PostAsync(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var httpClient = fluentClient.HttpClient;
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, httpClient.BaseAddress);
            requestMessage.SetContentSerializer(fluentClient.ContentSerializer);

            var fluentBuilder = new FormBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await FluentDispatcher.SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends a POST request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fluentClient" /> or <paramref name="builder" /> is <see langword="null" />.</exception>
        public static async Task<TResponse> PostAsync<TResponse>(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.PostAsync(builder).ConfigureAwait(false);
            var data = await response.DeserializeAsync<TResponse>().ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a PUT request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fluentClient" /> or <paramref name="builder" /> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> PutAsync(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var httpClient = fluentClient.HttpClient;
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, httpClient.BaseAddress);
            requestMessage.SetContentSerializer(fluentClient.ContentSerializer);

            var fluentBuilder = new FormBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await FluentDispatcher.SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends a PUT request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fluentClient" /> or <paramref name="builder" /> is <see langword="null" />.</exception>
        public static async Task<TResponse> PutAsync<TResponse>(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.PutAsync(builder).ConfigureAwait(false);
            var data = await response.DeserializeAsync<TResponse>().ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a PATCH request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fluentClient" /> or <paramref name="builder" /> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> PatchAsync(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var httpClient = fluentClient.HttpClient;
            var requestMessage = new HttpRequestMessage(FormBuilder.HttpPatch, httpClient.BaseAddress);
            requestMessage.SetContentSerializer(fluentClient.ContentSerializer);

            var fluentBuilder = new FormBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await FluentDispatcher.SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends a PATCH request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fluentClient" /> or <paramref name="builder" /> is <see langword="null" />.</exception>
        public static async Task<TResponse> PatchAsync<TResponse>(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.PatchAsync(builder).ConfigureAwait(false);
            var data = await response.DeserializeAsync<TResponse>().ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a DELETE request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fluentClient" /> or <paramref name="builder" /> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> DeleteAsync(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var httpClient = fluentClient.HttpClient;
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, httpClient.BaseAddress);
            requestMessage.SetContentSerializer(fluentClient.ContentSerializer);

            var fluentBuilder = new FormBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await FluentDispatcher.SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends a DELETE request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fluentClient" /> or <paramref name="builder" /> is <see langword="null" />.</exception>
        public static async Task<TResponse> DeleteAsync<TResponse>(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.DeleteAsync(builder).ConfigureAwait(false);
            var data = await response.DeserializeAsync<TResponse>().ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fluentClient" /> or <paramref name="builder" /> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> SendAsync(this IFluentClient fluentClient, Action<SendBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var httpClient = fluentClient.HttpClient;
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, httpClient.BaseAddress);
            requestMessage.SetContentSerializer(fluentClient.ContentSerializer);

            var fluentBuilder = new SendBuilder(requestMessage);
            builder(fluentBuilder);

            var response = await FluentDispatcher.SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends a request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fluentClient" /> or <paramref name="builder" /> is <see langword="null" />.</exception>
        public static async Task<TResponse> SendAsync<TResponse>(this IFluentClient fluentClient, Action<SendBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.SendAsync(builder).ConfigureAwait(false);
            var data = await response.DeserializeAsync<TResponse>().ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a request using specified request message as an asynchronous operation.
        /// </summary>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="requestMessage">The request message to send.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fluentClient" /> or <paramref name="requestMessage" /> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> SendAsync(this IFluentClient fluentClient, HttpRequestMessage requestMessage)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (requestMessage == null)
                throw new ArgumentNullException(nameof(requestMessage));

            var httpClient = fluentClient.HttpClient;

            requestMessage.SetContentSerializer(fluentClient.ContentSerializer);

            var response = await FluentDispatcher.SendAsync(httpClient, requestMessage).ConfigureAwait(false);

            return response;
        }

    }
}