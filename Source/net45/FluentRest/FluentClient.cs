using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace FluentRest
{
    /// <summary>
    /// Provides a fluent class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI. 
    /// </summary>
    public class FluentClient
    {
        private FluentRequest _defaultRequest;

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

            _defaultRequest = new FluentRequest();
        }



        /// <summary>
        /// Gets or sets the base URI for requests.
        /// </summary>
        /// <value>
        /// The base URI for requests.
        /// </value>
        public Uri BaseUri
        {
            get { return _defaultRequest.BaseUri; }
            set { _defaultRequest.BaseUri = value; }
        }

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
        /// Set the initial default values for all request from this instance of <see cref="FluentClient"/>.
        /// </summary>
        /// <param name="builder">The fluent builder factory.</param>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public void Defaults(Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fluentBuilder = new FormBuilder(_defaultRequest);
            builder(fluentBuilder);
        }


        /// <summary>
        /// Sends a GET request using specified fluent <paramref name="builder"/> as an asynchronous operation.
        /// </summary>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public async Task<FluentResponse> GetAsync(Action<QueryBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fluentRequest = _defaultRequest.Clone();
            fluentRequest.Method = HttpMethod.Get;

            var fluentBuilder = new QueryBuilder(fluentRequest);
            builder(fluentBuilder);

            var token = fluentBuilder.Token;
            var response = await Send(fluentRequest, token).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends a GET request using specified fluent <paramref name="builder"/> as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public async Task<TResponse> GetAsync<TResponse>(Action<QueryBuilder> builder)
        {
            var response = await GetAsync(builder).ConfigureAwait(false);
            var data = await response.DeserializeAsync<TResponse>().ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a POST request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public async Task<FluentResponse> PostAsync(Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fluentRequest = _defaultRequest.Clone();
            fluentRequest.Method = HttpMethod.Post;

            var fluentBuilder = new FormBuilder(fluentRequest);
            builder(fluentBuilder);

            var token = fluentBuilder.Token;
            var response = await Send(fluentRequest, token).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends a POST request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public async Task<TResponse> PostAsync<TResponse>(Action<FormBuilder> builder)
        {
            var response = await PostAsync(builder).ConfigureAwait(false);
            var data = await response.DeserializeAsync<TResponse>().ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a PUT request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public async Task<FluentResponse> PutAsync(Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fluentRequest = _defaultRequest.Clone();
            fluentRequest.Method = HttpMethod.Put;

            var fluentBuilder = new FormBuilder(fluentRequest);
            builder(fluentBuilder);

            var token = fluentBuilder.Token;
            var response = await Send(fluentRequest, token).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends a PUT request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public async Task<TResponse> PutAsync<TResponse>(Action<FormBuilder> builder)
        {
            var response = await PutAsync(builder).ConfigureAwait(false);
            var data = await response.DeserializeAsync<TResponse>().ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a DELETE request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public async Task<FluentResponse> DeleteAsync(Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fluentRequest = _defaultRequest.Clone();
            fluentRequest.Method = HttpMethod.Delete;

            var fluentBuilder = new FormBuilder(fluentRequest);
            builder(fluentBuilder);

            var token = fluentBuilder.Token;
            var response = await Send(fluentRequest, token).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends a DELETE request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public async Task<TResponse> DeleteAsync<TResponse>(Action<FormBuilder> builder)
        {
            var response = await DeleteAsync(builder).ConfigureAwait(false);
            var data = await response.DeserializeAsync<TResponse>().ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public async Task<FluentResponse> SendAsync(Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            // build request
            var fluentRequest = _defaultRequest.Clone();
            var fluentBuilder = new FormBuilder(fluentRequest);
            builder(fluentBuilder);

            var token = fluentBuilder.Token;
            var response = await Send(fluentRequest, token).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends a request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public async Task<TResponse> SendAsync<TResponse>(Action<FormBuilder> builder)
        {
            var response = await PostAsync(builder).ConfigureAwait(false);
            var data = await response.DeserializeAsync<TResponse>().ConfigureAwait(false);

            return data;
        }


        private async Task<FluentResponse> Send(FluentRequest fluentRequest, CancellationToken cancellationToken)
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
                .SendAsync(httpRequest, fluentRequest.CompletionOption, cancellationToken)
                .ConfigureAwait(false);

            var fluentResponse = new FluentResponse(Serializer, httpResponse.Content);
            fluentResponse.ReasonPhrase = httpResponse.ReasonPhrase;
            fluentResponse.StatusCode = httpResponse.StatusCode;
            fluentResponse.Request = fluentRequest;

            var headers = new Dictionary<string, ICollection<string>>();
            foreach (var header in httpResponse.Headers)
                headers.Add(header.Key, header.Value.ToList());

            fluentResponse.Headers = headers;

            return fluentResponse;
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