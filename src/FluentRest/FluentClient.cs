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
    public class FluentClient : IFluentClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentClient" /> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        public FluentClient(HttpClient httpClient) : this(httpClient, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentClient" /> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="contentSerializer">The content serializer.</param>
        public FluentClient(HttpClient httpClient, IContentSerializer contentSerializer)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            ContentSerializer = contentSerializer ?? new JsonContentSerializer();
        }

        /// <summary>
        /// Gets the <see cref="HttpClient" /> used to send request.
        /// </summary>
        /// <value>
        /// The <see cref="HttpClient" /> used to send request.
        /// </value>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// Gets the serializer to convert to and from <see cref="HttpContent" />.
        /// </summary>
        /// <value>
        /// The serializer to convert to and from <see cref="HttpContent" />.
        /// </value>
        public IContentSerializer ContentSerializer { get; }
    }
}