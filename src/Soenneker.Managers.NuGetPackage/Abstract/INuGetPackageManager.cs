using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Managers.NuGetPackage.Abstract;

/// <summary>
/// Handles building, packaging, and publishing .NET projects to NuGet
/// </summary>
public interface INuGetPackageManager
{
    /// <summary>
    /// Builds pack and push file.
    /// </summary>
    /// <param name="gitDirectory">The git directory.</param>
    /// <param name="libraryName">The library name.</param>
    /// <param name="targetFilePath">The target file path.</param>
    /// <param name="sourceFilePath">The source file path.</param>
    /// <param name="version">The version.</param>
    /// <param name="nuGetToken">The nu get token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask BuildPackAndPushFile(string gitDirectory, string libraryName, string targetFilePath, string sourceFilePath, string version, string nuGetToken,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Builds pack and push directory.
    /// </summary>
    /// <param name="gitDirectory">The git directory.</param>
    /// <param name="libraryName">The library name.</param>
    /// <param name="targetDirectory">The target directory.</param>
    /// <param name="sourceDirectory">The source directory.</param>
    /// <param name="version">The version.</param>
    /// <param name="nuGetToken">The nu get token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask BuildPackAndPushDirectory(string gitDirectory, string libraryName, string targetDirectory, string sourceDirectory, string version,
        string nuGetToken, CancellationToken cancellationToken = default);
}