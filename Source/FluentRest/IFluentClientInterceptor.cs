using System;
using System.Threading.Tasks;

namespace FluentRest
{
    /// <summary>
    /// An <see langword="interface"/> for transforming HTTP requests and response
    /// </summary>
    public interface IFluentClientInterceptor
    {
        /// <summary>
        /// Allow for modification of the current request before send the HTTP request.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <returns>
        /// The task representing the asynchronous operation.
        /// </returns>
        Task RequestAsync(InterceptorRequestContext context);

        /// <summary>
        /// Allow for modification of the current response before returning the response.
        /// </summary>
        /// <param name="context">The current response context.</param>
        /// <returns>
        /// The task representing the asynchronous operation.
        /// </returns>
        Task ResponseAsync(InterceptorResponseContext context);
    }
}
