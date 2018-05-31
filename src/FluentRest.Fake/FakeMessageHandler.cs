using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FluentRest.Fake
{
    /// <summary>
    /// A fake HTTP handler to save and load responses for testing.
    /// </summary>
    public class FakeMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeMessageHandler"/> class.
        /// </summary>
        public FakeMessageHandler()
            : this(MemoryMessageStore.Current, FakeResponseMode.Fake)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeMessageHandler"/> with the specified fake message store.
        /// </summary>
        /// <param name="messageStore">The fake message store.</param>
        public FakeMessageHandler(IFakeMessageStore messageStore)
            : this(messageStore, FakeResponseMode.Fake)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeMessageHandler"/> with the specified fake message store.
        /// </summary>
        /// <param name="messageStore">The fake message store.</param>
        /// <param name="mode">The fake response mode.</param>
        public FakeMessageHandler(IFakeMessageStore messageStore, FakeResponseMode mode)
        {
            Mode = mode;
            MessageStore = messageStore;
        }

        /// <summary>
        /// Gets or sets the fake response mode.
        /// </summary>
        /// <value>
        /// The fake response mode.
        /// </value>
        public FakeResponseMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the fake message store.
        /// </summary>
        /// <value>
        /// The fake message store.
        /// </value>
        public IFakeMessageStore MessageStore { get; set; }


        /// <summary>
        /// Sends an HTTP <paramref name="request"/> with a cancellation token as an asynchronous operation
        /// </summary>
        /// <param name="request">The HTTP request message to send.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = Mode == FakeResponseMode.Capture || Mode == FakeResponseMode.Normal
                ? await base.SendAsync(request, cancellationToken).ConfigureAwait(false)
                : await MessageStore.LoadAsync(request).ConfigureAwait(false);

            if (Mode == FakeResponseMode.Capture)
                await MessageStore.SaveAsync(request, response).ConfigureAwait(false);

            return response;
        }
    }
}
