using System;
using System.Net.Http;

namespace FluentRest
{
    /// <summary>
    /// The context for the request interceptor.
    /// </summary>
    public class InterceptorRequestContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptorRequestContext"/> class.
        /// </summary>
        /// <param name="client">The current <see cref="FluentClient"/>.</param>
        /// <param name="request">The <see cref="FluentRequest"/> used to build the current request.</param>
        public InterceptorRequestContext(FluentClient client, FluentRequest request)
        {
            Client = client;
            Request = request;
        }


        /// <summary>
        /// Gets the current <see cref="FluentClient"/> that called the interceptor.
        /// </summary>
        /// <value>
        /// The current <see cref="FluentClient"/> that called the interceptor.
        /// </value>
        public FluentClient Client { get; }

        /// <summary>
        /// Gets the <see cref="FluentRequest"/> used to build the current request. 
        /// Any change to <see cref="FluentRequest"/> will not be apply to the <see cref="HttpRequest"/> instance.
        /// </summary>
        /// <value>
        /// The <see cref="FluentRequest"/> used to build the current request.
        /// </value>
        public FluentRequest Request { get; }

        /// <summary>
        /// Gets or sets the <see cref="HttpRequestMessage"/> transformed from <see cref="Request"/> to send. 
        /// Any change to HttpRequest will be sent.
        /// </summary>
        /// <value>
        /// The <see cref="HttpRequestMessage"/> to send.
        /// </value>
        public HttpRequestMessage HttpRequest { get; set; }
    }
}