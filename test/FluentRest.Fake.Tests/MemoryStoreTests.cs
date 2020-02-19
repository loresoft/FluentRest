using System;
using System.Net;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FluentRest.Fake.Tests
{
    public class MemoryStoreTests
    {
        public IServiceProvider ServiceProvider { get; }

        public MemoryStoreTests()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IContentSerializer, JsonContentSerializer>();
            services.AddSingleton<IFakeMessageStore>(s => MemoryMessageStore.Current);

            services
                .AddHttpClient<EchoClient>(c => c.BaseAddress = new Uri("http://httpbin.org/"))
                .AddHttpMessageHandler(s => new FakeMessageHandler(s.GetService<IFakeMessageStore>(), FakeResponseMode.Fake));

            ServiceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async void PostTest()
        {
            var response = new EchoResult();
            response.Url = "http://httpbin.org/post?page=10";
            response.Headers["Accept"] = "application/json";
            response.QueryString["page"] = "10";
            response.Form["Test"] = "Fake";
            response.Form["key"] = "value";


            MemoryMessageStore.Current.Register(b => b
                .Url("http://httpbin.org/post?page=10")
                .StatusCode(HttpStatusCode.OK)
                .ReasonPhrase("OK")
                .Content(c => c
                    .Header("Content-Type", "application/json; charset=utf-8")
                    .Data(response)
                )
            );


            var client = ServiceProvider.GetService<EchoClient>();

            var result = await client.PostAsync<EchoResult>(b => b
                .AppendPath("post")
                .FormValue("Test", "Fake")
                .FormValue("key", "value")
                .QueryString("page", 10)
            ).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal("http://httpbin.org/post?page=10", result.Url);
            Assert.Equal("Fake", result.Form["Test"]);
            Assert.Equal("value", result.Form["key"]);
        }

        [Fact]
        public async void PostWithStringContentTest()
        {
            string json = "{ \"url\": \"http://httpbin.org/post\", \"data\": \"test\" }";

            MemoryMessageStore.Current.Register(b => b
                .Url("http://httpbin.org/post")
                .StatusCode(HttpStatusCode.OK)
                .ReasonPhrase("OK")
                .Content(c => c
                    .Header("Content-Type", "application/json; charset=utf-8")
                    .Data(json)
                )
            );

            var client = ServiceProvider.GetService<EchoClient>();
            var result = await client.PostAsync<EchoResult>(b => b
                .AppendPath("post")
            ).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal("http://httpbin.org/post", result.Url);
        }

        [Fact]
        public async void PostWithByteArrayContentTest()
        {
            string json = "{ \"url\": \"http://httpbin.org/post\", \"data\": \"test\" }";
            var data = Encoding.UTF8.GetBytes(json);

            MemoryMessageStore.Current.Register(b => b
                .Url("http://httpbin.org/post")
                .StatusCode(HttpStatusCode.OK)
                .ReasonPhrase("OK")
                .Content(c => c
                    .Header("Content-Type", "application/json; charset=utf-8")
                    .Data(data)
                )
            );

            var client = ServiceProvider.GetService<EchoClient>();
            var result = await client.PostAsync<EchoResult>(b => b
                .AppendPath("post")
            ).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal("http://httpbin.org/post", result.Url);
        }
    }
}
