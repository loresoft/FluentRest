using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FluentRest
{
    public class JsonContent : HttpContent
    {
        public object Content { get; }

        public JsonSerializerSettings Settings { get; set; }

        public JsonContent(object content) : this(content, JsonConvert.DefaultSettings())
        {
        }

        public JsonContent(object content, JsonSerializerSettings settings)
        {
            Content = content;
            Settings = settings;

            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            // wrap in task
            return Task.Run(() =>
            {
                // using write stream directly for efficiency 
                var streamWriter = new StreamWriter(stream);
                var jsonWriter = new JsonTextWriter(streamWriter);
                var jsonSerializer = JsonSerializer.Create(Settings);
                jsonSerializer.Serialize(jsonWriter, Content);
            });
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
    }
}