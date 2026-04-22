using Soenneker.Managers.NuGetPackage.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Managers.NuGetPackage.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class NuGetPackageManagerTests : HostedUnitTest
{
    private readonly INuGetPackageManager _util;

    public NuGetPackageManagerTests(Host host) : base(host)
    {
        _util = Resolve<INuGetPackageManager>(true);
    }

    [Test]
    public void Default()
    {

    }
}
