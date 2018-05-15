using System;
using System.Collections.Generic;
using FluentRest.Tests.GitHub.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FluentRest.Tests.GitHub
{
    public class GitHubFactoryTests
    {
        public IServiceProvider ServiceProvider { get; }

        public GitHubFactoryTests()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IContentSerializer, JsonContentSerializer>();

            services.AddHttpClient("github", c =>
                {
                    c.BaseAddress = new Uri("https://api.github.com/");

                    c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json"); // Github API versioning
                    c.DefaultRequestHeaders.Add("User-Agent", "GitHubFactoryTests"); // Github requires a user-agent
                })
                .AddHttpMessageHandler(() => new RetryHandler()) // Retry requests to github using our retry handler
                .AddTypedClient<GithubClient>();

            ServiceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async void GetRepo()
        {
            var client = ServiceProvider.GetService<GithubClient>();
            var result = await client.GetAsync<Repository>(b => b
                .AppendPath("repos")
                .AppendPath("loresoft")
                .AppendPath("FluentRest")
            );

            Assert.NotNull(result);
            Assert.Equal("FluentRest", result.Name);
        }

        [Fact]
        public async void GetRepoIssues()
        {
            var client = ServiceProvider.GetService<GithubClient>();
            var result = await client.GetAsync<List<Issue>>(b => b
                .AppendPath("repos")
                .AppendPath("loresoft")
                .AppendPath("FluentRest")
                .AppendPath("issues")
            );

            Assert.NotNull(result);
        }

        [Fact]
        public async void GetFirstIssue()
        {
            var client = ServiceProvider.GetService<GithubClient>();
            var result = await client.GetAsync<Issue>(b => b
                .AppendPath("repos")
                .AppendPath("loresoft")
                .AppendPath("FluentRest")
                .AppendPath("issues")
                .AppendPath("1")
            );

            Assert.NotNull(result);
        }


    }
}