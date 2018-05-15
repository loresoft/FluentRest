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

            var response =  await fluentClient.HttpClient.GetAsync(builder).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Sends a GET request using specified fluent <paramref name="builder"/> as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<TResponse> GetAsync<TResponse>(this IFluentClient fluentClient, Action<QueryBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.HttpClient.GetAsync(builder).ConfigureAwait(false);
            var data = await DeserializeAsync<TResponse>(fluentClient, response).ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a POST request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> PostAsync(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.HttpClient.PostAsync(builder).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Sends a POST request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<TResponse> PostAsync<TResponse>(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.HttpClient.PostAsync(builder).ConfigureAwait(false);
            var data = await DeserializeAsync<TResponse>(fluentClient, response).ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a PUT request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> PutAsync(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.HttpClient.PutAsync(builder).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Sends a PUT request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<TResponse> PutAsync<TResponse>(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.HttpClient.PutAsync(builder).ConfigureAwait(false);
            var data = await DeserializeAsync<TResponse>(fluentClient, response).ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a PATCH request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> PatchAsync(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.HttpClient.PatchAsync(builder).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Sends a PATCH request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<TResponse> PatchAsync<TResponse>(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.HttpClient.PatchAsync(builder).ConfigureAwait(false);
            var data = await DeserializeAsync<TResponse>(fluentClient, response).ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a DELETE request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> DeleteAsync(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.HttpClient.DeleteAsync(builder).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Sends a DELETE request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<TResponse> DeleteAsync<TResponse>(this IFluentClient fluentClient, Action<FormBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.HttpClient.DeleteAsync(builder).ConfigureAwait(false);
            var data = await DeserializeAsync<TResponse>(fluentClient, response).ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<HttpResponseMessage> SendAsync(this IFluentClient fluentClient, Action<SendBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.HttpClient.SendAsync(builder).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Sends a request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="fluentClient">The <see cref="IFluentClient"/> used to send request.</param>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static async Task<TResponse> SendAsync<TResponse>(this IFluentClient fluentClient, Action<SendBuilder> builder)
        {
            if (fluentClient == null)
                throw new ArgumentNullException(nameof(fluentClient));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var response = await fluentClient.HttpClient.SendAsync(builder).ConfigureAwait(false);
            var data = await DeserializeAsync<TResponse>(fluentClient, response).ConfigureAwait(false);

            return data;
        }


        private static async Task<TData> DeserializeAsync<TData>(IFluentClient fluentClient, HttpResponseMessage responseMessage, bool ensureSuccess = true)
        {
            if (responseMessage == null)
                throw new ArgumentNullException(nameof(responseMessage));

            var serializer = fluentClient.ContentSerializer;

            if (ensureSuccess)
                responseMessage.EnsureSuccessStatusCode();

            var data = await serializer
                .DeserializeAsync<TData>(responseMessage.Content)
                .ConfigureAwait(false);

            return data;
        }

    }
}