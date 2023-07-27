namespace FluentRest;

/// <summary>
/// The shared default <see cref="IContentSerializer"/>.
/// </summary>
public static class ContentSerializer
{
    /// <summary>
    /// Gets the current singleton instance of <see cref="IContentSerializer"/>.
    /// </summary>
    /// <value>The current singleton instance of <see cref="IContentSerializer"/>.</value>
    public static IContentSerializer Current { get; set; } = JsonContentSerializer.Default;
}
