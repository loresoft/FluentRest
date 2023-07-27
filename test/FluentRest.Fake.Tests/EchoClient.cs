using System;
using System.Net.Http;

namespace FluentRest.Fake.Tests;

public class EchoClient : FluentClient
{
    public EchoClient(HttpClient httpClient, IContentSerializer contentSerializer) : base(httpClient, contentSerializer)
    {
    }
}
