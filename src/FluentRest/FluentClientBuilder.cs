using System;
using System.Collections.Generic;
using System.Net.Http;

namespace FluentRest
{
    /// <summary>
    /// A fluent builder for <see cref="FluentClient"/>.
    /// </summary>
    public class FluentClientBuilder
    {
        /// <summary>
        /// Gets a value indicating whether the inner handler should be disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the inner handler should be disposed of by the Dispose method,
        /// <c>false</c> if you intend to reuse the inner handler.
        /// </value>
        public bool ShouldDisposeHandler { get; private set; } = true;

        /// <summary>
        /// Gets the serializer to convert to and from <see cref="HttpContent"/>.
        /// </summary>
        /// <value>
        /// The serializer to convert to and from <see cref="HttpContent"/>.
        /// </value>
        public IContentSerializer ContentSerializer { get; private set; } = new JsonContentSerializer();

        /// <summary>
        /// Gets the HTTP handler stack to use for sending requests.
        /// </summary>
        /// <value>
        /// The HTTP handler stack to use for sending requests.
        /// </value>
        public HttpMessageHandler MessageHandler { get; private set; } = new HttpClientHandler();

        /// <summary>
        /// Gets the fluent client interceptors.
        /// </summary>
        /// <value>
        /// The fluent client interceptors.
        /// </value>
        public List<IFluentClientInterceptor> Interceptors { get; } = new List<IFluentClientInterceptor>();

        /// <summary>
        /// Gets the default request.
        /// </summary>
        /// <value>
        /// The default request.
        /// </value>
        public FluentRequest DefaultRequest { get; } = new FluentRequest();



        /// <summary>
        /// Set the serializer to convert to and from HttpContent.
        /// </summary>
        /// <typeparam name="T">The type of serializer.</typeparam>
        /// <returns>A fluent client builder.</returns>
        public FluentClientBuilder Serializer<T>()
                    where T : IContentSerializer, new()
        {
            var v = new T();
            return Serializer(v);
        }

        /// <summary>
        /// Set the serializer to convert to and from HttpContent.
        /// </summary>
        /// <typeparam name="T">The type of serializer.</typeparam>
        /// <param name="factory">The factory to create the serialzier.</param>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        public FluentClientBuilder Serializer<T>(Func<T> factory)
            where T : IContentSerializer, new()
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var v = factory();
            return Serializer(v);
        }

        /// <summary>
        /// Set the serializer to convert to and from HttpContent.
        /// </summary>
        /// <typeparam name="T">The type of serializer.</typeparam>
        /// <param name="serializer">The serializer to use.</param>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        public FluentClientBuilder Serializer<T>(T serializer)
            where T : IContentSerializer
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            ContentSerializer = serializer;
            return this;
        }


        /// <summary>
        /// Set the HTTP handler stack to use for sending requests.
        /// </summary>
        /// <typeparam name="T">The type of the HTTP handler</typeparam>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        public FluentClientBuilder HttpHandler<T>()
            where T : HttpMessageHandler, new()
        {
            var v = new T();
            return HttpHandler(v);
        }

        /// <summary>
        /// Set the HTTP handler stack to use for sending requests.
        /// </summary>
        /// <typeparam name="T">The type of the HTTP handler</typeparam>
        /// <param name="factory">The factory to create the HTTP handler.</param>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        public FluentClientBuilder HttpHandler<T>(Func<T> factory)
            where T : HttpMessageHandler
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var v = factory();
            return HttpHandler(v);
        }

        /// <summary>
        /// Set the HTTP handler stack to use for sending requests.
        /// </summary>
        /// <typeparam name="T">The type of the HTTP handler</typeparam>
        /// <param name="handler">The HTTP handler to use.</param>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        public FluentClientBuilder HttpHandler<T>(T handler)
            where T : HttpMessageHandler
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            MessageHandler = handler;
            return this;
        }


        /// <summary>
        /// Add the fluent client interceptor.
        /// </summary>
        /// <typeparam name="T">The type of the fluent client interceptor.</typeparam>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        public FluentClientBuilder Interceptor<T>()
                    where T : IFluentClientInterceptor, new()
        {
            var v = new T();
            return Interceptor(v);
        }

        /// <summary>
        /// Add the fluent client interceptor.
        /// </summary>
        /// <typeparam name="T">The type of the fluent client interceptor.</typeparam>
        /// <param name="factory">The factory to create the interceptor.</param>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        public FluentClientBuilder Interceptor<T>(Func<T> factory)
            where T : IFluentClientInterceptor
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var v = factory();
            return Interceptor(v);
        }

        /// <summary>
        /// Add the fluent client interceptor.
        /// </summary>
        /// <typeparam name="T">The type of the fluent client interceptor.</typeparam>
        /// <param name="interceptor">The interceptor to add.</param>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        public FluentClientBuilder Interceptor<T>(T interceptor)
            where T : IFluentClientInterceptor
        {
            if (interceptor == null)
                throw new ArgumentNullException(nameof(interceptor));

            Interceptors.Add(interceptor);
            return this;
        }

        /// <summary>
        /// Add the fluent client interceptors.
        /// </summary>
        /// <typeparam name="T">The type of the fluent client interceptor.</typeparam>
        /// <param name="factory">The factory to create the interceptors.</param>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        public FluentClientBuilder Interceptor<T>(Func<IEnumerable<T>> factory)
            where T : IFluentClientInterceptor
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var v = factory();
            return Interceptor(v);
        }

        /// <summary>
        /// Add the fluent client interceptors.
        /// </summary>
        /// <typeparam name="T">The type of the fluent client interceptor.</typeparam>
        /// <param name="interceptors">The interceptors to add.</param>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        public FluentClientBuilder Interceptor<T>(IEnumerable<T> interceptors)
            where T : IFluentClientInterceptor
        {
            if (interceptors == null)
                throw new ArgumentNullException(nameof(interceptors));

            Interceptors.AddRange(Interceptors);
            return this;
        }


        /// <summary>
        /// Sets the base URI for requests.
        /// </summary>
        /// <param name="uri">The base URI.</param>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        public FluentClientBuilder BaseUri(string uri)
        {
            var v = new Uri(uri);
            return BaseUri(v);
        }

        /// <summary>
        /// Sets the base URI for requests.
        /// </summary>
        /// <param name="uri">The base URI.</param>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        public FluentClientBuilder BaseUri(Uri uri)
        {
            DefaultRequest.BaseUri = uri;
            return this;
        }


        /// <summary>
        /// Sets a value indicating whether the inner handler should be disposed.
        /// </summary>
        /// <param name="value">
        /// <c>true</c> if the inner handler should be disposed of by the Dispose method,
        /// <c>false</c> if you intend to reuse the inner handler.
        /// </param>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        public FluentClientBuilder DisposeHandler(bool value = true)
        {
            ShouldDisposeHandler = value;
            return this;
        }


        /// <summary>
        /// Set the initial default values for all requests from this instance of <see cref="FluentClient"/>.
        /// </summary>
        /// <param name="builder">The fluent builder factory.</param>
        /// <returns>
        /// A fluent client builder.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />.</exception>
        public FluentClientBuilder Defaults(Action<FormBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var fluentBuilder = new FormBuilder(DefaultRequest);
            builder(fluentBuilder);

            return this;
        }

    }
}
