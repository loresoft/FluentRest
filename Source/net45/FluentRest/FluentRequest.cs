using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace FluentRest
{
    /// <summary>
    /// Represents a Fluent HTTP request message.
    /// </summary>
    public class FluentRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentRequest"/> class.
        /// </summary>
        public FluentRequest()
        {
            Headers = new Dictionary<string, ICollection<string>>();
            FormData = new Dictionary<string, ICollection<string>>();
            QueryString = new Dictionary<string, ICollection<string>>();
            Paths = new List<string>();
            Method = HttpMethod.Get;
        }

        /// <summary>
        /// Gets or sets the collection of HTTP request headers.
        /// </summary>
        /// <value>
        /// The collection of HTTP request headers.
        /// </value>
        public IDictionary<string, ICollection<string>> Headers { get; set; }

        /// <summary>
        /// Gets or sets the collection of HTTP form data.
        /// </summary>
        /// <value>
        /// The collection of HTTP form data.
        /// </value>
        public IDictionary<string, ICollection<string>> FormData { get; set; }

        /// <summary>
        /// Gets or sets the collection of URI query string parameters.
        /// </summary>
        /// <value>
        /// The collection of URI query string parameters.
        /// </value>
        public IDictionary<string, ICollection<string>> QueryString { get; set; }

        /// <summary>
        /// Gets or sets the collection of URI segment paths.
        /// </summary>
        /// <value>
        /// The collection of URI segment paths.
        /// </value>
        public IList<string> Paths { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method used by the HTTP request message.
        /// </summary>
        /// <value>
        /// The HTTP method used by the HTTP request message.
        /// </value>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Gets or sets the base URI address used when sending requests.
        /// </summary>
        /// <value>
        /// The base URI address used when sending requests.
        /// </value>
        public Uri BaseUri { get; set; }

        /// <summary>
        /// Gets or sets the data object to serialize as the form content.
        /// </summary>
        /// <value>
        /// The data object to serialize as the form content.
        /// </value>
        public object ContentData { get; set; }


        /// <summary>
        /// Gets the computed Uri used for the HTTP request.
        /// </summary>
        /// <returns>The computed <see cref="Uri"/> used for the HTTP request.</returns>
        /// <exception cref="FluentException">BaseUri is required.</exception>
        public Uri RequestUri()
        {
            Validate();

            var requestUri = BuildRequestPath();
            requestUri = BuildQueryString(requestUri);

            return requestUri;
        }


        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <exception cref="FluentException">BaseUri is required.</exception>
        public void Validate()
        {
            if (BaseUri == null)
                throw new FluentException("BaseUri is required.");

        }


        private Uri BuildRequestPath()
        {
            if (Paths == null || Paths.Count == 0)
                return BaseUri;

            // append paths
            var paths = string.Join("/", Paths);
            var pathUri = new Uri(paths, UriKind.RelativeOrAbsolute);
            var requestPath = new Uri(BaseUri, pathUri);

            return requestPath;
        }

        private Uri BuildQueryString(Uri uri)
        {
            if (QueryString == null || QueryString.Count == 0)
                return uri;

            var queryString = new StringBuilder();

            foreach (var pair in QueryString)
            {
                var key = pair.Key;
                var values = pair.Value.ToList();

                foreach (var value in values)
                {
                    if (queryString.Length > 0)
                        queryString.Append("&");

                    queryString
                        .Append(Uri.EscapeDataString(key))
                        .Append("=")
                        .Append(Uri.EscapeDataString(value));
                }
            }

            var builder = new UriBuilder(uri);
            builder.Query = queryString.ToString();

            return builder.Uri;
        }
    }
}