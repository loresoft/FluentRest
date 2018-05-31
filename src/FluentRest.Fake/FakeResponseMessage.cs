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
        /// Initializes a new instance of the <see cref="FakeResponseMessage"/> class.
        /// </summary>
        public FakeResponseMessage()
        {
            StatusCode = HttpStatusCode.OK;
            ReasonPhrase = "OK";
            ResponseHeaders = new Dictionary<string, IEnumerable<string>>();
            ContentHeaders = new Dictionary<string, IEnumerable<string>>();
        }

        /// <summary>
        /// Gets or sets the HTTP status code.
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