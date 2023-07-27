using System;
using System.Net.Http;

namespace FluentRest.Tests.GitHub;

public class GithubClient : FluentClient
{
    public GithubClient(HttpClient httpClient, IContentSerializer contentSerializer) : base(httpClient, contentSerializer)
    {
    }
}
