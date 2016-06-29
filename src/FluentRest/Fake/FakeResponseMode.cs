using System;

namespace FluentRest.Fake
{
    /// <summary>
    /// The fake response modes
    /// </summary>
    public enum FakeResponseMode
    {
        /// <summary>
        /// Send HTTP request as normal.
        /// </summary>
        Normal,
        /// <summary>
        /// Respond using a fake HTTP response.
        /// </summary>
        Fake,
        /// <summary>
        /// Send HTTP request as normal and save the response for faking.
        /// </summary>
        Capture
    }
}