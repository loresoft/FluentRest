using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FluentRest
{
    /// <summary>
    /// Provides a fluent class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI. 
    /// </summary>
    public class FluentClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentClient"/> class.
        /// </summary>
        public FluentClient()
            : this(new JsonContentSerializer(), new HttpClientHandler(), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentClient"/> class.
        /// </summary>
        /// <param name="serializer">The serializer to convert to and from HttpContent.</param>
        /// <param name="httpHandler">The HTTP handler stack to use for sending requests.</param>
        public FluentClient(IContentSerializer serializer, HttpMessageHandler httpHandler)
            : this(serializer, httpHandler, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentClient"/> class.
        /// </summary>
        /// <param name="serializer">The serializer to convert to and from HttpContent.</param>
        /// <param name="httpHandler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="disposeHandler">
        /// <c>true</c> if the inner handler should be disposed of by the Dispose method, 
        /// <c>false</c> if you intend to reuse the inner handler.
        /// </param>
        public FluentClient(IContentSerializer serializer, HttpMessageHandler httpHandler, bool disposeHandler)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            if (httpHandler == null)
                throw new ArgumentNullException(nameof(httpHandler));

            Serializer = serializer;
            HttpHandler = httpHandler;
            DisposeHandler = disposeHandler;
        }


        /// <summary>
        /// Gets or sets the base URI for requests.
        /// </summary>
        /// <value>
        /// The base URI for requests.
        /// </value>
        public Uri BaseUri { get; set; }

        /// <summary>
        /// Gets the serializer to convert to and from <see cref="HttpContent"/>.
        /// </summary>
        /// <value>
        /// The serializer to convert to and from <see cref="HttpContent"/>.
        /// </value>
        public IContentSerializer Serializer { get; }

        /// <summary>
        /// Gets the HTTP handler stack to use for sending requests.
        /// </summary>
        /// <value>
        /// The HTTP handler stack to use for sending requests.
        /// </value>
        public HttpMessageHandler HttpHandler { get; }

        /// <summary>
        /// Gets a value indicating whether the inner handler should be disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the inner handler should be disposed of by the Dispose method, 
        /// <c>false</c> if you intend to reuse the inner handler.
        /// </value>
        public bool DisposeHandler { get; }


        /// <summary>
        /// Sends a GET request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public Task<TResponse> GetAsync<TResponse>(Action<QueryBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fluentRequest = new FluentRequest();
            fluentRequest.BaseUri = BaseUri;
            fluentRequest.Method = HttpMethod.Get;

            var fluentBuilder = new QueryBuilder(fluentRequest);
            builder(fluentBuilder);

            return Send<TResponse>(fluentRequest);
        }

        /// <summary>
        /// Sends a POST request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public Task<TResponse> PostAsync<TResponse>(Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fluentRequest = new FluentRequest();
            fluentRequest.BaseUri = BaseUri;
            fluentRequest.Method = HttpMethod.Post;

            var fluentBuilder = new FormBuilder(fluentRequest);
            builder(fluentBuilder);

            return Send<TResponse>(fluentRequest);
        }

        /// <summary>
        /// Sends a PUT request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public Task<TResponse> PutAsync<TResponse>(Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fluentRequest = new FluentRequest();
            fluentRequest.BaseUri = BaseUri;
            fluentRequest.Method = HttpMethod.Put;

            var fluentBuilder = new FormBuilder(fluentRequest);
            builder(fluentBuilder);

            return Send<TResponse>(fluentRequest);
        }

        /// <summary>
        /// Sends a DELETE request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public Task<TResponse> DeleteAsync<TResponse>(Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fluentRequest = new FluentRequest();
            fluentRequest.BaseUri = BaseUri;
            fluentRequest.Method = HttpMethod.Delete;

            var fluentBuilder = new FormBuilder(fluentRequest);
            builder(fluentBuilder);

            return Send<TResponse>(fluentRequest);
        }

        /// <summary>
        /// Sends a request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public Task<TResponse> SendAsync<TResponse>(Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            // build request
            var fluentRequest = new FluentRequest { BaseUri = BaseUri };
            var fluentBuilder = new FormBuilder(fluentRequest);
            builder(fluentBuilder);

            return Send<TResponse>(fluentRequest);
        }


        private async Task<TResponse> Send<TResponse>(FluentRequest fluentRequest)
        {
            var httpRequest = new HttpRequestMessage();
            httpRequest.RequestUri = fluentRequest.RequestUri();
            httpRequest.Method = fluentRequest.Method;

            // add serializer media type
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(Serializer.ContentType));

            // copy headers
            foreach (var header in fluentRequest.Headers)
            {
                var values = header.Value.ToList();
                httpRequest.Headers.Add(header.Key, values);
            }

            httpRequest.Content = await GetContent(fluentRequest)
                .ConfigureAwait(false);

            var httpClient = new HttpClient(HttpHandler, DisposeHandler);
            
            var headerValue = new ProductInfoHeaderValue(ThisAssembly.AssemblyProduct, ThisAssembly.AssemblyVersion);
            httpClient.DefaultRequestHeaders.UserAgent.Add(headerValue);

            var httpResponse = await httpClient
                .SendAsync(httpRequest)
                .ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();

            var response = await Serializer
                .DeserializeAsync<TResponse>(httpResponse.Content)
                .ConfigureAwait(false);

            return response;
        }

        private async Task<HttpContent> GetContent(FluentRequest fluentRequest)
        {
            if (fluentRequest.Method == HttpMethod.Get)
                return null;

            if (fluentRequest.ContentData != null)
                return await Serializer.SerializeAsync(fluentRequest.ContentData).ConfigureAwait(false);

            // convert NameValue to KeyValuePair
            var formData = new List<KeyValuePair<string, string>>();
            foreach (var pair in fluentRequest.FormData)
            {
                var key = pair.Key;
                var values = pair.Value.ToList();
                formData.AddRange(values.Select(value => new KeyValuePair<string, string>(key, value)));
            }

            var httpContent = new FormUrlEncodedContent(formData);
            return httpContent;
        }
    }
}