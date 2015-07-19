using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FluentRest
{
    /// <summary>
    /// A JSON content serializer.
    /// </summary>
    public class JsonContentSerializer : IContentSerializer
    {
        /// <summary>
        /// Gets or sets the JSON serializer settings.
        /// </summary>
        /// <value>
        /// The JSON serializer settings.
        /// </value>
        public JsonSerializerSettings Settings { get; set; }

        /// <summary>
        /// Gets the content-type the serializer supports.
        /// </summary>
        /// <value>
        /// The content-type the serializer supports.
        /// </value>
        public string ContentType { get; } = "application/json";

        /// <summary>
        /// Serializes the specified <paramref name="data"/> object asynchronous.
        /// </summary>
        /// <param name="data">The data object to serialize.</param>
        /// <returns>The <see cref="HttpContent"/> that the data object searilzed to.</returns>
        public async Task<HttpContent> SerializeAsync(object data)
        {
            if (data == null)
                return null;

            var json = await Task.Run(() => JsonConvert.SerializeObject(data, Settings)).ConfigureAwait(false);
            var stringContent = new StringContent(json, Encoding.UTF8, ContentType);

            return stringContent;
        }

        /// <summary>
        /// Deserializes the the <see cref="HttpContent"/> asynchronous.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="content">The content to deserialize.</param>
        /// <returns>The data object deserialized from the HttpContent.</returns>
        public async Task<TData> DeserializeAsync<TData>(HttpContent content)
        {
            var json = await content.ReadAsStringAsync().ConfigureAwait(false);
            var data = await Task.Run(() => JsonConvert.DeserializeObject<TData>(json, Settings)).ConfigureAwait(false);

            return data;
        }
    }
}