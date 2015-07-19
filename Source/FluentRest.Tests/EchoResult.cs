using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FluentRest.Tests
{
    public class EchoResult
    {
        public EchoResult()
        {
            Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        [JsonProperty("ip")]
        public string IpAddress { get; set; }

        [JsonProperty("domains")]
        public string Domains { get; set; }

        [JsonProperty("loc")]
        public string Location { get; set; }

        [JsonProperty("time")]
        public DateTime Received { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("url")]
        public string RequestUrl { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }

        [JsonProperty("body")]
        public string BodyContent { get; set; }
    }
}