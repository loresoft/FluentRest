using System;
using System.Net.Http;
using FluentRest.Tests.Google.Maps.Models;
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
                .AppendPath("geocode")
                .AppendPath("json")
                .QueryString("address", "1600 Amphitheatre Parkway, Mountain View, CA")
            );

            Assert.NotNull(result);
            Assert.Equal("OK", result.Status);
            Assert.Single(result.Results);

        }



        private static IFluentClient CreateClient()
        {
            var serializer = new JsonContentSerializer();

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/", UriKind.Absolute);

            var fluentClient = new FluentClient(httpClient, serializer);
            return fluentClient;
        }
    }


}
