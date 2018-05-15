using System;

namespace FluentRest
{
    /// <summary>
    /// The shared default <see cref="IContentSerializer"/>.
    /// </summary>
    public class ContentSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentSerializer"/> class.
        /// </summary>
        protected ContentSerializer()
        {

        }

        /// <summary>
        /// Gets the current singleton instance of <see cref="IContentSerializer"/>.
        /// </summary>
        /// <value>The current singleton instance of <see cref="IContentSerializer"/>.</value>
        public static IContentSerializer Current { get; set; } = JsonContentSerializer.Default;
    }
}