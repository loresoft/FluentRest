using System;
using System.Collections.Generic;

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
                Request.Headers[name] = value;

            return this as TBuilder;
        }

        /// <summary>
        /// Sets HTTP header with the specified <paramref name="name"/> and <paramref name="values"/>.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="values">The header values.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public TBuilder Header(string name, IEnumerable<string> values)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            foreach (var value in values)
                Request.Headers.Add(name, value);

            return this as TBuilder;
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
        /// Appends the specified <paramref name="path"/> to the BaseUri of the request.
        /// </summary>
        /// <param name="path">The path to append.</param>
        /// <returns>A fluent request builder.</returns>
        public TBuilder AppendPath(string path)
        {
            if (path != null)
                Request.Paths.Add(path);

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
            Request.QueryString.Add(name, v);

            return this as TBuilder;

        }

        /// <summary>
        /// Appends the specified <paramref name="name"/> and <paramref name="value"/> to the request Uri.
        /// </summary>
        /// <param name="name">The query parameter name.</param>
        /// <param name="value">The query parameter value.</param>
        /// <returns>A fluent request builder.</returns>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public TBuilder QueryString(string name, object value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var v = value != null ? Convert.ToString(value) : string.Empty;
            Request.QueryString.Add(name, v);

            return this as TBuilder;
        }
    }
}