using System;

namespace FluentRest.Fake;

/// <summary>
/// A <see langword="base"/> fluent builder for a <see cref="FakeResponseContainer"/>.
/// </summary>
/// <typeparam name="TBuilder">The type of the builder.</typeparam>
public abstract class FakeContainerBuilder<TBuilder>
    where TBuilder : FakeContainerBuilder<TBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FakeContainerBuilder{TBuilder}"/> class.
    /// </summary>
    /// <param name="container">The container to build.</param>
    protected FakeContainerBuilder(FakeResponseContainer container)
    {
        Container = container;
    }

    /// <summary>
    /// Gets the fake response container.
    /// </summary>
    /// <value>
    /// The fake response container.
    /// </value>
    public FakeResponseContainer Container { get; }
}

/// <summary>
/// A fluent builder for a <see cref="FakeResponseContainer"/>.
/// </summary>
public class FakeContainerBuilder : FakeContainerBuilder<FakeContainerBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FakeContainerBuilder"/> class.
    /// </summary>
    /// <param name="container">The container to build.</param>
    public FakeContainerBuilder(FakeResponseContainer container) : base(container)
    {

    }

    /// <summary>
    /// Sets the URL that the current fake response is for.
    /// </summary>
    /// <param name="value">The URL the fake response if for.</param>
    /// <returns>A fluent fake response builder.</returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public FakeResponseBuilder Url(string value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        Container.RequestUri = new Uri(value, UriKind.Absolute);

        return new FakeResponseBuilder(Container);
    }

    /// <summary>
    /// Sets the URL that the current fake response is for.
    /// </summary>
    /// <param name="value">The URL the fake response if for.</param>
    /// <returns>A fluent fake response builder.</returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
    public FakeResponseBuilder Url(Uri value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        Container.RequestUri = value;

        return new FakeResponseBuilder(Container);
    }
}
