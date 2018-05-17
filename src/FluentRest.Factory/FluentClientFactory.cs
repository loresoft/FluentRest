using System;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace FluentRest
{
    public class FluentClientFactory : IFluentClientFactory
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptionsMonitor<FluentClientOptions> _optionsMonitor;

        public FluentClientFactory(IHttpClientFactory clientFactory, IServiceProvider serviceProvider, IOptionsMonitor<FluentClientOptions> optionsMonitor) {
            _clientFactory = clientFactory;
            _serviceProvider = serviceProvider;
            _optionsMonitor = optionsMonitor;
        }

        public IFluentClient CreateClient(string name)
        {
            var options = _optionsMonitor.Get(name);
            var serializer = options.GetSerializer(_serviceProvider);
            
            return new FluentClient(_clientFactory.CreateClient(name), serializer);
        }
    }
}