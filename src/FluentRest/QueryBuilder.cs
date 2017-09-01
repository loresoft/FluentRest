using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace FluentRest
{
    /// <summary>
    /// A fluent query builder.
    /// </summary>
    public sealed class QueryBuilder : QueryBuilder<QueryBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder" /> class.
        /// </summary>
        /// <param name="request">The fluent HTTP request being built.</param>
        public QueryBuilder(FluentRequest request) : base(request)
        {
        }

    }

    /// <summary>
    /// A fluent query builder.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder.</typeparam>
    public abstract class QueryBuilder<TBuilder> : RequestBuilder<TBuilder>
        where TBuilder : QueryBuilder<TBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder{TBuilder}"/> class.
        /// </summary>
        /// <param name="request">The fluent HTTP request being built.</param>
        protected QueryBuilder(FluentRequest request) : base(request)
        {
        }

        /// <summary>
        /// Sets the expected HTTP status code of the response. If set and the status code does not match, an <exception cref="HttpRequestException">exception</exception> will be thrown.
        /// </summary>
        /// <param name="status">The expected status.</param>
        /// <returns>A fluent request builder.</returns>
        public TBuilder ExpectedStatus(HttpStatusCode status)
        {
            Request.ExpectedStatusCode = status;

            return this as TBuilder;
        }

        /// <summary>
        /// Start a fluent header builder.
        /// </summary>
        /// <param name="builder">The builder factory.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
        public TBuilder Header(Action<HeaderBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var headerBuilder = new HeaderBuilder(Request);
            builder(headerBuilder);

            return this as TBuilder;
        }

        /// <summary>
        /// Sets HTTP header with the specified <paramref name="name"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public TBuilder Header(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (value == null)
                Request.Headers.Remove(name);
            else
                Request.Headers[name] = new List<string>(new[] { value });

            return this as TBuilder;
        }

        /// <summary>
        /// Sets HTTP header with the specified <paramref name="name"/> and <paramref name="value"/> if the specified <paramref name="condition"/> is true.
        /// </summary>
        /// <param name="condition">If condition is true, header will be added; otherwise ignore header.</param>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public TBuilder HeaderIf(Func<bool> condition, string name, string value)
        {
            if (condition == null || !condition())
                return this as TBuilder;

            return Header(name, value);
        }


        /// <summary>
        /// Sets the base URI address used when sending requests.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is <see langword="null" />.</exception>
        public TBuilder BaseUri(Uri path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            Request.BaseUri = path;
            return this as TBuilder;
        }

        /// <summary>
        /// Sets the base URI address used when sending requests.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is <see langword="null" />.</exception>
        public TBuilder BaseUri(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            Request.BaseUri = new Uri(path, UriKind.Absolute);
            return this as TBuilder;
        }


        /// <summary>
        /// Sets the <see cref="P:FluentRest.FluentRequest.BaseUri"/> and the <see cref="P:FluentRest.FluentRequest.QueryString"/> from the specified Uri.
        /// </summary>
        /// <param name="path">The full Uri path.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is <see langword="null" />.</exception>
        public TBuilder FullUri(Uri path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var baseBuilder = new UriBuilder(path) { Query = string.Empty };

            // set BaseUri
            BaseUri(baseBuilder.Uri);

            // Set QueryString
            ParseQueryString(path.Query);

            return this as TBuilder;
        }

        /// <summary>
        /// Sets the <see cref="P:FluentRest.FluentRequest.BaseUri"/> and the <see cref="P:FluentRest.FluentRequest.QueryString"/> from the specified Uri.
        /// </summary>
        /// <param name="path">The full Uri path.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is <see langword="null" />.</exception>
        public TBuilder FullUri(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var u = new Uri(path, UriKind.Absolute);
            return FullUri(u);
        }


        /// <summary>
        /// Appends the specified <paramref name="path"/> to the BaseUri of the request.
        /// </summary>
        /// <param name="path">The path to append.</param>
        /// <returns>A fluent request builder.</returns>
        public TBuilder AppendPath(Uri path)
        {
            if (path != null)
                Request.Paths.Add(path.ToString());

            return this as TBuilder;
        }

        /// <summary>
        /// Appends the specified <paramref name="path" /> to the BaseUri of the request.
        /// </summary>
        /// <param name="path">The path to append.</param>
        /// <param name="encode">if <see langword="true"/>, URL encode the specified <paramref name="path"/>.</param>
        /// <returns>
        /// A fluent request builder.
        /// </returns>
        public TBuilder AppendPath(string path, bool encode = false)
        {
            if (path == null)
                return this as TBuilder;

            var s = encode ? Uri.EscapeUriString(path) : path;
            Request.Paths.Add(s);

            return this as TBuilder;
        }

        /// <summary>
        /// Appends the specified <paramref name="paths"/> to the BaseUri of the request.
        /// </summary>
        /// <param name="paths">The paths to append.</param>
        /// <param name="encode">if <see langword="true"/>, URL encode the specified <paramref name="paths"/>.</param>
        /// <returns>A fluent request builder.</returns>
        public TBuilder AppendPath(IEnumerable<string> paths, bool encode = false)
        {
            if (paths == null)
                return this as TBuilder;

            foreach (var path in paths)
            {
                var s = encode ? Uri.EscapeUriString(path) : path;
                Request.Paths.Add(s);
            }

            return this as TBuilder;
        }


        /// <summary>
        /// Appends the specified <paramref name="name"/> and <paramref name="value"/> to the request Uri.
        /// </summary>
        /// <param name="name">The query parameter name.</param>
        /// <param name="value">The query parameter value.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public TBuilder QueryString(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var v = value ?? string.Empty;

            var list = Request.QueryString.GetOrAdd(name, n => new List<string>());
            list.Add(v);

            return this as TBuilder;

        }

        /// <summary>
        /// Appends the specified <paramref name="name" /> and <paramref name="value" /> to the request Uri.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The query parameter name.</param>
        /// <param name="value">The query parameter value.</param>
        /// <returns>
        /// A fluent request builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public TBuilder QueryString<TValue>(string name, TValue value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var v = value != null ? value.ToString() : string.Empty;
            return QueryString(name, v);
        }

        /// <summary>
        /// Appends the specified <paramref name="name"/> and <paramref name="value"/> to the request Uri if the specified <paramref name="condition"/> is true.
        /// </summary>
        /// <param name="condition">If condition is true, query string will be added; otherwise ignore query string.</param>
        /// <param name="name">The query parameter name.</param>
        /// <param name="value">The query parameter value.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public TBuilder QueryStringIf(Func<bool> condition, string name, string value)
        {
            if (condition == null || !condition())
                return this as TBuilder;

            return QueryString(name, value);
        }

        /// <summary>
        /// Appends the specified <paramref name="name" /> and <paramref name="value" /> to the request Uri if the specified <paramref name="condition" /> is true.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="condition">If condition is true, query string will be added; otherwise ignore query string.</param>
        /// <param name="name">The query parameter name.</param>
        /// <param name="value">The query parameter value.</param>
        /// <returns>
        /// A fluent request builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public TBuilder QueryStringIf<TValue>(Func<bool> condition, string name, TValue value)
        {
            if (condition == null || !condition())
                return this as TBuilder;

            return QueryString(name, value);
        }

        // based on HttpUtility.ParseQueryString
        private void ParseQueryString(string s)
        {
            if (string.IsNullOrEmpty(s))
                return;

            int l = s.Length;
            int i = 0;

            // remove leading ?
            if (s[0] == '?')
                i = 1;

            while (i < l)
            {
                // find next & while noting first = on the way (and if there are more)
                int si = i;
                int ti = -1;

                while (i < l)
                {
                    char ch = s[i];

                    if (ch == '=')
                    {
                        if (ti < 0)
                            ti = i;
                    }
                    else if (ch == '&')
                    {
                        break;
                    }

                    i++;
                }

                // extract the name / value pair
                string name = null;
                string value = null;

                if (ti >= 0)
                {
                    name = s.Substring(si, ti - si);
                    value = s.Substring(ti + 1, i - ti - 1);
                }
                else
                {
                    value = s.Substring(si, i - si);
                }

                // decode
                name = string.IsNullOrEmpty(name) ? string.Empty : Uri.UnescapeDataString(name);
                value = string.IsNullOrEmpty(value) ? string.Empty : Uri.UnescapeDataString(value);

                // add name / value pair to the collection
                QueryString(name, value);

                // trailing '&'

                //if (i == l-1 && s[i] == '&')
                //    base.Add(null, String.Empty);

                i++;
            }
        }
    }
}