using System.Net.Http;

using Microsoft.Extensions.DependencyInjection;

namespace FluentRest;

/// <summary>
/// A factory abstraction for a component that can create <see cref="FluentClient"/> instances with custom
/// configuration for a given logical name.
/// </summary>
/// <remarks>
/// A default <see cref="IFluentClientFactory"/> can be registered in an <see cref="IServiceCollection"/>
/// by calling <see cref="HttpClientFactoryServiceCollectionExtensions.AddHttpClient(IServiceCollection)"/>.
/// The default <see cref="IFluentClientFactory"/> will be registered in the service collection as a singleton.
/// </remarks>
public interface IFluentClientFactory
{
    /// <summary>
    /// Creates and configures an <see cref="IFluentClient"/> instance using the configuration that corresponds
    /// to the logical name specified by <paramref name="name"/>.
    /// </summary>
    IFluentClient CreateClient(string name);
}
