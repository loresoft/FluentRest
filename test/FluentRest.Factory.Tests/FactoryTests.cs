using System;
using System.Collections.Generic;
using System.Net.Http;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using Xunit;

namespace FluentRest.Tests;

public class FactoryTests
{
    [Fact]
    public void TypedClient()
    {
        var services = new ServiceCollection();

        services.AddFluentClient();
        services.AddHttpClient<SampleClient>(c =>
            {
                c.BaseAddress = new Uri("https://sample.com/");
            })
            .SetSerializer<MyContentSerializer>();

        var serviceProvider = services.BuildServiceProvider();

        var typedClient = serviceProvider.GetService<SampleClient>();
        Assert.Equal(typeof(MyContentSerializer), typedClient.ContentSerializer.GetType());
        Assert.Equal(new Uri("https://sample.com/"), typedClient.HttpClient.BaseAddress);
        Assert.IsAssignableFrom<IFluentClient>(typedClient);

        var fluentClientFactory = serviceProvider.GetService<IFluentClientFactory>();
        var namedClient = fluentClientFactory.CreateClient(typeof(SampleClient).Name);
        Assert.Equal(typeof(MyContentSerializer), namedClient.ContentSerializer.GetType());
        Assert.Equal(new Uri("https://sample.com/"), typedClient.HttpClient.BaseAddress);
        Assert.IsAssignableFrom<IFluentClient>(namedClient);
    }

    [Fact]
    public void NamedClient()
    {
        var services = new ServiceCollection();

        services.AddFluentClient();
        services.AddHttpClient("sample", c =>
            {
                c.BaseAddress = new Uri("https://sample.com/");
            })
            .SetSerializer<MyContentSerializer>();

        var serviceProvider = services.BuildServiceProvider();

        var clientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var sampleClient = clientFactory.CreateClient("sample");
        Assert.NotNull(sampleClient);

        var fluentClientFactory = serviceProvider.GetService<IFluentClientFactory>();
        var fluentSampleClient = fluentClientFactory.CreateClient("sample");
        Assert.Equal(typeof(MyContentSerializer), fluentSampleClient.ContentSerializer.GetType());
        Assert.Equal(new Uri("https://sample.com/"), fluentSampleClient.HttpClient.BaseAddress);
        Assert.IsAssignableFrom<IFluentClient>(fluentSampleClient);
    }

    [Fact]
    public void NotConfiguredClient()
    {
        var services = new ServiceCollection();

        services.AddFluentClient();

        var serviceProvider = services.BuildServiceProvider();

        var clientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var sampleClient = clientFactory.CreateClient("sample");
        Assert.NotNull(sampleClient);

        var fluentClientFactory = serviceProvider.GetService<IFluentClientFactory>();
        var fluentSampleClient = fluentClientFactory.CreateClient("sample");
        Assert.Equal(typeof(JsonContentSerializer), fluentSampleClient.ContentSerializer.GetType());
        Assert.IsAssignableFrom<IFluentClient>(fluentSampleClient);
    }

    [Fact]
    public void DefaultClient()
    {
        var services = new ServiceCollection();

        services.AddFluentClient();
        services.AddHttpClient(Options.DefaultName, c =>
        {
            c.BaseAddress = new Uri("https://sample.com/");
        })
            .SetSerializer<MyContentSerializer>();

        var serviceProvider = services.BuildServiceProvider();

        var clientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var sampleClient = clientFactory.CreateClient();
        Assert.NotNull(sampleClient);
        Assert.Equal(new Uri("https://sample.com/"), sampleClient.BaseAddress);

        var fluentClientFactory = serviceProvider.GetService<IFluentClientFactory>();
        var fluentSampleClient = fluentClientFactory.CreateClient();
        Assert.Equal(typeof(MyContentSerializer), fluentSampleClient.ContentSerializer.GetType());
        Assert.Equal(new Uri("https://sample.com/"), fluentSampleClient.HttpClient.BaseAddress);
        Assert.IsAssignableFrom<IFluentClient>(fluentSampleClient);
    }

    [Fact]
    public void ServicesSerializer()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IContentSerializer, ServicesContentSerializer>();
        services.AddFluentClient();

        var serviceProvider = services.BuildServiceProvider();

        var clientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var sampleClient = clientFactory.CreateClient("sample");
        Assert.NotNull(sampleClient);

        var fluentClientFactory = serviceProvider.GetService<IFluentClientFactory>();
        var fluentSampleClient = fluentClientFactory.CreateClient("sample");
        Assert.Equal(typeof(ServicesContentSerializer), fluentSampleClient.ContentSerializer.GetType());
        Assert.IsAssignableFrom<IFluentClient>(fluentSampleClient);
    }

    [Fact]
    public void StaticSerializer()
    {
        ContentSerializer.Current = new StaticContentSerializer();
        var services = new ServiceCollection();

        services.AddFluentClient();

        var serviceProvider = services.BuildServiceProvider();

        var clientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var sampleClient = clientFactory.CreateClient("sample");
        Assert.NotNull(sampleClient);

        var fluentClientFactory = serviceProvider.GetService<IFluentClientFactory>();
        var fluentSampleClient = fluentClientFactory.CreateClient("sample");
        Assert.Equal(typeof(StaticContentSerializer), fluentSampleClient.ContentSerializer.GetType());
        Assert.IsAssignableFrom<IFluentClient>(fluentSampleClient);
    }
}

public class SampleClient : FluentClient
{
    public SampleClient(HttpClient httpClient, IContentSerializer serializer) : base(httpClient, serializer) { }
}

public class MyContentSerializer : JsonContentSerializer { }
public class ServicesContentSerializer : JsonContentSerializer { }
public class StaticContentSerializer : JsonContentSerializer { }
