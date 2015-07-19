using System;
using System.Linq;

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
        /// Initializes a new instance of the <see cref="RequestBuilder{TBuilder}"/> class.
        /// </summary>
        /// <param name="request">The fluent HTTP request being built.</param>
        protected RequestBuilder(FluentRequest request)
        {
            Request = request;
        }
    }
}
