using System;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;

using FluentRest.Tests.GitHub;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using Xunit;

using XUnit.Hosting;

using IContainer = DotNet.Testcontainers.Containers.IContainer;

namespace FluentRest.Tests;

public class HostFixture : TestApplicationFixture, IAsyncLifetime
{
    private readonly IContainer _container = new ContainerBuilder()
        .WithImage("kennethreitz/httpbin:latest")
        .WithPortBinding(80, true)
        .Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        // wait for startup
        await Task.Delay(5000);

        // get container url
        HttpBinUrl = $"http://{_container.Hostname}:{_container.GetMappedPublicPort(80)}";
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    public string HttpBinUrl { get; private set; } = "https://httpbin.org/";

    protected override void ConfigureApplication(HostApplicationBuilder builder)
    {
        base.ConfigureApplication(builder);

        builder.Services
            .TryAddSingleton<IContentSerializer, JsonContentSerializer>();

        builder.Services
            .AddHttpClient<GithubClient>(c =>
            {
                c.BaseAddress = new Uri("https://api.github.com/");

                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                c.DefaultRequestHeaders.Add("User-Agent", "GitHubClient");
            })
            .AddHttpMessageHandler(() => new RetryHandler());

        builder.Services
            .AddHttpClient("GoogleMaps", client => client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/", UriKind.Absolute));

        builder.Services
            .AddHttpClient("HttpBin", client => client.BaseAddress = new Uri(HttpBinUrl, UriKind.Absolute));
    }
}
