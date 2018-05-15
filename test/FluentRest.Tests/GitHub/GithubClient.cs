using System;
using System.Net.Http;

namespace FluentRest.Tests.GitHub
{
    public class GithubClient : FluentClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentClient" /> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="contentSerializer">The content serializer.</param>
        public GithubClient(HttpClient httpClient, IContentSerializer contentSerializer) : base(httpClient, contentSerializer)
        {
        }
    }
}