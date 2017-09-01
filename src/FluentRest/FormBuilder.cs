using System;
using System.Collections.Generic;
using System.Net.Http;

namespace FluentRest
{
    /// <summary>
    /// A fluent form post builder.
    /// </summary>
    public sealed class FormBuilder : PostBuilder<FormBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormBuilder"/> class.
        /// </summary>
        /// <param name="request">The fluent HTTP request being built.</param>
        public FormBuilder(FluentRequest request) : base(request)
        {
        }
    }

    /// <summary>
    /// A fluent form post builder.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder.</typeparam>
    public abstract class PostBuilder<TBuilder> : QueryBuilder<TBuilder>
        where TBuilder : PostBuilder<TBuilder>
    {
        internal static readonly HttpMethod HttpPatch = new HttpMethod("PATCH");

        /// <summary>
        /// Initializes a new instance of the <see cref="PostBuilder{TBuilder}"/> class.
        /// </summary>
        /// <param name="request">The fluent HTTP request being built.</param>
        protected PostBuilder(FluentRequest request) : base(request)
        {
        }


        /// <summary>
        /// Sets HTTP request method.
        /// </summary>
        /// <param name="method">The header request method.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> is <see langword="null" />.</exception>
        public TBuilder Method(HttpMethod method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            Request.Method = method;

            return this as TBuilder;
        }

        /// <summary>
        /// Sets HTTP request method to POST.
        /// </summary>
        /// <returns>A fluent request builder.</returns>
        public TBuilder Post()
        {
            return Method(HttpMethod.Post);
        }

        /// <summary>
        /// Sets HTTP request method to PUT.
        /// </summary>
        /// <returns>A fluent request builder.</returns>
        public TBuilder Put()
        {
            return Method(HttpMethod.Put);
        }

        /// <summary>
        /// Sets HTTP request method to PATCH.
        /// </summary>
        /// <returns>A fluent request builder.</returns>
        public TBuilder Patch()
        {
            return Method(HttpPatch);
        }

        /// <summary>
        /// Sets HTTP request method to DELETE.
        /// </summary>
        /// <returns>A fluent request builder.</returns>
        public TBuilder Delete()
        {
            return Method(HttpMethod.Delete);
        }

        /// <summary>
        /// Appends the specified <paramref name="name"/> and <paramref name="value"/> to the form post body.
        /// </summary>
        /// <param name="name">The form parameter name.</param>
        /// <param name="value">The form parameter value.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public TBuilder FormValue(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var v = value ?? string.Empty;

            var list = Request.FormData.GetOrAdd(name, n => new List<string>());
            list.Add(v);

            return this as TBuilder;
        }

        /// <summary>
        /// Appends the specified <paramref name="name" /> and <paramref name="value" /> to the form post body.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The form parameter name.</param>
        /// <param name="value">The form parameter value.</param>
        /// <returns>
        /// A fluent request builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public TBuilder FormValue<TValue>(string name, TValue value)
        {
            var v = value != null ? value.ToString() : string.Empty;
            return FormValue(name, v);
        }

        /// <summary>
        /// Appends the specified key value pairs to the form post body.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="data">The form key value parameters.</param>
        /// <returns>
        /// A fluent request builder.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="data" /> is <see langword="null" />.</exception>
        public TBuilder FormValue<TValue>(IEnumerable<KeyValuePair<string, TValue>> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            foreach (var pair in data)
                FormValue(pair.Key, pair.Value);

            return this as TBuilder;
        }

        /// <summary>
        /// Appends the specified key value pairs to the form post body.
        /// </summary>
        /// <param name="data">The form key value parameters.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data" /> is <see langword="null" />.</exception>
        public TBuilder FormValue(IEnumerable<KeyValuePair<string, string>> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            foreach (var pair in data)
                FormValue(pair.Key, pair.Value);

            return this as TBuilder;
        }

        /// <summary>
        /// Appends the specified <paramref name="name"/> and <paramref name="value"/> to the form post body if the specified <paramref name="condition"/> is true.
        /// </summary>
        /// <param name="condition">If condition is true, form data will be added; otherwise ignore form data.</param>
        /// <param name="name">The form parameter name.</param>
        /// <param name="value">The form parameter value.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public TBuilder FormValueIf(Func<bool> condition, string name, string value)
        {
            if (condition == null || !condition())
                return this as TBuilder;

            return FormValue(name, value);
        }

        /// <summary>
        /// Appends the specified <paramref name="name" /> and <paramref name="value" /> to the form post body if the specified <paramref name="condition" /> is true.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="condition">If condition is true, form data will be added; otherwise ignore form data.</param>
        /// <param name="name">The form parameter name.</param>
        /// <param name="value">The form parameter value.</param>
        /// <returns>
        /// A fluent request builder.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public TBuilder FormValueIf<TValue>(Func<bool> condition, string name, TValue value)
        {
            if (condition == null || !condition())
                return this as TBuilder;

            return FormValue(name, value);
        }


        /// <summary>
        /// Sets the raw post body to the serialized content of the specified <paramref name="data"/> object.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="data">The data to be serialized.</param>
        /// <returns>A fluent request builder.</returns>
        /// <remarks>Setting the content of the request overrides any calls to FormValue.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null" />.</exception>
        public TBuilder Content<TData>(TData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            Request.ContentData = data;
            return this as TBuilder;
        }
    }
}