using System;

namespace FluentRest
{
    /// <summary>
    /// Extension methods for requests
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
