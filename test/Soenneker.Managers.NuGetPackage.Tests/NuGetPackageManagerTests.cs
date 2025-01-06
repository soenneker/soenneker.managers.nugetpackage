using Soenneker.Managers.NuGetPackage.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Managers.NuGetPackage.Tests;

[Collection("Collection")]
public class NuGetPackageManagerTests : FixturedUnitTest
{
    private readonly INuGetPackageManager _util;

    public NuGetPackageManagerTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<INuGetPackageManager>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
