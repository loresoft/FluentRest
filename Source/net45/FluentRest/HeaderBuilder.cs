using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using HttpRequestHeaders = FluentRest.HttpRequestHeaders;

namespace FluentRest
{
    /// <summary>
    /// Fluent header builder
    /// </summary>
    public class HeaderBuilder : HeaderBuilder<HeaderBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderBuilder"/> class.
        /// </summary>
        /// <param name="request">The fluent HTTP request being built.</param>
        public HeaderBuilder(FluentRequest request) : base(request)
        {
        }
    }

    /// <summary>
    /// Fluent header builder
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder.</typeparam>
    public abstract class HeaderBuilder<TBuilder> : RequestBuilder<TBuilder>
            where TBuilder : HeaderBuilder<TBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderBuilder{TBuilder}"/> class.
        /// </summary>
        /// <param name="request">The fluent HTTP request being built.</param>
        protected HeaderBuilder(FluentRequest request) : base(request)
        {
        }

        /// <summary>
        /// Append the media-type to the Accept header for an HTTP request.
        /// </summary>
        /// <param name="mediaType">The media-type header value.</param>
        /// <param name="quality">The quality associated with the header value.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder Accept(string mediaType, double? quality = null)
        {
            var header = quality.HasValue
                ? new MediaTypeWithQualityHeaderValue(mediaType, quality.Value)
                : new MediaTypeWithQualityHeaderValue(mediaType);

            AppendHeader(HttpRequestHeaders.Accept, header.ToString());

            return this as TBuilder;
        }

        /// <summary>
        /// Append the value to the Accept-Charset header for an HTTP request.
        /// </summary>
        /// <param name="value">The header value.</param>
        /// <param name="quality">The quality associated with the header value.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder AcceptCharset(string value, double? quality = null)
        {
            var header = quality.HasValue
                ? new StringWithQualityHeaderValue(value, quality.Value)
                : new StringWithQualityHeaderValue(value);

            AppendHeader(HttpRequestHeaders.AcceptCharset, header.ToString());

            return this as TBuilder;
        }

        /// <summary>
        /// Append the value to the Accept-Encoding header for an HTTP request.
        /// </summary>
        /// <param name="value">The header value.</param>
        /// <param name="quality">The quality associated with the header value.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder AcceptEncoding(string value, double? quality = null)
        {
            var header = quality.HasValue
                ? new StringWithQualityHeaderValue(value, quality.Value)
                : new StringWithQualityHeaderValue(value);

            AppendHeader(HttpRequestHeaders.AcceptEncoding, header.ToString());

            return this as TBuilder;
        }

        /// <summary>
        /// Append the value to the Accept-Language header for an HTTP request.
        /// </summary>
        /// <param name="value">The header value.</param>
        /// <param name="quality">The quality associated with the header value.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder AcceptLanguage(string value, double? quality = null)
        {
            var header = quality.HasValue
                ? new StringWithQualityHeaderValue(value, quality.Value)
                : new StringWithQualityHeaderValue(value);

            AppendHeader(HttpRequestHeaders.AcceptLanguage, header.ToString());

            return this as TBuilder;
        }

        /// <summary>
        /// Sets the value of the Authorization header for an HTTP request.
        /// </summary>
        /// <param name="scheme">The scheme to use for authorization.</param>
        /// <param name="parameter">The credentials containing the authentication information.</param>
        /// <returns>A fluent header builder.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TBuilder Authorization(string scheme, string parameter = null)
        {
            if (scheme == null)
                throw new ArgumentNullException(nameof(scheme));

            var header = new AuthenticationHeaderValue(scheme, parameter);
            SetHeader(HttpRequestHeaders.Authorization, header.ToString());

            return this as TBuilder;
        }

        /// <summary>
        /// Sets the value of the Cache-Control header for an HTTP request.
        /// </summary>
        /// <param name="value">The header value.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder CacheControl(string value)
        {
            SetHeader(HttpRequestHeaders.CacheControl, value);
            return this as TBuilder;
        }

        /// <summary>
        /// Append the value of the Expect header for an HTTP request.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder Expect(string name, string value = null)
        {
            var header = new NameValueWithParametersHeaderValue(name, value);

            AppendHeader(HttpRequestHeaders.Expect, header.ToString());

            return this as TBuilder;
        }

        /// <summary>
        /// Sets the value of the From header for an HTTP request.
        /// </summary>
        /// <param name="value">The header value.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder From(string value)
        {
            SetHeader(HttpRequestHeaders.From, value);
            return this as TBuilder;
        }

        /// <summary>
        /// Sets the value of the Host header for an HTTP request.
        /// </summary>
        /// <param name="value">The header value.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder Host(string value)
        {
            SetHeader(HttpRequestHeaders.Host, value);
            return this as TBuilder;
        }

        /// <summary>
        /// Sets the value of the If-Modified-Since header for an HTTP request.
        /// </summary>
        /// <param name="modifiedDate">The modified date.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder IfModifiedSince(DateTimeOffset? modifiedDate)
        {
            string value = modifiedDate?.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture);
            SetHeader(HttpRequestHeaders.From, value);
            return this as TBuilder;
        }

        /// <summary>
        /// Sets the value of the If-Unmodified-Since header for an HTTP request.
        /// </summary>
        /// <param name="modifiedDate">The modified date.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder IfUnmodifiedSince(DateTimeOffset? modifiedDate)
        {
            string value = modifiedDate?.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture);
            SetHeader(HttpRequestHeaders.IfUnmodifiedSince, value);
            return this as TBuilder;
        }

        /// <summary>
        /// Sets the value of the Proxy-Authorization header for an HTTP request.
        /// </summary>
        /// <param name="scheme">The scheme to use for authorization.</param>
        /// <param name="parameter">The credentials containing the authentication information.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder ProxyAuthorization(string scheme, string parameter = null)
        {
            if (scheme == null)
                throw new ArgumentNullException(nameof(scheme));

            var header = new AuthenticationHeaderValue(scheme, parameter);
            SetHeader(HttpRequestHeaders.ProxyAuthorization, header.ToString());

            return this as TBuilder;
        }

        /// <summary>
        /// Sets the value of the Range header for an HTTP request.
        /// </summary>
        /// <param name="from">The position at which to start sending data.</param>
        /// <param name="to">The position at which to stop sending data.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder Range(long? from, long? to)
        {
            var header = new RangeHeaderValue(from, to);
            SetHeader(HttpRequestHeaders.Range, header.ToString());

            return this as TBuilder;
        }

        /// <summary>
        /// Sets the value of the Referer header for an HTTP request.
        /// </summary>
        /// <param name="uri">The header URI.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder Referrer(Uri uri)
        {
            var value = uri?.ToString();
            SetHeader(HttpRequestHeaders.Referer, value);
            return this as TBuilder;
        }

        /// <summary>
        /// Sets the value of the Referer header for an HTTP request.
        /// </summary>
        /// <param name="value">The header value.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder Referrer(string value)
        {
            SetHeader(HttpRequestHeaders.Referer, value);
            return this as TBuilder;
        }

        /// <summary>
        /// Sets the value of the User-Agent header for an HTTP request.
        /// </summary>
        /// <param name="value">The header value.</param>
        /// <returns>A fluent header builder.</returns>
        public TBuilder UserAgent(string value)
        {
            SetHeader(HttpRequestHeaders.UserAgent, value);
            return this as TBuilder;
        }


        private void SetHeader(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (value == null)
                Request.Headers.Remove(name);
            else
                Request.Headers[name] = new List<string>(new[] { value });
        }

        private void AppendHeader(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var list = Request.Headers.GetOrAdd(name, n => new List<string>());
            list.Add(value);
        }
    }
}