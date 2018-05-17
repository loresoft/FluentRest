using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace FluentRest
{
    /// <summary>
    /// Extension methods for configuring an <see cref="FluentClient"/>
    /// </summary>
    public static class FluentExtensions {
        /// <summary>
        /// Sets the content serializer type to use when using the <see cref="FluentClient"/>.
        /// </summary>
        public static IHttpClientBuilder SetSerializer<T>(this IHttpClientBuilder builder) where T: class, IContentSerializer {
            builder.Services.AddSingleton(typeof(T));
            builder.Services.Configure<FluentClientFactoryOptions>(builder.Name, options => options.SerializerType = typeof(T));
            return builder;
        }

        /// <summary>
        /// Sets the content serializer instance to use when using the <see cref="FluentClient"/>.
        /// </summary>
        public static IHttpClientBuilder SetSerializer(this IHttpClientBuilder builder, IContentSerializer contentSerializer) {
            builder.Services.Configure<FluentClientFactoryOptions>(builder.Name, options => options.Serializer = contentSerializer);
            return builder;
        }

        /// <summary>
        /// Adds support for using <see cref="FluentClient"/> with <see cref="IHttpClientFactory"/>.
        /// </summary>
        public static IServiceCollection AddFluentClient(this IServiceCollection services) {
            services.AddHttpClient();
            services.AddSingleton<IFluentClientFactory, FluentClientFactory>();
            services.AddSingleton(typeof(ITypedHttpClientFactory<>), typeof(FluentTypedHttpClientFactory<>));

            return services;
        }
    }
}