using System;
using System.Collections.Generic;

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
        /// <summary>
        /// Initializes a new instance of the <see cref="PostBuilder{TBuilder}"/> class.
        /// </summary>
        /// <param name="request">The fluent HTTP request being built.</param>
        protected PostBuilder(FluentRequest request) : base(request)
        {
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
        /// Appends the specified <paramref name="name"/> and <paramref name="value"/> to the form post body.
        /// </summary>
        /// <param name="name">The form parameter name.</param>
        /// <param name="value">The form parameter value.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public TBuilder FormValue<T>(string name, object value)
        {
            var v = value != null ? Convert.ToString(value) : string.Empty;
            return FormValue(name, v);
        }

        /// <summary>
        /// Appends the specified key value pairs to the form post body.
        /// </summary>
        /// <param name="data">The form key value parameters.</param>
        /// <returns>A fluent request builder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data" /> is <see langword="null" />.</exception>
        public TBuilder FormValue(IEnumerable<KeyValuePair<string, object>> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            foreach (var pair in data)
            {
                var v = Convert.ToString(pair.Value);
                FormValue(pair.Key, v);
            }

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