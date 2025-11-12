using Xunit;

using XUnit.Hosting;

namespace FluentRest.Tests;

[Collection(HostCollection.CollectionName)]
public abstract class HostTestBase(HostFixture fixture)
    : TestHostBase<HostFixture>(fixture)
{
}
