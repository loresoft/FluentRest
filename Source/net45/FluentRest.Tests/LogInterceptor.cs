using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentRest.Tests
{
    public class LogInterceptor : IFluentClientInterceptor
    {
        private readonly Action<string> _writer;
        private const string _key = "LogInterceptor:Stopwatch";

        public LogInterceptor(Action<string> writer)
        {
            _writer = writer;
        }

        public Task RequestAsync(InterceptorRequestContext requestContext)
        {
            var fluentRequest = requestContext.Request;
            var httpRequest = requestContext.HttpRequest;

            var watch = Stopwatch.StartNew();
            fluentRequest.State[_key] = watch;

            _writer?.Invoke($"Request: {httpRequest}");

            // use WhenAll for backward compatibility
            return Task.WhenAll();
        }

        public Task ResponseAsync(InterceptorResponseContext responseContext)
        {
            var fluentResponse = responseContext.Response;
            var httpResponse = responseContext.HttpResponse;

            var message = $"Response: {httpResponse}";

            var watch = fluentResponse.Request?.GetState<Stopwatch>(_key);
            if (watch != null)
            {
                watch.Stop();
                message += $"; Time: {watch.ElapsedMilliseconds} ms";
            }

            _writer?.Invoke(message);


            // use WhenAll for backward compatibility
            return Task.WhenAll();
        }
    }
}