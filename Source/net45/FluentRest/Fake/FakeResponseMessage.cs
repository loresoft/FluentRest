using System;
using System.Collections.Generic;
using System.Net;

namespace FluentRest.Fake
{
    /// <summary>
    /// A fake response message for saving HTTP responses
    /// </summary>
    public class FakeResponseMessage
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the reason phrase.
        /// </summary>
        /// <value>
        /// The reason phrase.
        /// </value>
        public string ReasonPhrase { get; set; }

        /// <summary>
        /// Gets or sets the response headers.
        /// </summary>
        /// <value>
        /// The response headers.
        /// </value>
        public Dictionary<string, IEnumerable<string>> ResponseHeaders { get; set; }

        /// <summary>
        /// Gets or sets the content headers.
        /// </summary>
        /// <value>
        /// The content headers.
        /// </value>
        public Dictionary<string, IEnumerable<string>> ContentHeaders { get; set; }
    }
}