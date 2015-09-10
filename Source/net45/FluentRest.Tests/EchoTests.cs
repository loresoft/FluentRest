using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace FluentRest.Tests
{
    public class EchoTests
    {
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
        public async void EchoGetBearer()
        {
            var client = CreateClient();

            var result = await client.GetAsync<EchoResult>(b => b
                .AppendPath("get")
                .QueryString("page", 1)
                .QueryString("size", 10)
                .BearerToken("abcdef")
            );

            Assert.NotNull(result);
            Assert.Equal("http://httpbin.org/get?page=1&size=10", result.Url);
            Assert.Equal("1", result.QueryString["page"]);
            Assert.Equal("10", result.QueryString["size"]);

            Assert.Equal("Bearer abcdef", result.Headers["Authorization"]);
        }

        [Fact]
        public void EchoGetCancellation()
        {
            var client = CreateClient();

            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(1));

            var task = client.GetAsync<EchoResult>(b => b
                .AppendPath("delay")
                .AppendPath("30")
                .QueryString("page", 1)
                .QueryString("size", 10)
                .CancellationToken(tokenSource.Token)
            );

            Assert.Throws<OperationCanceledException>(() => task.Wait(tokenSource.Token));
        }

        [Fact]
        public async void EchoGetAcceptMultiple()
        {
            var client = CreateClient();

            var result = await client.GetAsync<EchoResult>(b => b
                .AppendPath("get")
                .QueryString("page", 10)
                .Header(h => h
                    .Accept("text/xml")
                    .Accept("application/bson")
                )
                .Header("x-blah", "testing header")
            );

            Assert.NotNull(result);
            Assert.Equal("http://httpbin.org/get?page=10", result.Url);
            Assert.Equal("application/json, text/xml, application/bson", result.Headers[HttpRequestHeaders.Accept]);
            Assert.Equal("testing header", result.Headers["x-blah"]);

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
        public async void EchoPut()
        {
            var client = CreateClient();

            var result = await client.PutAsync<EchoResult>(b => b
                .AppendPath("put")
                .FormValue("Test", "Value")
                .FormValue("key", "value")
                .QueryString("page", 10)
            );

            Assert.NotNull(result);
            Assert.Equal("http://httpbin.org/put?page=10", result.Url);
            Assert.Equal("Value", result.Form["Test"]);
            Assert.Equal("value", result.Form["key"]);
        }

        [Fact]
        public async void EchoDelete()
        {
            var client = CreateClient();

            var result = await client.DeleteAsync<EchoResult>(b => b
                .AppendPath("delete")
                .FormValue("Test", "Value")
                .FormValue("key", "value")
            );

            Assert.NotNull(result);
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

        private static FluentClient CreateClient()
        {
            var client = new FluentClient();
            client.BaseUri = new Uri("http://httpbin.org/", UriKind.Absolute);
            return client;
        }

    }
}
