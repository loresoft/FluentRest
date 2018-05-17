using System;
using System.Collections.Generic;
using System.Net.Http;
using FluentRest.Tests.GitHub;
using FluentRest.Tests.GitHub.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace FluentRest.Tests
{
    public class FactoryTests
    {
        public IServiceProvider ServiceProvider { get; }

        public FactoryTests()
        {
            var services = new ServiceCollection();

            // global fallback if not found in services collection and not configured
            ContentSerializer.Current = new StaticContentSerializer();

            // this will be the default serializer
            services.AddSingleton<IContentSerializer, ServicesContentSerializer>();
            
            services.AddFluentClientFactory();
            
            services.AddHttpClient<GithubClient>(c =>
                {
                    c.BaseAddress = new Uri("https://api.github.com/");

                    c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json"); // Github API versioning
                    c.DefaultRequestHeaders.Add("User-Agent", "GitHubFactoryTests"); // Github requires a user-agent
                })
                .UseSerializer<MyContentSerializer>()
                .AddHttpMessageHandler(() => new RetryHandler()); // Retry requests to github using our retry handler
            
            services.AddHttpClient("random", c =>
                {
                    c.BaseAddress = new Uri("https://random.com/");
                })
                .UseSerializer<MyContentSerializer>()
                .AddHttpMessageHandler(() => new RetryHandler()); // Retry requests to github using our retry handler
            
            services.AddHttpClient("stuff", c =>
                {
                    c.BaseAddress = new Uri("https://stuff.com/");
                });

            ServiceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void GetRepoFactoryTyped()
        {
            var client = ServiceProvider.GetService<GithubClient>();
            Assert.Equal(typeof(MyContentSerializer), client.ContentSerializer.GetType());
            Assert.Equal(new Uri("https://api.github.com/"), client.HttpClient.BaseAddress);
            Assert.IsAssignableFrom<IFluentClient>(client);

            var clientFactory = ServiceProvider.GetService<IFluentClientFactory>();
            var randomClient = clientFactory.CreateClient("random");
            Assert.Equal(typeof(MyContentSerializer), randomClient.ContentSerializer.GetType());
            Assert.Equal(new Uri("https://random.com/"), randomClient.HttpClient.BaseAddress);
            Assert.IsAssignableFrom<IFluentClient>(randomClient);

            var stuffClient = clientFactory.CreateClient("stuff");
            Assert.Equal(typeof(ServicesContentSerializer), stuffClient.ContentSerializer.GetType());
            Assert.Equal(new Uri("https://stuff.com/"), stuffClient.HttpClient.BaseAddress);
            Assert.IsAssignableFrom<IFluentClient>(stuffClient);
        }

        [Fact]
        public void GetRepoFactory()
        {
            var clientFactory = ServiceProvider.GetService<IFluentClientFactory>();
            var client = clientFactory.CreateClient(typeof(GithubClient).Name);
            Assert.Equal(typeof(MyContentSerializer), client.ContentSerializer.GetType());
            Assert.Equal(new Uri("https://api.github.com/"), client.HttpClient.BaseAddress);
            Assert.IsAssignableFrom<IFluentClient>(client);
        }
    }

    public class MyContentSerializer : JsonContentSerializer { }
    public class ServicesContentSerializer : JsonContentSerializer { }
    public class StaticContentSerializer : JsonContentSerializer { }
}