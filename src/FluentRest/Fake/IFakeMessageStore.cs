using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentRest.Fake
{
    /// <summary>
    /// An <see langword="interface"/> defining the fake message store.
    /// </summary>
    public interface IFakeMessageStore
    {
        /// <summary>
        /// Saves the specified HTTP <paramref name="response"/> to the message store as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message that was sent.</param>
        /// <param name="response">The HTTP response messsage to save.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task SaveAsync(HttpRequestMessage request, HttpResponseMessage response);

        /// <summary>
        /// Loads an HTTP fake response message for the specified HTTP <paramref name="request"/> as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to load response for.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<HttpResponseMessage> LoadAsync(HttpRequestMessage request);
    }
}