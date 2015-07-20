using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FluentRest.Fake
{
    /// <summary>
    /// A fake HTTP handler to save and load responses for testing.
    /// </summary>
    public class FakeMessageHandler : HttpClientHandler
    {
        private const int bufferSize = 4096;

        /// <summary>
        /// Gets or sets the directory location to store response files.
        /// </summary>
        /// <value>
        /// The directory location to store response files.
        /// </value>
        public string StorePath { get; set; }

        /// <summary>
        /// Gets or sets the fake response mode.
        /// </summary>
        /// <value>
        /// The fake response mode.
        /// </value>
        public FakeResponseMode Mode { get; set; }


        /// <summary>
        /// Sends specified <paramref name="request"/> the asynchronous.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = Mode == FakeResponseMode.Capture || Mode == FakeResponseMode.Normal
                ? await base.SendAsync(request, cancellationToken).ConfigureAwait(false)
                : await LoadAsync(request).ConfigureAwait(false);

            if (Mode == FakeResponseMode.Capture)
                await SaveAsync(request, response).ConfigureAwait(false);

            return response;
        }


        private async Task SaveAsync(HttpRequestMessage request, HttpResponseMessage response)
        {
            string contentPath;
            string responsePath;

            GetPaths(request, out responsePath, out contentPath);

            await SaveContent(response, contentPath).ConfigureAwait(false);
            await SaveResponse(response, responsePath).ConfigureAwait(false);
        }

        private static async Task SaveContent(HttpResponseMessage response, string contentPath)
        {
            // don't save content if not success
            if (!response.IsSuccessStatusCode || response.Content == null || response.StatusCode == HttpStatusCode.NoContent)
                return;

            var contents = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            File.WriteAllBytes(contentPath, contents);
        }

        private static Task SaveResponse(HttpResponseMessage response, string responsePath)
        {
            return Task.Run(() =>
            {
                var fakeResponse = Convert(response);
                var json = JsonConvert.SerializeObject(fakeResponse, Formatting.Indented);
                File.WriteAllText(responsePath, json);
            });
        }


        private async Task<HttpResponseMessage> LoadAsync(HttpRequestMessage request)
        {
            string contentPath;
            string responsePath;
            GetPaths(request, out responsePath, out contentPath);

            if (!File.Exists(responsePath))
            {
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
                httpResponseMessage.RequestMessage = request;
                httpResponseMessage.ReasonPhrase = $"Response file '{responsePath}' not found";
                return httpResponseMessage;
            }

            var httpContent = LoadContent(contentPath);

            var httpResponse = await LoadResponse(httpContent, responsePath).ConfigureAwait(false);
            httpResponse.RequestMessage = request;

            return httpResponse;
        }

        private static HttpContent LoadContent(string contentPath)
        {
            if (!File.Exists(contentPath))
                return null;

            // need to leave stream open
            var fileStream = new FileStream(contentPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
            var httpContent = new StreamContent(fileStream, bufferSize);

            return httpContent;
        }

        private static Task<HttpResponseMessage> LoadResponse(HttpContent httpContent, string responsePath)
        {
            return Task.Run(() =>
            {
                var json = File.ReadAllText(responsePath);
                var fakeResponse = JsonConvert.DeserializeObject<FakeResponseMessage>(json);
                var httpResponse = Convert(fakeResponse);

                if (httpContent == null)
                    return httpResponse;

                // copy headers
                foreach (var header in fakeResponse.ResponseHeaders)
                    httpContent.Headers.TryAddWithoutValidation(header.Key, header.Value);

                httpResponse.Content = httpContent;

                return httpResponse;
            });
        }


        private void GetPaths(HttpRequestMessage request, out string responsePath, out string contentPath)
        {
            var hash = RequestHash(request);
            var rootPath = Path.GetFullPath(StorePath ?? @".\");
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var contentFile = string.Concat(hash, ".data");
            contentPath = Path.Combine(rootPath, contentFile);

            var responseFile = string.Concat(hash, ".json");
            responsePath = Path.Combine(rootPath, responseFile);
        }


        private static FakeResponseMessage Convert(HttpResponseMessage httpResponse)
        {
            var response = new FakeResponseMessage();
            response.ReasonPhrase = httpResponse.ReasonPhrase;
            response.StatusCode = httpResponse.StatusCode;
            response.ResponseHeaders = httpResponse.Headers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value); ;
            response.ContentHeaders = httpResponse.Content.Headers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value); ;

            return response;
        }

        private static HttpResponseMessage Convert(FakeResponseMessage fakeResponse)
        {
            var response = new HttpResponseMessage();
            response.ReasonPhrase = fakeResponse.ReasonPhrase;
            response.StatusCode = fakeResponse.StatusCode;

            foreach (var header in fakeResponse.ResponseHeaders)
                response.Headers.Add(header.Key, header.Value);

            return response;
        }


        private static string RequestHash(HttpRequestMessage request)
        {
            // use ToString as fingerprint
            var requestString = request.ToString();
            var inputBytes = Encoding.UTF8.GetBytes(requestString);

            byte[] hashBytes;

            using (var sha1 = SHA1.Create())
                hashBytes = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
