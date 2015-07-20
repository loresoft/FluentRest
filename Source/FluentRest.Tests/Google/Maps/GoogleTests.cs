using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentRest.Fake;
using FluentRest.Tests.Google.Maps.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace FluentRest.Tests
{
    public class GoogleMapsTests
    {
        [Fact]
        public async void GeocodeTest()
        {
            var client = CreateClient();
            var result = await client.GetAsync<GeocodeResponse>(b => b
                .BaseUri("https://maps.googleapis.com/maps/api/")
                .AppendPath("geocode")
                .AppendPath("json")
                .QueryString("address", "1600 Amphitheatre Parkway, Mountain View, CA")
            );

            Assert.NotNull(result);
            Assert.Equal("OK", result.Status);
            Assert.Equal(1, result.Results.Length);

            // reload captured fake response
            var fakeClient = CreateClient(FakeResponseMode.Fake);
            var fakeResult = await fakeClient.GetAsync<GeocodeResponse>(b => b
                .BaseUri("https://maps.googleapis.com/maps/api/")
                .AppendPath("geocode")
                .AppendPath("json")
                .QueryString("address", "1600 Amphitheatre Parkway, Mountain View, CA")
            );

            Assert.NotNull(fakeResult);
            Assert.Equal("OK", fakeResult.Status);
            Assert.Equal(1, fakeResult.Results.Length);

        }



        private static FluentClient CreateClient(FakeResponseMode mode = FakeResponseMode.Capture)
        {
            var serializer = new JsonContentSerializer();
            var fakeHttp = new FakeMessageHandler { Mode = mode };

            var client = new FluentClient(serializer, fakeHttp);
            return client;
        }
    }


}
