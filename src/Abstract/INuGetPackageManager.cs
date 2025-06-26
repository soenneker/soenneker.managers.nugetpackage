using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Managers.NuGetPackage.Abstract;

/// <summary>
/// Handles building, packaging, and publishing .NET projects to NuGet
/// </summary>
public interface INuGetPackageManager
{
    ValueTask BuildPackAndPushFile(string gitDirectory, string libraryName, string targetFilePath, string sourceFilePath, string version, string nuGetToken,
        CancellationToken cancellationToken = default);

    ValueTask BuildPackAndPushDirectory(string gitDirectory, string libraryName, string targetDirectory, string sourceDirectory, string version,
        string nuGetToken, CancellationToken cancellationToken = default);
}