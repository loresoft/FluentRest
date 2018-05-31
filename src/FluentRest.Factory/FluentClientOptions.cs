using System;
using Microsoft.Extensions.DependencyInjection;

namespace FluentRest
{
    /// <summary>
    /// An options class for configuring the default <see cref="IFluentClientFactory"/>.
    /// </summary>
    public class FluentClientFactoryOptions {
        /// <summary>
        /// Gets or sets a <see cref="IContentSerializer"/> instance to use. If SerializerType is set then that will be used.
        /// </summary>
        public IContentSerializer Serializer { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="IContentSerializer"/> type to use. This type needs to be registered in the services collection.
        /// </summary>
        public Type SerializerType { get; set; }

        internal IContentSerializer GetSerializer(IServiceProvider serviceProvider) {
            if (SerializerType != null)
                return serviceProvider.GetRequiredService(SerializerType) as IContentSerializer;
            
            if (Serializer != null)
                return Serializer;
            
            IContentSerializer serializer;
            serializer = serviceProvider.GetService<IContentSerializer>();
            if (serializer == null)
                serializer = ContentSerializer.Current;

            return serializer;
        }
    }
}