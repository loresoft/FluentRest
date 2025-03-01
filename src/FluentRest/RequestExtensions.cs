using System.Text;

namespace FluentRest;

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
    public static TBuilder BearerToken<TBuilder>(this TBuilder builder, string? token)
         where TBuilder : QueryBuilder<TBuilder>
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        builder.Header(h => h.Authorization("Bearer", token));

        return builder;
    }

    /// <summary>
    /// Set basic authorization header from the specified username and password. Authorization: Basic abcdef
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder.</typeparam>
    /// <param name="builder">The builder to add header to.</param>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <returns>A fluent request builder.</returns>
    public static TBuilder BasicAuthorization<TBuilder>(this TBuilder builder, string username, string password)
        where TBuilder : QueryBuilder<TBuilder>
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));
        if (string.IsNullOrEmpty(username))
            throw new ArgumentException($"'{nameof(username)}' cannot be null or empty.", nameof(username));
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));

        string value = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
        builder.Header(h => h.Authorization("Basic", value));

        return builder;
    }
}
