using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FluentRest.Tests
{
    public class EchoResult
    {
        public EchoResult()
        {
            Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            QueryString = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Form = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }

        [JsonProperty("args")]
        public Dictionary<string, string> QueryString { get; set; }

        [JsonProperty("form")]
        public Dictionary<string, string> Form { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("json")]
        public JObject Json { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }


}