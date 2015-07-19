using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentRest.Fake;
using Newtonsoft.Json.Linq;
using Xunit;

namespace FluentRest.Tests
{
    public class GoogleTests
    {
        [Fact]
        public async void GeocodeTest()
        {
            var client = CreateClient();
            var result = await client.GetAsync<JObject>(b => b
                .BaseUri("https://maps.googleapis.com/maps/api/")
                .AppendPath("geocode")
                .AppendPath("json")
                .QueryString("address", "1600 Amphitheatre Parkway, Mountain View, CA")
            );

            Assert.NotNull(result);
        }



        private static FluentClient CreateClient()
        {
            var serializer = new JsonContentSerializer();
            var fakeHttp = new FakeMessageHandler { Mode = FakeResponseMode.Capture };

            var client = new FluentClient(serializer, fakeHttp);
            return client;
        }
    }
}
