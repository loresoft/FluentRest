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
        /// Initializes a new instance of the <see cref="InterceptorResponseContext"/> class.
        /// </summary>
        /// <param name="client">The current <see cref="FluentClient"/> that called the interceptor.</param>
        /// <param name="httpResponse">The <see cref="HttpResponseMessage"/> received from the HTTP call.</param>
        public InterceptorResponseContext(FluentClient client, HttpResponseMessage httpResponse)
        {
            Client = client;
            HttpResponse = httpResponse;
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
        /// Changes to the resonse instance will be returned to the acive <see cref="FluentClient"/> call.
        /// </summary>
        /// <value>
        /// The <see cref="FluentResponse"/> transformed from the <see cref="HttpResponse"/>.
        /// </value>
        public FluentResponse Response { get; set; }

        /// <summary>
        /// Gets the <see cref="HttpResponseMessage"/> received from the HTTP call.
        /// </summary>
        /// <value>
        /// The <see cref="HttpResponseMessage"/> received from the HTTP call.
        /// </value>
        public HttpResponseMessage HttpResponse { get; }
    }
}