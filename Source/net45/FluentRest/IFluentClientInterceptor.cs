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
        Task RequestAsync(InterceptorRequestContext requestContext);

        /// <summary>
        /// Allow for modification of the current response before returning the response.
        /// </summary>
        Task ResponseAsync(InterceptorResponseContext responseContext);
    }
}
