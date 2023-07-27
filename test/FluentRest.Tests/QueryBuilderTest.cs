using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using Xunit;

namespace FluentRest.Tests;

public class QueryBuilderTest
{
    [Fact]
    public void QueryStringNull()
    {
        var request = new HttpRequestMessage();
        var builder = new QueryBuilder(request);

        string value = null;
        builder.BaseUri("http://test.com/");
        builder.QueryString("Test", value);

        var uri = request.GetUrlBuilder();

        Assert.Equal("http://test.com/?Test=", uri.ToString());
    }

    [Fact]
    public void QueryStringMultipleValue()
    {
        var request = new HttpRequestMessage();
        var builder = new QueryBuilder(request);

        builder.BaseUri("http://test.com/");
        builder.QueryString("Test", "Test1");
        builder.QueryString("Test", "Test2");

        var uri = request.GetUrlBuilder();

        Assert.Equal("http://test.com/?Test=Test1&Test=Test2", uri.ToString());
    }

    [Fact]
    public void HeaderSingleValue()
    {
        var request = new HttpRequestMessage();
        var builder = new QueryBuilder(request);

        builder.BaseUri("http://test.com/");
        builder.Header("Test", "Test");

        Assert.True(builder.RequestMessage.Headers.Contains("Test"));
        Assert.True(builder.RequestMessage.Headers.GetValues("Test").First() == "Test");
    }

    [Fact]
    public void HeaderNullValue()
    {
        var request = new HttpRequestMessage();
        var builder = new QueryBuilder(request);

        string value = null;
        builder.BaseUri("http://test.com/");
        builder.Header("Test", "Test");

        Assert.True(builder.RequestMessage.Headers.Contains("Test"));

        builder.Header("Test", value);
        Assert.False(builder.RequestMessage.Headers.Contains("Test"));
    }

    [Fact]
    public void QueryStringFullUri()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://blah.com");
        var builder = new QueryBuilder(request);

        builder.FullUri("http://test.com/path?q=testing&size=10");

        var urlBuilder = request.GetUrlBuilder().ToUri();

        Assert.Equal("http://test.com/path?q=testing&size=10", urlBuilder.ToString());
    }

    [Fact]
    public void AppendPathWithoutTrailingSlash()
    {
        var request = new HttpRequestMessage();
        var builder = new QueryBuilder(request);

        builder.BaseUri("http://test.com/api").AppendPath("v1");

        var urlBuilder = request.GetUrlBuilder();

        Assert.Equal("http://test.com/api/v1", urlBuilder.ToString());
    }

    [Fact]
    public void AppendPathWithTrailingSlash()
    {
        var request = new HttpRequestMessage();
        var builder = new QueryBuilder(request);

        builder.BaseUri("http://test.com/api/").AppendPath("v1");

        var urlBuilder = request.GetUrlBuilder();

        Assert.Equal("http://test.com/api/v1", urlBuilder.ToString());
    }

    [Fact]
    public void AppendParamsArrayPaths()
    {
        var request = new HttpRequestMessage();
        var builder = new QueryBuilder(request);

        builder.BaseUri("http://test.com/api/").AppendPath("v1", "bar");

        var urlBuilder = request.GetUrlBuilder();

        Assert.Equal("http://test.com/api/v1/bar", urlBuilder.ToString());
    }

    [Fact]
    public void AppendEnumerablePaths()
    {
        var request = new HttpRequestMessage();
        var builder = new QueryBuilder(request);

        IEnumerable<string> enumerablePaths = new List<string>(new[] { "v1", "bar" });
        builder.BaseUri("http://test.com/api/").AppendPath(enumerablePaths);

        var urlBuilder = request.GetUrlBuilder();

        Assert.Equal("http://test.com/api/v1/bar", urlBuilder.ToString());
    }

    [Fact]
    public void RequestUriNoBasePath()
    {
        var request = new HttpRequestMessage();
        var builder = new QueryBuilder(request);

        builder.AppendPath("http://test.com/api/v1");

        var urlBuilder = request.GetUrlBuilder();
    }
}
