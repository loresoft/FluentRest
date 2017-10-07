using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
            : this(new JsonContentSerializer(), new HttpClientHandler(), true, new List<IFluentClientInterceptor>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentClient"/> class.
        /// </summary>
        /// <param name="serializer">The serializer to convert to and from HttpContent.</param>
        /// <param name="httpHandler">The HTTP handler stack to use for sending requests.</param>
        public FluentClient(IContentSerializer serializer, HttpMessageHandler httpHandler)
            : this(serializer, httpHandler, true, new List<IFluentClientInterceptor>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FluentRest.FluentClient" /> class.
        /// </summary>
        /// <param name="serializer">The serializer to convert to and from HttpContent.</param>
        /// <param name="httpHandler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="disposeHandler">
        /// <c>true</c> if the inner handler should be disposed of by the Dispose method,
        /// <c>false</c> if you intend to reuse the inner handler.
        /// </param>
        public FluentClient(IContentSerializer serializer, HttpMessageHandler httpHandler, bool disposeHandler)
            : this(serializer, httpHandler, disposeHandler, new List<IFluentClientInterceptor>())
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
        /// <param name="interceptors">The list of <see cref="IFluentClientInterceptor"/> for this client..</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public FluentClient(IContentSerializer serializer, HttpMessageHandler httpHandler, bool disposeHandler, IEnumerable<IFluentClientInterceptor> interceptors)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            if (httpHandler == null)
                throw new ArgumentNullException(nameof(httpHandler));

            if (interceptors == null)
                throw new ArgumentNullException(nameof(interceptors));

            Serializer = serializer;
            HttpHandler = httpHandler;
            DisposeHandler = disposeHandler;
            Interceptors = new List<IFluentClientInterceptor>(interceptors);
            MaxRetry = 1;

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
            get => _defaultRequest.BaseUri;
            set => _defaultRequest.BaseUri = value;
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
        /// Gets the list of fluent client interceptors for this client.
        /// </summary>
        /// <value>
        /// The fluent interceptors for this client.
        /// </value>
        public IList<IFluentClientInterceptor> Interceptors { get; }

        /// <summary>
        /// Gets or sets the maximum number of times to allow retry when triggered by an interceptor.
        /// </summary>
        /// <value>
        /// The maximum number of times to allow retry.
        /// </value>
        public int MaxRetry { get; set; }

        /// <summary>
        /// Set the initial default values for all requests from this instance of <see cref="FluentClient" />.
        /// </summary>
        /// <param name="request">The default request.</param>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is <see langword="null" />.</exception>
        public void Defaults(FluentRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            _defaultRequest = request;
        }

        /// <summary>
        /// Set the initial default values for all requests from this instance of <see cref="FluentClient"/>.
        /// </summary>
        /// <param name="builder">The fluent builder factory.</param>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public void Defaults(Action<SendBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fluentBuilder = new SendBuilder(_defaultRequest);
            builder(fluentBuilder);
        }

        /// <summary>
        /// Creates a FluentRequest object that is configured with defaults.
        /// </summary>
        /// <returns>A FluentRequest object configured with defaults.</returns>
        public FluentRequest CreateRequest()
        {
            return _defaultRequest.Clone();
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

            var fluentRequest = CreateRequest();
            fluentRequest.Method = HttpMethod.Get;

            var fluentBuilder = new QueryBuilder(fluentRequest);
            builder(fluentBuilder);

            var response = await SendAsync(fluentRequest).ConfigureAwait(false);

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

            var fluentRequest = CreateRequest();
            fluentRequest.Method = HttpMethod.Post;

            var fluentBuilder = new FormBuilder(fluentRequest);
            builder(fluentBuilder);

            var response = await SendAsync(fluentRequest).ConfigureAwait(false);

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

            var fluentRequest = CreateRequest();
            fluentRequest.Method = HttpMethod.Put;

            var fluentBuilder = new FormBuilder(fluentRequest);
            builder(fluentBuilder);

            var response = await SendAsync(fluentRequest).ConfigureAwait(false);

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
        /// Sends a PATCH request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public async Task<FluentResponse> PatchAsync(Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fluentRequest = CreateRequest();
            fluentRequest.Method = FormBuilder.HttpPatch;

            var fluentBuilder = new FormBuilder(fluentRequest);
            builder(fluentBuilder);

            var response = await SendAsync(fluentRequest).ConfigureAwait(false);

            return response;
        }


        /// <summary>
        /// Sends a PATCH request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public async Task<TResponse> PatchAsync<TResponse>(Action<FormBuilder> builder)
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

            var fluentRequest = CreateRequest();
            fluentRequest.Method = HttpMethod.Delete;

            var fluentBuilder = new FormBuilder(fluentRequest);
            builder(fluentBuilder);

            var response = await SendAsync(fluentRequest).ConfigureAwait(false);

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
        public async Task<FluentResponse> SendAsync(Action<SendBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            // build request
            var fluentRequest = CreateRequest();

            var fluentBuilder = new SendBuilder(fluentRequest);
            builder(fluentBuilder);

            var response = await SendAsync(fluentRequest).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Sends a request using specified fluent builder as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public async Task<TResponse> SendAsync<TResponse>(Action<SendBuilder> builder)
        {
            var response = await SendAsync(builder).ConfigureAwait(false);
            var data = await response.DeserializeAsync<TResponse>().ConfigureAwait(false);

            return data;
        }


        /// <summary>
        /// Sends a request using specified fluent request as an asynchronous operation.
        /// </summary>
        /// <param name="fluentRequest">The fluent request to send.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="fluentRequest" /> is <see langword="null" />.</exception>
        public async Task<FluentResponse> SendAsync(FluentRequest fluentRequest)
        {
            if (fluentRequest == null)
                throw new ArgumentNullException(nameof(fluentRequest));

            var httpClient = new HttpClient(HttpHandler, DisposeHandler);

            var headerValue = new ProductInfoHeaderValue(ThisAssembly.AssemblyProduct, ThisAssembly.AssemblyVersion);
            httpClient.DefaultRequestHeaders.UserAgent.Add(headerValue);

            Exception exception = null;
            HttpResponseMessage httpResponse = null;
            FluentResponse fluentResponse;

            int count = 0;

            do
            {
                var httpRequest = await TransformRequest(fluentRequest).ConfigureAwait(false);

                try
                {
                    httpResponse = await httpClient
                        .SendAsync(httpRequest, fluentRequest.CompletionOption, fluentRequest.CancellationToken)
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                fluentResponse = await TransformResponse(fluentRequest, httpResponse, exception).ConfigureAwait(false);

                // throw if error not handled
                if (exception != null && !fluentResponse.ShouldRetry)
                    throw new FluentException(exception.Message, exception);

                // track call count to prevent infinite loop
                count++;
            } while (fluentResponse.ShouldRetry && count <= MaxRetry);

            if (fluentRequest.ExpectedStatusCode.HasValue &&
                fluentResponse.StatusCode != fluentRequest.ExpectedStatusCode.Value)
            {
                var content = await fluentResponse.HttpContent.ReadAsStringAsync();
                if (content.Length > 1000)
                    content = content.Substring(0, 1000);

                throw new HttpRequestException($"Expected status code {fluentRequest.ExpectedStatusCode.Value} but recieved status code {fluentResponse.StatusCode} ({fluentResponse.ReasonPhrase}).\n" + content);
            }

            return fluentResponse;
        }

        private async Task<HttpContent> GetContent(FluentRequest fluentRequest)
        {
            if (fluentRequest.Method == HttpMethod.Get)
                return null;

            switch (fluentRequest.ContentData)
            {
                case HttpContent content:
                    return content;
                case byte[] byteData:
                    return new ByteArrayContent(byteData)
                    {
                        Headers = { ContentType = new MediaTypeHeaderValue(fluentRequest.ContentType) }
                    };
                case Stream streamData:
                    return new StreamContent(streamData)
                    {
                        Headers = { ContentType = new MediaTypeHeaderValue(fluentRequest.ContentType) }
                    };
                case string stringData:
                    return new StringContent(stringData, Encoding.UTF8, fluentRequest.ContentType ?? "application/json");
                case null when (fluentRequest.FormData == null || fluentRequest.FormData.Count == 0):
                    return new StringContent(String.Empty, Encoding.UTF8, fluentRequest.ContentType ?? "application/json");
                case null:
                {
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
                default:
                    return await Serializer.SerializeAsync(fluentRequest.ContentData).ConfigureAwait(false);
            }
        }

        private async Task<HttpRequestMessage> TransformRequest(FluentRequest fluentRequest)
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

            httpRequest.Content = await GetContent(fluentRequest).ConfigureAwait(false);

            foreach (var builder in fluentRequest.RequestBuilders)
                httpRequest = builder(httpRequest);

            // run request interceptors
            var context = new InterceptorRequestContext(this, fluentRequest) { HttpRequest = httpRequest };
            foreach (var interceptor in Interceptors)
                await interceptor.RequestAsync(context).ConfigureAwait(false);

            return context.HttpRequest ?? httpRequest;
        }

        private async Task<FluentResponse> TransformResponse(FluentRequest fluentRequest, HttpResponseMessage httpResponse, Exception exception)
        {
            var fluentResponse = new FluentResponse(Serializer, httpResponse?.Content);
            fluentResponse.ReasonPhrase = httpResponse?.ReasonPhrase;
            fluentResponse.StatusCode = httpResponse?.StatusCode ?? System.Net.HttpStatusCode.InternalServerError;
            fluentResponse.Request = fluentRequest;

            var headers = new Dictionary<string, ICollection<string>>();
            if (httpResponse != null)
                foreach (var header in httpResponse.Headers)
                    headers.Add(header.Key, header.Value.ToList());

            fluentResponse.Headers = headers;

            // run response interceptors
            var context = new InterceptorResponseContext(this, httpResponse, exception) { Response = fluentResponse };
            //TODO consider reversing
            foreach (var interceptor in Interceptors)
                await interceptor.ResponseAsync(context).ConfigureAwait(false);

            return context.Response ?? fluentResponse;
        }


        /// <summary>
        /// Creates an instance of <see cref="FluentClient"/> with specified fluent <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The fluent builder.</param>
        /// <returns>A new instance of <see cref="FluentClient"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public static FluentClient Create(Action<FluentClientBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));


            var fluentClientBuilder = new FluentClientBuilder();
            builder(fluentClientBuilder);

            var client = new FluentClient(
                fluentClientBuilder.ContentSerializer,
                fluentClientBuilder.MessageHandler,
                fluentClientBuilder.ShouldDisposeHandler,
                fluentClientBuilder.Interceptors);

            client.Defaults(fluentClientBuilder.DefaultRequest);

            return client;
        }
    }
}