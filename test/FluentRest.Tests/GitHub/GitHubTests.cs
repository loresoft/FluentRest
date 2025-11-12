using System.Collections.Generic;
using System.Threading.Tasks;

using FluentRest.Tests.GitHub.Models;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace FluentRest.Tests.GitHub;

public class GitHubTests(HostFixture fixture) : HostTestBase(fixture)
{

    [Fact]
    public async Task GetRepo()
    {
        var client = Services.GetService<GithubClient>();
        var result = await client.GetAsync<Repository>(b => b
            .AppendPath("repos")
            .AppendPath("loresoft")
            .AppendPath("FluentRest")
        );

        Assert.NotNull(result);
        Assert.Equal("FluentRest", result.Name);
    }

    [Fact]
    public async Task GetRepoIssues()
    {
        var client = Services.GetService<GithubClient>();
        var result = await client.GetAsync<List<Issue>>(b => b
            .AppendPath("repos")
            .AppendPath("loresoft")
            .AppendPath("FluentRest")
            .AppendPath("issues")
        );

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetFirstIssue()
    {
        var client = Services.GetService<GithubClient>();
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
