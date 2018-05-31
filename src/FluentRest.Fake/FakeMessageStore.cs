using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentRest.Fake
{
    /// <summary>
    /// A <see langword="base"/> class for <see cref="IFakeMessageStore"/>.
    /// </summary>
    public abstract class FakeMessageStore : IFakeMessageStore
    {
        /// <summary>
        /// Saves the specified HTTP <paramref name="response" /> to the message store as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message that was sent.</param>
        /// <param name="response">The HTTP response messsage to save.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public abstract Task SaveAsync(HttpRequestMessage request, HttpResponseMessage response);

        /// <summary>
        /// Loads an HTTP fake response message for the specified HTTP <paramref name="request" /> as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to load response for.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public abstract Task<HttpResponseMessage> LoadAsync(HttpRequestMessage request);

        /// <summary>
        /// Converts the specified HTTP response to a fake response message.
        /// </summary>
        /// <param name="httpResponse">The HTTP response to convert.</param>
        /// <returns>A fake response messages.</returns>
        protected virtual FakeResponseMessage Convert(HttpResponseMessage httpResponse)
        {
            var response = new FakeResponseMessage();
            response.ReasonPhrase = httpResponse.ReasonPhrase;
            response.StatusCode = httpResponse.StatusCode;
            response.ResponseHeaders = httpResponse.Headers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value); ;
            response.ContentHeaders = httpResponse.Content.Headers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value); ;

            return response;
        }

        /// <summary>
        /// Converts the specified fake response to an HTTP response message.
        /// </summary>
        /// <param name="fakeResponse">The fake response to convert.</param>
        /// <returns>An HTTP response message.</returns>
        protected virtual HttpResponseMessage Convert(FakeResponseMessage fakeResponse)
        {
            var response = new HttpResponseMessage();
            response.ReasonPhrase = fakeResponse.ReasonPhrase;
            response.StatusCode = fakeResponse.StatusCode;

            foreach (var header in fakeResponse.ResponseHeaders)
                response.Headers.Add(header.Key, header.Value);

            return response;
        }

        /// <summary>
        /// Generates a hash for the specified HTTP <paramref name="request"/>.
        /// </summary>
        /// <param name="request">The HTTP request to hash.</param>
        /// <returns>An SH1 hash of the HTTP request.</returns>
        public virtual string GenerateKey(HttpRequestMessage request)
        {
            // use RequestUri as fingerprint
            return request.RequestUri.ToString();
        }
    }
}
