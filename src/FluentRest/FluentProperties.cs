using System;

namespace FluentRest
{
    /// <summary>
    /// Fluent property name constants
    /// </summary>
    public static class FluentProperties
    {
        /// <summary>
        /// The request URL builder property name
        /// </summary>
        public const string RequestUrlBuilder = "FluentUrlBuilder";

        /// <summary>
        /// The request form data property name
        /// </summary>
        public const string RequestFormData = "FluentFormData";

        /// <summary>
        /// The request content data property name
        /// </summary>
        public const string RequestContentData = "FluentContentData";

        /// <summary>
        /// The content serializer property name
        /// </summary>
        public const string ContentSerializer = "FluentContentSerializer";

        /// <summary>
        /// The cancellation token property name
        /// </summary>
        public const string CancellationToken = "FluentCancellationToken";

        /// <summary>
        /// The HTTP completion option property name
        /// </summary>
        public const string HttpCompletionOption = "FluentCompletionOption";

    }
}