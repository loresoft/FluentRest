using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                .AppendPath("Project")
                .AppendPath("123")
                .QueryString("page", 1)
                .QueryString("size", 10)
            );

            Assert.NotNull(result);
            Assert.Equal("GET", result.Method);
            Assert.Equal("/Project/123?page=1&size=10", result.RequestUrl);
        }


        [Fact]
        public async void EchoGetAcceptMultiple()
        {
            var client = CreateClient();

            var result = await client.GetAsync<EchoResult>(b => b
                .AppendPath("Project")
                .AppendPath("123")
                .QueryString("page", 10)
                .Header(h => h
                    .Accept("text/xml")
                    .Accept("application/bson")
                )
                .Header("x-blah", "testing header")
            );

            Assert.NotNull(result);
            Assert.Equal("GET", result.Method);
            Assert.Equal("/Project/123?page=10", result.RequestUrl);
            Assert.Equal("application/json, text/xml, application/bson", result.Headers[HttpRequestHeaders.Accept]);

        }

        [Fact]
        public async void EchoPost()
        {
            var client = CreateClient();

            var result = await client.PostAsync<EchoResult>(b => b
                .AppendPath("Project")
                .AppendPath("123")
                .FormValue("Test", "Value")
                .FormValue("key", "value")
                .QueryString("page", 10)
            );

            Assert.NotNull(result);
            Assert.Equal("POST", result.Method);
            Assert.Equal("/Project/123?page=10", result.RequestUrl);
        }

        [Fact]
        public async void EchoPut()
        {
            var client = CreateClient();

            var result = await client.PutAsync<EchoResult>(b => b
                .AppendPath("Project")
                .AppendPath("123")
                .FormValue("Test", "Value")
                .FormValue("key", "value")
                .QueryString("page", 10)
            );

            Assert.NotNull(result);
            Assert.Equal("PUT", result.Method);
            Assert.Equal("/Project/123?page=10", result.RequestUrl);
        }
        
        [Fact]
        public async void EchoDelete()
        {
            var client = CreateClient();

            var result = await client.DeleteAsync<EchoResult>(b => b
                .AppendPath("Project")
                .AppendPath("123")
                .FormValue("Test", "Value")
                .FormValue("key", "value")
            );

            Assert.NotNull(result);
            Assert.Equal("DELETE", result.Method);
            Assert.Equal("Test=Value&key=value", result.BodyContent);
        }
        
        [Fact]
        public async void EchoPostData()
        {
            var user = UserData.Create();
            var client = CreateClient();

            var result = await client.PostAsync<EchoResult>(b => b
                .AppendPath("Project")
                .AppendPath("123")
                .QueryString("page", 10)
                .Content(user)
            );

            Assert.NotNull(result);
            Assert.Equal("POST", result.Method);
            Assert.Equal("/Project/123?page=10", result.RequestUrl);
            Assert.Equal("application/json; charset=utf-8", result.Headers[HttpRequestHeaders.ContentType]);
        }

        private static FluentClient CreateClient()
        {
            var client = new FluentClient();
            client.BaseUri = new Uri("http://echo.jpillora.com/", UriKind.Absolute);
            return client;
        }

    }
}
