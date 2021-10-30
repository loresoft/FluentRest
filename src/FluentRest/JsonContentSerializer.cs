using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FluentRest
{
    /// <summary>
    /// A JSON content serializer.
    /// </summary>
    public class JsonContentSerializer : IContentSerializer
    {

        /// <summary>
        /// Create a new JSON content serializer with the specified options.
        /// </summary>
        /// <param name="options"></param>
        public JsonContentSerializer(JsonSerializerOptions options = null)
        {
            Options = options ?? new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };
        }

        /// <summary>
        /// Gets or sets the JSON serializer options.
        /// </summary>
        /// <value>
        /// The JSON serializer options.
        /// </value>
        public JsonSerializerOptions Options { get; }

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
        /// <returns>The <see cref="HttpContent"/> that the data object serialized to.</returns>
        public Task<HttpContent> SerializeAsync(object data)
        {
            if (data == null)
                return Task.FromResult<HttpContent>(null);

            var objectType = data.GetType();
            var json = JsonSerializer.Serialize(data, objectType, Options);
            var httpContent = new StringContent(json, Encoding.UTF8, ContentType);

            return Task.FromResult<HttpContent>(httpContent);
        }

        /// <summary>
        /// Deserialize the <see cref="HttpContent"/> asynchronously.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="content">The content to deserialize.</param>
        /// <returns>The data object deserialized from the HttpContent.</returns>
        public async Task<TData> DeserializeAsync<TData>(HttpContent content)
        {
            using var stream = await content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await JsonSerializer
                .DeserializeAsync<TData>(stream, Options)
                .ConfigureAwait(false);

            return result;
        }

        #region Singleton

        private static readonly Lazy<JsonContentSerializer> _current = new Lazy<JsonContentSerializer>(() => new JsonContentSerializer());

        /// <summary>
        /// Gets the current singleton instance of JsonContentSerializer.
        /// </summary>
        /// <value>The current singleton instance.</value>
        /// <remarks>
        /// An instance of JsonContentSerializer wont be created until the very first
        /// call to the sealed class. This is a CLR optimization that
        /// provides a properly lazy-loading singleton.
        /// </remarks>
        public static JsonContentSerializer Default => _current.Value;

        #endregion
    }
}
