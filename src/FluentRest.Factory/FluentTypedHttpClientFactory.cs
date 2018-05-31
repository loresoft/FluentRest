using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace FluentRest
{
    internal class FluentTypedHttpClientFactory<TClient> : ITypedHttpClientFactory<TClient>
    {
        private readonly static Func<ObjectFactory> _createActivator = () => ActivatorUtilities.CreateFactory(typeof(TClient), new Type[] { typeof(HttpClient), typeof(IContentSerializer) });
        private readonly IServiceProvider _services;
        private readonly IOptionsMonitor<FluentClientFactoryOptions> _optionsMonitor;

        private ObjectFactory _activator;
        private bool _initialized;
        private object _lock;

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

            LazyInitializer.EnsureInitialized(ref _activator, ref _initialized, ref _lock, _createActivator);

            var options = _optionsMonitor.Get(typeof(TClient).Name);
            var serializer = options.GetSerializer(_services);
            
            return (TClient)_activator(_services, new object[] { httpClient, serializer });
        }
    }
}