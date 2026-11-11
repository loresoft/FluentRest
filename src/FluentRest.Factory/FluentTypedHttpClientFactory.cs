using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace FluentRest;

internal class FluentTypedHttpClientFactory<TClient> : ITypedHttpClientFactory<TClient>
{
    private readonly static Func<ObjectFactory> _createActivator = () => ActivatorUtilities.CreateFactory(typeof(TClient), new Type[] { typeof(HttpClient), typeof(IContentSerializer) });
    private readonly IServiceProvider _services;
    private readonly IOptionsMonitor<FluentClientFactoryOptions> _optionsMonitor;

    private readonly Lazy<ObjectFactory> _activator = new Lazy<ObjectFactory>(_createActivator);

    public FluentTypedHttpClientFactory(IServiceProvider services, IOptionsMonitor<FluentClientFactoryOptions> optionsMonitor)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        _services = services;
        _optionsMonitor = optionsMonitor;
    }

    public TClient CreateClient(HttpClient httpClient)
    {
        if (httpClient == null)
        {
            throw new ArgumentNullException(nameof(httpClient));
        }

        var activator = _activator.Value;

        var options = _optionsMonitor.Get(typeof(TClient).Name);
        var serializer = options.GetSerializer(_services);

        return (TClient)activator(_services, new object[] { httpClient, serializer });
    }
}
