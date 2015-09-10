﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FluentRest.Fake
{
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
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (value == null)
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
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        public FakeContentBuilder Data<T>(T value)
        {
            var json = JsonConvert.SerializeObject(value, Formatting.Indented);
            var content = Encoding.UTF8.GetBytes(json);

            Container.HttpContent = content;

            Header("Content-Length", content.Length.ToString());
            Header("Content-Type", "application/json; charset=utf-8");

            return this;
        }
    }
}