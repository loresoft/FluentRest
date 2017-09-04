using System;

namespace FluentRest
{
    /// <summary>
    /// A fluent form post builder.
    /// </summary>
    public sealed class FormBuilder : PostBuilder<FormBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormBuilder"/> class.
        /// </summary>
        /// <param name="request">The fluent HTTP request being built.</param>
        public FormBuilder(FluentRequest request) : base(request)
        {
        }
    }
}