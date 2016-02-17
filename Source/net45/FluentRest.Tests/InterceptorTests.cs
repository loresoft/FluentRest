using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace FluentRest.Tests
{
    public class InterceptorTests
    {
        private readonly ITestOutputHelper _output;

        public InterceptorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async void EchoGet()
        {
            var client = CreateClient();

            var result = await client.GetAsync<EchoResult>(b => b
                .AppendPath("get")
                .QueryString("page", 1)
                .QueryString("size", 10)
            );

            Assert.NotNull(result);
            Assert.Equal("http://httpbin.org/get?page=1&size=10", result.Url);
            Assert.Equal("1", result.QueryString["page"]);
            Assert.Equal("10", result.QueryString["size"]);

        }

        [Fact]
        public async void EchoPost()
        {
            var client = CreateClient();

            var result = await client.PostAsync<EchoResult>(b => b
                .AppendPath("post")
                .FormValue("Test", "Value")
                .FormValue("key", "value")
                .QueryString("page", 10)
            );

            Assert.NotNull(result);
            Assert.Equal("http://httpbin.org/post?page=10", result.Url);
            Assert.Equal("Value", result.Form["Test"]);
            Assert.Equal("value", result.Form["key"]);
        }

        [Fact]
        public async void EchoPostData()
        {
            var user = UserData.Create();
            var client = CreateClient();

            var result = await client.PostAsync<EchoResult>(b => b
                .AppendPath("post")
                .QueryString("page", 10)
                .Content(user)
            );

            Assert.NotNull(result);
            Assert.Equal("http://httpbin.org/post?page=10", result.Url);
            Assert.Equal("application/json; charset=utf-8", result.Headers[HttpRequestHeaders.ContentType]);

            dynamic data = result.Json;
            Assert.Equal(user.Id, (long)data.Id);
            Assert.Equal(user.FirstName, (string)data.FirstName);
        }


        private FluentClient CreateClient()
        {
            var client = FluentClient.Create(c => c
                .Interceptor(new LogInterceptor(_output.WriteLine))
                .BaseUri(new Uri("http://httpbin.org/", UriKind.Absolute))
            );

            return client;
        }

    }
}