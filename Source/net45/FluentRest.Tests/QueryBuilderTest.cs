using System;
using System.Linq;
using Xunit;

namespace FluentRest.Tests
{
    public class QueryBuilderTest
    {
        [Fact]
        public void QueryStringNull()
        {
            var request = new FluentRequest();
            var builder = new QueryBuilder(request);

            string value = null;
            builder.BaseUri("http://test.com/");
            builder.QueryString("Test", value);

            var uri = request.RequestUri();

            Assert.Equal("http://test.com/?Test=", uri.ToString());
        }

        [Fact]
        public void QueryStringMultipleValue()
        {
            var request = new FluentRequest();
            var builder = new QueryBuilder(request);

            builder.BaseUri("http://test.com/");
            builder.QueryString("Test", "Test1");
            builder.QueryString("Test", "Test2");

            var uri = request.RequestUri();

            Assert.Equal("http://test.com/?Test=Test1&Test=Test2", uri.ToString());
        }

        [Fact]
        public void HeaderSingleValue()
        {
            var request = new FluentRequest();
            var builder = new QueryBuilder(request);

            builder.BaseUri("http://test.com/");
            builder.Header("Test", "Test");

            Assert.True(builder.Request.Headers.ContainsKey("Test"));
            Assert.True(builder.Request.Headers["Test"].First() == "Test");
        }

        [Fact]
        public void HeaderNullValue()
        {
            var request = new FluentRequest();
            var builder = new QueryBuilder(request);

            string value = null;
            builder.BaseUri("http://test.com/");
            builder.Header("Test", "Test");

            Assert.True(builder.Request.Headers.ContainsKey("Test"));

            builder.Header("Test", value);
            Assert.False(builder.Request.Headers.ContainsKey("Test"));
        }

        [Fact]
        public void QueryStringFullUri()
        {
            var request = new FluentRequest();
            var builder = new QueryBuilder(request);

            builder.FullUri("http://test.com/path?q=testing&size=10");


            Assert.Equal("http://test.com/path", request.BaseUri.ToString());
            Assert.Equal(2, request.QueryString.Count);
            Assert.Equal("testing", request.QueryString["q"].FirstOrDefault());
        }

        [Fact]
        public void AppendPathWithoutTrailingSlash()
        {
            var request = new FluentRequest();
            var builder = new QueryBuilder(request);

            builder.BaseUri("http://test.com/api").AppendPath("v1");

            var url = request.RequestUri();

            Assert.Equal("http://test.com/api/v1", url.ToString());
        }

        [Fact]
        public void AppendPathWithTrailingSlash()
        {
            var request = new FluentRequest();
            var builder = new QueryBuilder(request);

            builder.BaseUri("http://test.com/api/").AppendPath("v1");

            var url = request.RequestUri();

            Assert.Equal("http://test.com/api/v1", url.ToString());
        }

        [Fact]
        public void RequestUriNoBasePath()
        {
            var request = new FluentRequest();
            var builder = new QueryBuilder(request);

            builder.AppendPath("http://test.com/api/v1");

            Assert.Throws<FluentException>(() => request.RequestUri());
        }

        [Fact]
        public void RequestUriInvalid()
        {
            var request = new FluentRequest();
            var builder = new QueryBuilder(request);

            Assert.Throws<UriFormatException>(() => builder.BaseUri("/api/").AppendPath("v1"));
        }

        [Fact]
        public void RequestUriInvalidUri()
        {
            var request = new FluentRequest();
            var builder = new QueryBuilder(request);
            var baseUri = new Uri("/api", UriKind.Relative);

            Assert.Throws<UriFormatException>(() =>
            {
                builder.BaseUri(baseUri).AppendPath("v1");
                var uri = request.RequestUri();
            });
        }

    }
}
