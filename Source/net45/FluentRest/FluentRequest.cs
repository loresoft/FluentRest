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
#if !PORTABLE
        : ICloneable
#endif
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentRequest"/> class.
        /// </summary>
        public FluentRequest()
        {
            State = new Dictionary<string, object>();
            Headers = new Dictionary<string, ICollection<string>>();
            FormData = new Dictionary<string, ICollection<string>>();
            QueryString = new Dictionary<string, ICollection<string>>();
            Paths = new List<string>();
            Method = HttpMethod.Get;
            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }

        /// <summary>
        /// Gets or sets the state dictionary.
        /// </summary>
        /// <value>
        /// The state dictionary.
        /// </value>
        public IDictionary<string, object> State { get; set; }

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
        /// Gets or sets when the operation should complete (as soon as a response is available or after reading the whole response content).
        /// </summary>
        /// <value>
        /// When the operation should complete (as soon as a response is available or after reading the whole response content).
        /// </value>
        public HttpCompletionOption CompletionOption { get; set; }

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
        /// Gets the value for the specified <paramref name="key"/> from the request <see cref="State"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The state key.</param>
        /// <returns>The value for the specified <paramref name="key"/> if found; otherwise null.</returns>
        public T GetState<T>(string key)
        {
            object value = null;
            if (!State.TryGetValue(key, out value))
                return default(T);

            if (value == null)
                return default(T);

            return value is T ? (T)value : default(T);
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


        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public FluentRequest Clone()
        {
            var request = new FluentRequest
            {
                BaseUri = BaseUri,
                CompletionOption = CompletionOption,
                Method = Method,
                Paths = new List<string>(Paths),
                Headers = new Dictionary<string, ICollection<string>>(Headers),
                QueryString = new Dictionary<string, ICollection<string>>(QueryString),
                FormData = new Dictionary<string, ICollection<string>>(FormData)
            };


            return request;
        }

#if !PORTABLE
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
#endif

        private Uri BuildRequestPath()
        {
            if (Paths == null || Paths.Count == 0)
                return BaseUri;

            // append paths
            var basePath = BaseUri.ToString();
            basePath = AppendSlash(basePath);

            var paths = string.Join("/", Paths);
            var fullPath = basePath + paths;

            var requestPath = new Uri(fullPath, UriKind.Absolute);

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

        private static string AppendSlash(string path)
        {
            if (path == null)
                return null;

            int l = path.Length;
            if (l == 0)
                return path;

            if (path[l - 1] != '/')
                path += '/';

            return path;
        }

    }
}