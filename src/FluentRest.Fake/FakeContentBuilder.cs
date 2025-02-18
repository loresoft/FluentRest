using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FluentRest.Fake;

/// <summary>
/// A fluent fake content builder for a <see cref="FakeResponseContainer"/>.
/// </summary>
public class FakeContentBuilder : FakeContainerBuilder<FakeResponseBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FakeContentBuilder"/> class.
    /// </summary>
    /// <param name="container">The container to build.</param>
    public FakeContentBuilder(FakeResponseContainer container) : base(container)
    {
    }

    /// <summary>
    /// Sets HTTP response header with the specified <paramref name="name"/> and <paramref name="value"/>.
    /// </summary>
    /// <param name="name">The header name.</param>
    /// <param name="value">The header value.</param>
    /// <returns>A fluent fake response builder.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    public FakeContentBuilder Header(string name, string value)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        if (value is null)
            Container.ResponseMessage.ContentHeaders.Remove(name);
        else
            Container.ResponseMessage.ContentHeaders[name] = new List<string>(new[] { value });

        return this;
    }

    /// <summary>
    /// Sets HTTP response content to JSON serialized data of the specified <paramref name="value"/> object.
    /// </summary>
    /// <typeparam name="T">The data type to serialize.</typeparam>
    /// <param name="value">The data object to be specified to JSON.</param>
    /// <param name="serializer">Used to control how the response data is serialized into a byte array.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public FakeContentBuilder Data<T>(T value, SerializeResponseContentCallback serializer = null)
    {
        byte[] content;

        // convert various passing in data types to raw bytes
        if (value is string stringContent)
        {
            content = Encoding.UTF8.GetBytes(stringContent);
        }
        else if (value is byte[] byteArrayContent)
        {
            content = byteArrayContent;
        }
        else
        {
            if (serializer is null)
                serializer = this.Container.SerializeResponseContentCallback ?? DefaultSerializer;

            content = serializer(value, typeof(T));
        }

        Container.HttpContent = content;

        Header("Content-Length", content.Length.ToString());
        Header("Content-Type", "application/json; charset=utf-8");

        return this;
    }

    private byte[] DefaultSerializer(object content, Type contentType)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.SerializeToUtf8Bytes(content, contentType, options);
    }
}
