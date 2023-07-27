using System;
using System.IO;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace FluentRest.Fake.Tests;

public class FileMessageStoreTests
{
    public IServiceProvider ServiceProvider { get; }

    public FileMessageStoreTests()
    {
        var storePath = Path.Combine(AppContext.BaseDirectory, "Data");
        if (!Directory.Exists(storePath))
            Directory.CreateDirectory(storePath);

        var services = new ServiceCollection();

        services.AddSingleton<IContentSerializer, JsonContentSerializer>();
        services.AddSingleton<IFakeMessageStore>(s => new FileMessageStore(storePath));

        services
            .AddHttpClient<EchoClient>(c => c.BaseAddress = new Uri("http://httpbin.org/"))
            .AddHttpMessageHandler(s => new FakeMessageHandler(s.GetService<IFakeMessageStore>(), FakeResponseMode.Fake));

        ServiceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async void Register()
    {
        var client = ServiceProvider.GetService<EchoClient>();

        var result = await client.PostAsync<EchoResult>(b => b
            .AppendPath("post")
            .FormValue("Test", "Fake")
            .FormValue("key", "value")
            .QueryString("page", 10)
        ).ConfigureAwait(false);

        Assert.NotNull(result);
        Assert.Equal("http://httpbin.org/post?page=10", result.Url);
        Assert.Equal("Fake", result.Form["Test"]);
        Assert.Equal("value", result.Form["key"]);
    }

}
