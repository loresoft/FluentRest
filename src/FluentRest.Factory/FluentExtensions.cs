using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace FluentRest
{
    public static class FluentExtensions {
        public static IHttpClientBuilder UseSerializer<T>(this IHttpClientBuilder builder) where T: class, IContentSerializer {
            builder.Services.AddSingleton(typeof(T));
            builder.Services.Configure<FluentClientOptions>(builder.Name, options => options.SerializerType = typeof(T));
            return builder;
        }

        public static IHttpClientBuilder UseSerializer(this IHttpClientBuilder builder, IContentSerializer contentSerializer) {
            builder.Services.Configure<FluentClientOptions>(builder.Name, options => options.Serializer = contentSerializer);
            return builder;
        }

        public static IServiceCollection AddFluentClientFactory(this IServiceCollection services) {
            services.AddSingleton<IFluentClientFactory, FluentClientFactory>();
            services.AddSingleton(typeof(ITypedHttpClientFactory<>), typeof(FluentTypedHttpClientFactory<>));

            return services;
        }
    }
}