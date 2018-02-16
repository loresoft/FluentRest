using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentRest
{
    /// <summary>
    /// An <see langword="interface"/> defining an <see cref="HttpContent"/> serializer.
    /// </summary>
    public interface IContentSerializer
    {
        /// <summary>
        /// Gets the content-type the serializer supports.
        /// </summary>
        /// <value>
        /// The content-type the serializer supports.
        /// </value>
        string ContentType { get; }

        /// <summary>
        /// Serializes the specified <paramref name="data"/> object asynchronous.
        /// </summary>
        /// <param name="data">The data object to serialize.</param>
        /// <returns>The <see cref="HttpContent"/> that the data object serialized to.</returns>
        Task<HttpContent> SerializeAsync(object data);

        /// <summary>
        /// Deserialize the <see cref="HttpContent"/> asynchronously.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="content">The content to deserialize.</param>
        /// <returns>The data object deserialized from the HttpContent.</returns>
        Task<TData> DeserializeAsync<TData>(HttpContent content);
    }
}