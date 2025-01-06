using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Managers.NuGetPackage.Abstract;

/// <summary>
/// Handles building, packaging, and publishing .NET projects to NuGet
/// </summary>
public interface INuGetPackageManager
{
    ValueTask BuildPackAndPushExe(string gitDirectory, string targetCsProj, string targetExePath, string filePath, string version, string nuGetToken, CancellationToken cancellationToken = default);
}
