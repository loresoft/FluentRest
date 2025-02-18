namespace FluentRest;

/// <summary>
/// A base class for exceptions thrown based on problem detail response
/// </summary>
#pragma warning disable RCS1194 // Implement exception constructors
public class ProblemException : HttpRequestException
#pragma warning restore RCS1194 // Implement exception constructors
{
    /// <summary>
    /// Initializes a new instance of the ProblemException class.
    /// </summary>
    /// <param name="problemDetails">The problem detail information</param>
    /// <exception cref="ArgumentNullException">when <paramref name="problemDetails"/> is null</exception>
    public ProblemException(ProblemDetails problemDetails) : base(problemDetails.Title)
    {
        ProblemDetails = problemDetails ?? throw new ArgumentNullException(nameof(problemDetails));
    }


    /// <summary>
    /// Initializes a new instance of the ProblemException class.
    /// </summary>
    /// <param name="problemDetails">The problem detail information</param>
    /// <param name="innerException">The inner exception</param>
    /// <exception cref="ArgumentNullException">when <paramref name="problemDetails"/> is null</exception>
    public ProblemException(ProblemDetails problemDetails, Exception innerException) : base(problemDetails.Title, innerException)
    {
        ProblemDetails = problemDetails ?? throw new ArgumentNullException(nameof(problemDetails));
    }

    /// <summary>
    /// Gets the problem details for this exception
    /// </summary>
    public ProblemDetails ProblemDetails { get; }
}
