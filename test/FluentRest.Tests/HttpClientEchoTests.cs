using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Xunit;

namespace FluentRest.Tests
{
    public class HttpClientEchoTests
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
        public async void EchoBasicAuthorization()
        {
            var client = CreateClient();

            var result = await client.GetAsync<EchoResult>(b => b
                .AppendPath("basic-auth/ejsmith/password")
                .BasicAuthorization("ejsmith", "password")
            );

            Assert.NotNull(result);
            Assert.Equal("true", result.Authenticated);
            Assert.Equal("ejsmith", result.User);
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
            Assert.Equal("text/xml, application/bson, application/json", result.Headers[HttpRequestHeaders.Accept]);
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
        public async void EchoPostResponse()
        {
            var client = CreateClient();

            var response = await client.PostAsync(b => b
                .AppendPath("post")
                .FormValue("Test", "Value")
                .FormValue("key", "value")
                .QueryString("page", 10)
            );

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.DeserializeAsync<EchoResult>();

            Assert.NotNull(result);
            Assert.Equal("http://httpbin.org/post?page=10", result.Url);
            Assert.Equal("Value", result.Form["Test"]);
            Assert.Equal("value", result.Form["key"]);
        }

        [Fact]
        public async void EchoPatch()
        {
            var client = CreateClient();

            var response = await client.PatchAsync<EchoResult>(b => b
                .AppendPath("patch")
                .FormValue("Test", "Value")
                .QueryString("page", 10)
            );

            Assert.NotNull(response);

            Assert.Equal("http://httpbin.org/patch?page=10", response.Url);
            Assert.Equal("Value", response.Form["Test"]);
        }

        [Fact]
        public async void EchoPatchResponse()
        {
            var client = CreateClient();

            var response = await client.PatchAsync(b => b
                .AppendPath("patch")
                .FormValue("Test", "Value")
                .QueryString("page", 10)
            );

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.DeserializeAsync<EchoResult>();

            Assert.NotNull(result);
            Assert.Equal("http://httpbin.org/patch?page=10", result.Url);
            Assert.Equal("Value", result.Form["Test"]);
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
            Assert.True(result.Headers.ContainsKey("Content-Length"));
            int contentLength = Int32.Parse(result.Headers["Content-Length"]);
            Assert.True(contentLength > 0);
            Assert.Equal("http://httpbin.org/post?page=10", result.Url);
            Assert.Equal("application/json; charset=utf-8", result.Headers[HttpRequestHeaders.ContentType]);
            Assert.True(result.Headers.ContainsKey("Content-Type"));
            var contentType = result.Headers["Content-Type"];
            Assert.Equal("application/json; charset=utf-8", contentType);

            dynamic data = result.Json;
            Assert.Equal(user.Id, (long)data.Id);
            Assert.Equal(user.FirstName, (string)data.FirstName);
        }

        [Fact]
        public async void EchoPostRawJsonContent()
        {
            var user = UserData.Create();
            var json = JsonConvert.SerializeObject(user);

            var client = CreateClient();

            var result = await client.PostAsync<EchoResult>(b => b
                .AppendPath("post")
                .Content(json, "application/json")
            );

            Assert.NotNull(result);
            Assert.Equal(json, result.Data);
            Assert.True(result.Headers.ContainsKey("Content-Type"));
            var contentType = result.Headers["Content-Type"];
            Assert.Equal("application/json; charset=utf-8", contentType);
        }

        [Fact]
        public async void EchoPostNonFluentRawJsonContent()
        {
            var user = UserData.Create();
            var json = JsonConvert.SerializeObject(user);
            var client = CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
            var builder = new SendBuilder(request).AppendPath("post").Post();

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.SendAsync(request);
            Assert.NotNull(response);

            var result = await response.Result.DeserializeAsync<EchoResult>();

            Assert.NotNull(result);
            Assert.Equal(json, result.Data);
            Assert.True(result.Headers.ContainsKey("Content-Type"));

            var contentType = result.Headers["Content-Type"];
            Assert.Equal("application/json; charset=utf-8", contentType);
        }

        [Fact]
        public async void EchoPostRawTextContent()
        {
            var client = CreateClient();

            var result = await client.PostAsync<EchoResult>(b => b
                .AppendPath("post")
                .Content("test", "text/plain", Encoding.UTF8)
            );

            Assert.NotNull(result);
            Assert.Equal("test", result.Data);
            Assert.True(result.Headers.ContainsKey("Content-Type"));
            var contentType = result.Headers["Content-Type"];
            Assert.Equal("text/plain; charset=utf-8", contentType);

            result = await client.PostAsync<EchoResult>(b => b
                .AppendPath("post")
                .Content("test", "text/plain")
            );

            Assert.NotNull(result);
            Assert.Equal("test", result.Data);
            Assert.True(result.Headers.ContainsKey("Content-Type"));
            contentType = result.Headers["Content-Type"];
            Assert.Equal("text/plain; charset=utf-8", contentType);
        }

        [Fact]
        public async void EchoPostDataCustomCompressedContent()
        {
            var user = UserData.Create();
            var data = JsonCompress(user);
            var client = CreateClient();

            var result = await client.PostAsync<EchoResult>(b => b
                .AppendPath("post")
                .QueryString("page", 10)
                .Content(data)
            );

            Assert.NotNull(result);
            Assert.True(result.Headers.ContainsKey("Content-Length"));
            int contentLength = Int32.Parse(result.Headers["Content-Length"]);
            Assert.True(contentLength > 0);
            Assert.Equal("http://httpbin.org/post?page=10", result.Url);
            Assert.Equal("application/json; charset=utf-8", result.Headers[HttpRequestHeaders.ContentType]);
            Assert.Equal("gzip", result.Headers[HttpRequestHeaders.ContentEncoding]);
        }

        private static ByteArrayContent JsonCompress(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            using (var stream = new MemoryStream())
            {
                using (var zipper = new GZipStream(stream, CompressionMode.Compress, true))
                    zipper.Write(bytes, 0, bytes.Length);

                var content = new ByteArrayContent(stream.ToArray());
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                content.Headers.ContentType.CharSet = "utf-8";
                content.Headers.ContentEncoding.Add("gzip");
                return content;
            }
        }

        [Fact]
        public async void EchoError()
        {
            var client = CreateClient();

            await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                var result = await client.PostAsync<EchoResult>(b => b
                    .AppendPath("status/500")
                    .FormValue("Test", "Value")
                    .FormValue("key", "value")
                    .QueryString("page", 10)
                );
                Assert.NotNull(result);
            });
        }

        [Fact]
        public async void DefaultPost()
        {
            var client = CreateClient();

            //client.Defaults(c => c
            //    .Header(h => h.Authorization("Token", "abc-def-123"))
            //);

            var result = await client.PostAsync<EchoResult>(b => b
                .Header(h => h.Authorization("Token", "abc-def-123"))
                .AppendPath("post")
                .FormValue("Test", "Value")
                .FormValue("key", "value")
                .QueryString("page", 10)
            );

            Assert.NotNull(result);
            Assert.Equal("http://httpbin.org/post?page=10", result.Url);
            Assert.Equal("Value", result.Form["Test"]);
            Assert.Equal("value", result.Form["key"]);
            Assert.Equal("Token abc-def-123", result.Headers["Authorization"]);
        }

        [Fact]
        public async void SendRequest()
        {
            var client = CreateClient();

            var result = await client.SendAsync<EchoResult>(b => b
                .Post()
                .AppendPath("post")
            );

            Assert.NotNull(result);
            Assert.True(result.Headers.ContainsKey("Content-Length"));
            Assert.Equal("http://httpbin.org/post", result.Url);
        }

        private static HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://httpbin.org/", UriKind.Absolute);

            return httpClient;
        }
    }
}