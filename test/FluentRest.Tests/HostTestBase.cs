using Xunit;
using Xunit.Abstractions;

using XUnit.Hosting;

namespace FluentRest.Tests;

[Collection(HostCollection.CollectionName)]
public abstract class HostTestBase : TestHostBase<HostFixture>
{
    protected HostTestBase(ITestOutputHelper output, HostFixture fixture)
        : base(output, fixture)
    {
    }
}
