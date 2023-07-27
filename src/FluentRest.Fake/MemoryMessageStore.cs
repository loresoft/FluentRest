using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentRest.Fake;

/// <summary>
/// A memory based fake message store.  The fake response messages are saved and loaded from a ResponseStore using a sh1 has of the URL as the key.
/// </summary>
public class MemoryMessageStore : FakeMessageStore
{
    private static readonly Lazy<MemoryMessageStore> _current = new Lazy<MemoryMessageStore>(() => new MemoryMessageStore());

    /// <summary>
    /// Gets the current singleton instance of <see cref="MemoryMessageStore"/>.
    /// </summary>
    /// <value>The current singleton instance <see cref="MemoryMessageStore"/>.</value>
    public static MemoryMessageStore Current => _current.Value;

    private readonly System.Collections.Concurrent.ConcurrentDictionary<string, FakeResponseContainer> _responseStore = new System.Collections.Concurrent.ConcurrentDictionary<string, FakeResponseContainer>();

    /// <summary>
    /// Gets the response store.
    /// </summary>
    /// <value>
    /// The response store.
    /// </value>
    public System.Collections.Generic.IDictionary<string, FakeResponseContainer> ResponseStore => _responseStore;

    /// <summary>
    /// The callback used to serialize fake response data objects into a byte array.
    /// If not specified, the default settings for the System.Text.Json.JsonSerializer will be used.
    ///</summary>
    public SerializeResponseContentCallback SerializeResponseContentCallback { get; set; }

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
        // don't save content if not success
        if (!response.IsSuccessStatusCode || response.Content == null || response.StatusCode == HttpStatusCode.NoContent)
            return;

        var httpContent = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        var fakeResponse = Convert(response);

        var key = GenerateKey(request);
        var container = new FakeResponseContainer
        {
            HttpContent = httpContent,
            ResponseMessage = fakeResponse
        };


        // save to store
        _responseStore.AddOrUpdate(key, container, (k, o) => container);
    }

    /// <summary>
    /// Loads an HTTP fake response message for the specified HTTP <paramref name="request" /> as an asynchronous operation.
    /// </summary>
    /// <param name="request">The HTTP request message to load response for.</param>
    /// <returns>
    /// The task object representing the asynchronous operation.
    /// </returns>
    public override Task<HttpResponseMessage> LoadAsync(HttpRequestMessage request)
    {
        var taskSource = new TaskCompletionSource<HttpResponseMessage>();
        var key = GenerateKey(request);

        FakeResponseContainer container;

        var found = _responseStore.TryGetValue(key, out container);
        if (!found)
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            httpResponseMessage.RequestMessage = request;
            httpResponseMessage.ReasonPhrase = $"Response for key '{key}' not found";

            taskSource.SetResult(httpResponseMessage);
            return taskSource.Task;
        }

        var fakeResponse = container.ResponseMessage;
        var httpResponse = Convert(fakeResponse);

        taskSource.SetResult(httpResponse);

        if (container.HttpContent == null)
            return taskSource.Task;

        var httpContent = new ByteArrayContent(container.HttpContent);

        // copy headers
        foreach (var header in fakeResponse.ResponseHeaders)
            httpContent.Headers.TryAddWithoutValidation(header.Key, header.Value);

        httpResponse.Content = httpContent;
        httpResponse.RequestMessage = request;

        return taskSource.Task;
    }


    /// <summary>
    /// Register a fake response using the specified fluent <paramref name="builder"/> action.
    /// </summary>
    /// <param name="builder">The fluent container builder.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Register(Action<FakeContainerBuilder> builder)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        var container = new FakeResponseContainer
        {
            SerializeResponseContentCallback = SerializeResponseContentCallback
        };
        var containerBuilder = new FakeContainerBuilder(container);

        builder(containerBuilder);

        // save to store
        var key = container.RequestUri.ToString();

        _responseStore.AddOrUpdate(key, container, (k, o) => container);
    }
}

/// <summary>
/// Callback used to serialize fake response content data objects into a byte array.
///</summary>
public delegate byte[] SerializeResponseContentCallback(object content, Type contentType);
