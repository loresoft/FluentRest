using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentRest.Fake;
using Xunit;

namespace FluentRest.Tests
{

    public class MemoryStoreTests
    {
        [Fact]
        public async void Register()
        {
            var response = new EchoResult();
            response.Url = "http://httpbin.org/post?page=10";
            response.Headers["Accept"] = "application/json";
            response.QueryString["page"] = "10";
            response.Form["Test"] = "Value";
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


        private static FluentClient CreateClient()
        {
            var serializer = new JsonContentSerializer();

            var fakeStore = MemoryMessageStore.Current;
            var fakeHttp = new FakeMessageHandler(fakeStore, FakeResponseMode.Fake);

            var client = new FluentClient(serializer, fakeHttp);
            client.BaseUri = new Uri("http://httpbin.org/", UriKind.Absolute);

            return client;
        }

    }
}
