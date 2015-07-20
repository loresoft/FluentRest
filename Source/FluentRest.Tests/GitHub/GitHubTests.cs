using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentRest.Fake;
using FluentRest.Tests.GitHub.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace FluentRest.Tests.GitHub
{
    public class GitHubTests
    {
        [Fact]
        public async void GetRepo()
        {
            var client = CreateClient();
            var result = await client.GetAsync<Repository>(b => b
                .AppendPath("repos")
                .AppendPath("loresoft")
                .AppendPath("FluentRest")
                .Header(h => h.Authorization("token", "7ca62d97436f382253c6b9648d40b4b59630b778"))
            );

            Assert.NotNull(result);
            Assert.Equal("FluentRest", result.Name);
        }

        [Fact]
        public async void GetRepoIssues()
        {
            var client = CreateClient();
            var result = await client.GetAsync<List<Issue>>(b => b
                .AppendPath("repos")
                .AppendPath("loresoft")
                .AppendPath("FluentRest")
                .AppendPath("issues")
                .Header(h => h.Authorization("token", "7ca62d97436f382253c6b9648d40b4b59630b778"))
            );

            Assert.NotNull(result);
        }

        [Fact]
        public async void GetFirstIssue()
        {
            var client = CreateClient();
            var result = await client.GetAsync<Issue>(b => b
                .AppendPath("repos")
                .AppendPath("loresoft")
                .AppendPath("FluentRest")
                .AppendPath("issues")
                .AppendPath("1")
                .Header(h => h.Authorization("token", "7ca62d97436f382253c6b9648d40b4b59630b778"))
            );

            Assert.NotNull(result);
        }

        private static FluentClient CreateClient()
        {
            var serializer = new JsonContentSerializer();

            var fakeHttp = new FakeMessageHandler();
            fakeHttp.Mode = FakeResponseMode.Fake;
            fakeHttp.StorePath = @".\GitHub\Responses";

            var client = new FluentClient(serializer, fakeHttp);
            client.BaseUri = new Uri("https://api.github.com/", UriKind.Absolute);

            return client;
        }

    }

}
