using System;
using System.Net.Http;

namespace FluentRest
{
    /// <summary>
    /// An <see langword="interface"/> for transforming HTTP requests and response
    /// </summary>
    public interface IFluentInterceptor
    {
        /// <summary>
        /// Transforms the <see cref="FluentRequest"/> to <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="fluentRequest">The fluent request.</param>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>The <see cref="HttpRequestMessage"/> to be sent.</returns>
        HttpRequestMessage TransformRequest(FluentRequest fluentRequest, HttpRequestMessage httpRequest);

        /// <summary>
        /// Transforms the <see cref="HttpResponseMessage"/> to <see cref="FluentResponse"/>.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="fluentResponse">The fluent response.</param>
        /// <returns>The FluentResponse from the the HTTP call.</returns>
        FluentResponse TransformResponse(HttpResponseMessage httpResponse, FluentResponse fluentResponse);
    }
}