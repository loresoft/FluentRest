using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace FluentRest
{
    /// <summary>
    /// A JSON content serializer.
    /// </summary>
    public class JsonContentSerializer : IContentSerializer
    {
        private readonly JsonSerializer _serializer;

        /// <summary>
        /// Create a new JSON content serializer with the specified settings.
        /// </summary>
        /// <param name="settings"></param>
        public JsonContentSerializer(JsonSerializerSettings settings = null)
        {
            Settings = settings;
            _serializer = JsonSerializer.Create(settings);
        }

        /// <summary>
        /// Gets or sets the JSON serializer settings.
        /// </summary>
        /// <value>
        /// The JSON serializer settings.
        /// </value>
        public JsonSerializerSettings Settings { get; }

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
                return null;

            string json;

            using (var writer = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                _serializer.Serialize(jsonWriter, data);
                jsonWriter.Flush();

                json = writer.ToString();
            }

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
            using (var s = await content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var sr = new StreamReader(s))
            {
                return (TData)_serializer.Deserialize(sr, typeof(TData));
            }
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