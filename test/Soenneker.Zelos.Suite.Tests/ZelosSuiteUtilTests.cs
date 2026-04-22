using Soenneker.Tests.HostedUnit;

namespace Soenneker.Zelos.Suite.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class ZelosSuiteUtilTests : HostedUnitTest
{
    public ZelosSuiteUtilTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}
