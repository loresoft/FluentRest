using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRest
{
    /// <summary>
    /// Extension methtods for requests
    /// </summary>
    public static class RequestExtensions
    {
        /// <summary>
        /// Set the bearer authorization token header. Authorization: Bearer abcdef
        /// </summary>
        /// <typeparam name="TBuilder">The type of the builder.</typeparam>
        /// <param name="builder">The builder to add header to.</param>
        /// <param name="token">The bearer authorization token.</param>
        /// <returns>A fluent request builder.</returns>
        public static TBuilder BearerToken<TBuilder>(this TBuilder builder, string token)
             where TBuilder : QueryBuilder<TBuilder>
        {
            builder.Header(h => h.Authorization("Bearer", token));

            return builder;
        }
    }
}
