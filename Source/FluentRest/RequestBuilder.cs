using System;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace FluentRest
{
    /// <summary>
    /// A fluent request builder.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder.</typeparam>
    public abstract class RequestBuilder<TBuilder>
        where TBuilder : RequestBuilder<TBuilder>
    {
        /// <summary>
        /// Gets the fluent HTTP request being built.
        /// </summary>
        /// <value>
        /// The fluent HTTP request being built.
        /// </value>
        public FluentRequest Request { get; }

        /// <summary>
        /// Gets or sets the cancellation token to cancel the request operation.
        /// </summary>
        /// <value>
        /// The cancellation token to cancel the request operation.
        /// </value>
        /// <remarks>
        /// CancellationToken is stored here to keep FluentRequest serializable.
        /// </remarks>
        internal CancellationToken Token { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBuilder{TBuilder}"/> class.
        /// </summary>
        /// <param name="request">The fluent HTTP request being built.</param>
        protected RequestBuilder(FluentRequest request)
        {
            Request = request;
            Token = System.Threading.CancellationToken.None;
        }

        /// <summary>
        /// Sets the cancellation token to cancel the request operation.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A fluent request builder.</returns>
        public TBuilder CancellationToken(CancellationToken cancellationToken)
        {
            Token = cancellationToken;
            return this as TBuilder;
        }

        /// <summary>
        /// Sets when the operation should complete (as soon as a response is available or after reading the whole response content).
        /// </summary>
        /// <param name="completionOption">Hhen the operation should complete.</param>
        /// <returns>A fluent request builder.</returns>
        public TBuilder CompletionOption(HttpCompletionOption completionOption)
        {
            Request.CompletionOption = completionOption;
            return this as TBuilder;
        }
    }
}
