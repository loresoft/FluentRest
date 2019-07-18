using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FluentRest.Tests
{
    public class UrlBuilderTests
    {
        private readonly ITestOutputHelper _output;

        public UrlBuilderTests(ITestOutputHelper output)
        {
            _output = output;
        }


        [Theory]
        [InlineData("http://foo/bar/baz", "date", "today", "http://foo/bar/baz?date=today")]
        [InlineData("http://foo/bar/baz", "date", "sunday afternoon", "http://foo/bar/baz?date=sunday+afternoon")]
        [InlineData("http://foo/bar/baz?date=today", "key1", "value1", "http://foo/bar/baz?date=today&key1=value1")]
        [InlineData("http://foo/bar/baz?date=today", "key1", "value 1&", "http://foo/bar/baz?date=today&key1=value+1%26")]
        [InlineData("foo/bar/baz?date=today", "key1", "value 1&", "http://foo/bar/baz?date=today&key1=value+1%26")]
        public void AppendQuery(string url, string key, string value, string expected)
        {
            var builder = new UrlBuilder(url);
            builder.Should().NotBeNull();

            builder.AppendQuery(key, value);
            builder.Query.Should().NotBeEmpty();
            builder.Query.Should().ContainKey(key);
            builder.Query[key].Should().Contain(value);

            builder.ToString().Should().Be(expected);

        }

        [Theory]
        [InlineData("http://foo/bar/baz", "date=today", "http://foo/bar/baz?date=today")]
        [InlineData("http://foo/bar/baz?date=today", "date=yesterday", "http://foo/bar/baz?date=yesterday")]
        [InlineData("http://foo/bar/baz?date=today", "?date=tomorrow", "http://foo/bar/baz?date=tomorrow")]
        [InlineData("http://foo/bar/baz?date=today", "&date=tomorrow", "http://foo/bar/baz?date=tomorrow")]
        [InlineData("http://foo/bar/baz?date=today", "date=", "http://foo/bar/baz?date=")]
        [InlineData("foo/bar/baz?date=today", "date=", "http://foo/bar/baz?date=")]
        public void SetQuery(string url, string query, string expected)
        {
            var builder = new UrlBuilder(url);
            builder.Should().NotBeNull();

            builder.SetQuery(query);
            builder.Query.Should().NotBeEmpty();
            builder.Query.Count.Should().Be(1);
            builder.ToString().Should().Be(expected);

        }

        [Theory]
        [InlineData("http://foo/bar/baz?date=today", null, "http://foo/bar/baz")]
        [InlineData("http://foo/bar/baz?date=today", "", "http://foo/bar/baz")]
        [InlineData("http://foo/bar/baz?date=today", "?", "http://foo/bar/baz")]
        [InlineData("http://foo/bar/baz?date=today", "&", "http://foo/bar/baz")]
        [InlineData("foo/bar/baz?date=today", "&", "http://foo/bar/baz")]
        public void SetQueryEmpty(string url, string query, string expected)
        {
            var builder = new UrlBuilder(url);
            builder.Should().NotBeNull();

            builder.SetQuery(query);
            builder.Query.Should().BeEmpty();
            builder.ToString().Should().Be(expected);

        }

        [Theory]
        [InlineData("http://foo.com/", "/bar/baz", "http://foo.com/bar/baz")]
        [InlineData("http://foo.com/bar", "baz", "http://foo.com/bar/baz")]
        [InlineData("http://foo.com/bar", "/baz", "http://foo.com/bar/baz")]
        [InlineData("http://foo.com/bar/", "/baz", "http://foo.com/bar/baz")]
        [InlineData("foo.com/bar/", "/baz", "http://foo.com/bar/baz")]
        public void AppendPath(string url, string path, string expected)
        {
            var builder = new UrlBuilder(url);
            builder.Should().NotBeNull();

            builder.AppendPath(path);
            builder.Path.Should().NotBeEmpty();
            builder.ToString().Should().Be(expected);
        }

        [Fact]
        public void AppendParamsPath()
        {
            var builder = new UrlBuilder("http://foo.com/");
            builder.Should().NotBeNull();

            builder.AppendPaths("bar", "baz");
            builder.Path.Should().NotBeEmpty();
            builder.ToString().Should().Be("http://foo.com/bar/baz");
        }

        [Theory]
        [InlineData("http://foo.com/", 123, "http://foo.com/123")]
        [InlineData("http://foo.com/bar", 5, "http://foo.com/bar/5")]
        public void AppendPathTypeInt(string url, int path, string expected)
        {
            var builder = new UrlBuilder(url);
            builder.Should().NotBeNull();

            builder.AppendPath(path);
            builder.Path.Should().NotBeEmpty();
            builder.ToString().Should().Be(expected);
        }
        
        [Theory]
        [InlineData("http://foo.com/", "/bar/baz", "http://foo.com/bar/baz")]
        [InlineData("http://foo.com/bar", "baz", "http://foo.com/baz")]
        [InlineData("http://foo.com/bar", "/baz", "http://foo.com/baz")]
        [InlineData("foo.com/bar", "/baz", "http://foo.com/baz")]
        public void SetPath(string url, string path, string expected)
        {
            var builder = new UrlBuilder(url);
            builder.Should().NotBeNull();

            builder.SetPath(path);
            builder.Path.Should().NotBeEmpty();
            builder.ToString().Should().Be(expected);
        }


        [Theory]
        [InlineData("http://foo.com/bar/baz", null, "http://foo.com/")]
        [InlineData("http://foo.com/bar/baz", "", "http://foo.com/")]
        [InlineData("http://foo.com/bar/baz", "/", "http://foo.com/")]
        public void SetPathEmpty(string url, string path, string expected)
        {
            var builder = new UrlBuilder(url);
            builder.Should().NotBeNull();

            builder.SetPath(path);
            builder.Path.Should().BeEmpty();
            builder.ToString().Should().Be(expected);

        }


        [Fact]
        public void Build()
        {
            var builder = new UrlBuilder();
            builder
                .SetScheme("https")
                .SetHost("foo.com")
                .SetPort(443)
                .AppendPath("/bar");

            builder.Scheme.Should().Be("https");
            builder.Host.Should().Be("foo.com");
            builder.Port.Should().Be(443);

            builder.Path.Should().NotBeEmpty();
            builder.Path.Should().Contain("bar");

            builder.ToString().Should().Be("https://foo.com/bar");

        }
    }
}
