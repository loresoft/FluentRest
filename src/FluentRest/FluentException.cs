using System;

namespace FluentRest
{
    /// <summary>
    /// A fluent client exception.
    /// </summary>
    public class FluentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FluentException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public FluentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}