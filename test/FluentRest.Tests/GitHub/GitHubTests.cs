using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using FluentRest.Tests.GitHub.Models;
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
                //.Header(h => h.Authorization("token", "7ca..."))
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
                //.Header(h => h.Authorization("token", "7ca..."))
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
                //.Header(h => h.Authorization("token", "7ca..."))
            );

            Assert.NotNull(result);
        }

        private static FluentClient CreateClient()
        {
            var contentSerializer = new JsonContentSerializer();

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.github.com/", UriKind.Absolute);

            var fluentClient = new FluentClient(httpClient, contentSerializer);
            return fluentClient;
        }

    }

}
