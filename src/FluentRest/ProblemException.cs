namespace FluentRest;

/// <summary>
/// A base class for exceptions thrown based on problem detail respose
/// </summary>
public class ProblemException : HttpRequestException
{
    /// <summary>
    /// Initializes a new instance of the ProblemException class.
    /// </summary>
    /// <param name="problemDetails">The proble detail information</param>
    /// <exception cref="ArgumentNullException">when <paramref name="problemDetails"/> is null</exception>
    public ProblemException(ProblemDetails problemDetails) : base(problemDetails.Title)
    {
        ProblemDetails = problemDetails ?? throw new ArgumentNullException(nameof(problemDetails));
    }


    /// <summary>
    /// Initializes a new instance of the ProblemException class.
    /// </summary>
    /// <param name="problemDetails">The proble detail information</param>
    /// <param name="innerException">The inner exception</param>
    /// <exception cref="ArgumentNullException">when <paramref name="problemDetails"/> is null</exception>
    public ProblemException(ProblemDetails problemDetails, Exception innerException) : base(problemDetails.Title, innerException)
    {
        ProblemDetails = problemDetails ?? throw new ArgumentNullException(nameof(problemDetails));
    }

    /// <summary>
    /// Gets the problem details for this execption
    /// </summary>
    public ProblemDetails ProblemDetails { get; }
}
