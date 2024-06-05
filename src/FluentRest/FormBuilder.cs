namespace FluentRest;

/// <summary>
/// A fluent form post builder.
/// </summary>
public class FormBuilder : PostBuilder<FormBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormBuilder"/> class.
    /// </summary>
    /// <param name="requestMessage">The fluent HTTP request being built.</param>
    public FormBuilder(HttpRequestMessage requestMessage) : base(requestMessage)
    {
    }
}
