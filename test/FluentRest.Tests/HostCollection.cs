using Xunit;

namespace FluentRest.Tests;

[CollectionDefinition(CollectionName)]
public class HostCollection : ICollectionFixture<HostFixture>
{
    public const string CollectionName = nameof(HostCollection);
}
