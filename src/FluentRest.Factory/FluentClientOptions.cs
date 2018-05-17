using System;
using Microsoft.Extensions.DependencyInjection;

namespace FluentRest
{
    public class FluentClientOptions {
        public IContentSerializer Serializer { get; set; }
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