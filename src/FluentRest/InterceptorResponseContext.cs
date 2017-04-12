using System;
using System.Net.Http;

namespace FluentRest
{
    /// <summary>
    /// The context for the response interceptor.
    /// </summary>
    public class InterceptorResponseContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptorResponseContext" /> class.
        /// </summary>
        /// <param name="client">The current <see cref="FluentClient" /> that called the interceptor.</param>
        /// <param name="httpResponse">The <see cref="HttpResponseMessage" /> received from the HTTP call.</param>
        /// <param name="exception">The exception that occurred during the HTTP request.</param>
        public InterceptorResponseContext(FluentClient client, HttpResponseMessage httpResponse, Exception exception)
        {
            Client = client;
            HttpResponse = httpResponse;
            Exception = exception;
        }

        /// <summary>
        /// Gets the current <see cref="FluentClient"/> that called the interceptor.
        /// </summary>
        /// <value>
        /// The current <see cref="FluentClient"/> that called the interceptor.
        /// </value>
        public FluentClient Client { get; }

        /// <summary>
        /// Gets or sets the <see cref="FluentResponse"/> transformed from the <see cref="HttpResponse"/> received.
        /// Changes to the response instance will be returned to the active <see cref="FluentClient"/> call.
        /// </summary>
        /// <value>
        /// The <see cref="FluentResponse"/> transformed from the <see cref="HttpResponse"/>.
        /// </value>
        public FluentResponse Response { get; set; }

        /// <summary>
        /// Gets the <see cref="HttpResponseMessage"/> received from the HTTP request.
        /// </summary>
        /// <value>
        /// The <see cref="HttpResponseMessage"/> received from the HTTP request.
        /// </value>
        public HttpResponseMessage HttpResponse { get; }

        /// <summary>
        /// Gets the exception that occurred during the HTTP request.
        /// </summary>
        /// <value>
        /// The exception the occurred during the HTTP request.
        /// </value>
        public Exception Exception { get; }
    }
}