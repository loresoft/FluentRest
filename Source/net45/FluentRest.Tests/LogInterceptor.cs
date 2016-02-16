using System;
using System.Diagnostics;
using System.Net.Http;

namespace FluentRest.Tests
{
    public class LogInterceptor : IFluentInterceptor
    {
        private readonly Stopwatch _watch = new Stopwatch();
        private readonly Action<string> _writer;

        public LogInterceptor(Action<string> writer)
        {
            _writer = writer;
        }


        public HttpRequestMessage TransformRequest(FluentRequest fluentRequest, HttpRequestMessage httpRequest)
        {
            _watch.Restart();

            _writer?.Invoke($"Request: {httpRequest}");

            return httpRequest;
        }

        public FluentResponse TransformResponse(HttpResponseMessage httpResponse, FluentResponse fluentResponse)
        {
            _watch.Stop();

            _writer?.Invoke($"Response: {httpResponse}; Time: {_watch.ElapsedMilliseconds} ms");

            return fluentResponse;
        }
    }
}