using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FluentRest.Fake
{
    /// <summary>
    /// A file based fake message store.  The fake response messages are saved and loaded from the StorePath directory.
    /// </summary>
    public class FileMessageStore : FakeMessageStore
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
        /// Saves the specified HTTP <paramref name="response" /> to the message store as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message that was sent.</param>
        /// <param name="response">The HTTP response messsage to save.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public override async Task SaveAsync(HttpRequestMessage request, HttpResponseMessage response)
        {
            string contentPath;
            string responsePath;

            GetPaths(request, out responsePath, out contentPath);

            await SaveContent(response, contentPath).ConfigureAwait(false);
            await SaveResponse(response, responsePath).ConfigureAwait(false);
        }

        /// <summary>
        /// Loads an HTTP fake response message for the specified HTTP <paramref name="request" /> as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to load response for.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public override async Task<HttpResponseMessage> LoadAsync(HttpRequestMessage request)
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


        private async Task SaveContent(HttpResponseMessage response, string contentPath)
        {
            // don't save content if not success
            if (!response.IsSuccessStatusCode || response.Content == null || response.StatusCode == HttpStatusCode.NoContent)
                return;

            var contents = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            File.WriteAllBytes(contentPath, contents);
        }

        private Task SaveResponse(HttpResponseMessage response, string responsePath)
        {
            return Task.Factory.StartNew(() =>
            {
                var fakeResponse = Convert(response);
                var json = JsonConvert.SerializeObject(fakeResponse, Formatting.Indented);
                File.WriteAllText(responsePath, json);
            });
        }


        private HttpContent LoadContent(string contentPath)
        {
            if (!File.Exists(contentPath))
                return null;

            // need to leave stream open
            var fileStream = new FileStream(contentPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
            var httpContent = new StreamContent(fileStream, bufferSize);

            return httpContent;
        }

        private Task<HttpResponseMessage> LoadResponse(HttpContent httpContent, string responsePath)
        {
            return Task.Factory.StartNew(() =>
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
            var key = GenerateKey(request);
            var hash = GenerateHash(key);
            var rootPath = Path.GetFullPath(StorePath ?? @".\");
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var contentFile = string.Concat(hash, ".data");
            contentPath = Path.Combine(rootPath, contentFile);

            var responseFile = string.Concat(hash, ".json");
            responsePath = Path.Combine(rootPath, responseFile);
        }

        
        /// <summary>
        /// Generates a hash for the specified <paramref name="text"/>.
        /// </summary>
        /// <param name="text">The value to hash.</param>
        /// <returns>An SH1 hash of the text.</returns>
        public static string GenerateHash(string text)
        {
            var inputBytes = Encoding.UTF8.GetBytes(text);

            byte[] hashBytes;

            using (var sha1 = new System.Security.Cryptography.SHA1Managed())
                hashBytes = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

    }
}