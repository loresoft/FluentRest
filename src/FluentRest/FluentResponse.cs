using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentRest
{
    /// <summary>
    /// Represents a Fluent HTTP response message.
    /// </summary>
    public class FluentResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentResponse"/> class.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="httpContent">Content of the HTTP.</param>
        public FluentResponse(IContentSerializer serializer, HttpContent httpContent)
        {
            Serializer = serializer;
            HttpContent = httpContent;
        }

        /// <summary>
        /// Gets or sets the collection of HTTP response headers.
        /// </summary>
        /// <value>
        /// The collection of HTTP response headers.
        /// </value>
        public IDictionary<string, ICollection<string>> Headers { get; set; }

        /// <summary>
        /// Gets or sets the reason phrase which typically is sent by servers together with the status code.
        /// </summary>
        /// <value>
        /// The reason phrase which typically is sent by servers together with the status code.
        /// </value>
        public string ReasonPhrase { get; set; }

        /// <summary>
        /// Gets or sets the status code of the HTTP response.
        /// </summary>
        /// <value>
        /// The status code of the HTTP response.
        /// </value>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets a value that indicates if the HTTP response was successful.
        /// </summary>
        /// <returns>
        /// Returns value that indicates if the HTTP response was successful. <see langword="true"/> if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode"/> was in the range 200-299; otherwise false.
        /// </returns>
        public bool IsSuccessStatusCode => StatusCode >= HttpStatusCode.OK && StatusCode <= (HttpStatusCode)299;

        /// <summary>
        /// Gets or sets the fluent request which led to this response.
        /// </summary>
        /// <value>
        /// The fluent request which led to this response.
        /// </value>
        public FluentRequest Request { get; set; }

        /// <summary>
        /// Gets or sets the response HttpContent.
        /// </summary>
        /// <value>
        /// The response HttpContent.
        /// </value>
        public HttpContent HttpContent { get; }

        /// <summary>
        /// Gets the response serializer.
        /// </summary>
        /// <value>
        /// The response serializer.
        /// </value>
        public IContentSerializer Serializer { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the request should be retried.
        /// </summary>
        /// <value>
        /// <c>true</c> if the request should be retried; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This flag can be used by interceptors to cause the current request to be resubmit.
        /// </remarks>
        public bool ShouldRetry { get; set; }

        /// <summary>
        /// Throws an exception if the <see cref="IsSuccessStatusCode"/> property for the HTTP response is false.
        /// </summary>
        /// <returns>The HTTP response message if the call is successful.</returns>
        /// <exception cref="HttpRequestException">Response status code does not indicate success.</exception>
        public FluentResponse EnsureSuccessStatusCode()
        {
            if (IsSuccessStatusCode)
                return this;

            HttpContent?.Dispose();

            int statusCode = (int)StatusCode;
            throw new HttpRequestException($"Response status code does not indicate success: {statusCode} ({ReasonPhrase}).");
        }

        /// <summary>
        /// Deserialize the <see cref="System.Net.Http.HttpContent" /> asynchronously.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="ensureSuccess">Throw an exception if the <see cref="IsSuccessStatusCode"/> property for the HTTP response is false.</param>
        /// <returns>
        /// The data object deserialized from the HttpContent.
        /// </returns>
        /// <exception cref="HttpRequestException">Response status code does not indicate success.</exception>
        public async Task<TData> DeserializeAsync<TData>(bool ensureSuccess = true)
        {
            if (ensureSuccess)
                EnsureSuccessStatusCode();

            var response = await Serializer
                .DeserializeAsync<TData>(HttpContent)
                .ConfigureAwait(false);

            return response;
        }
    }
}