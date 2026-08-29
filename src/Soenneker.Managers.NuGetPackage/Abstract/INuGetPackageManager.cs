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
    /// <param name="gitDirectory">Git repository directory to inspect or update.</param>
    /// <param name="libraryName">Name of the library to load.</param>
    /// <param name="targetFilePath">Path of the target file to use.</param>
    /// <param name="sourceFilePath">Path of the source file to use.</param>
    /// <param name="version">Version for the build pack and push file operation.</param>
    /// <param name="nuGetToken">Nu Get Token for the build pack and push file operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the build pack and push file operation is complete.</returns>
    ValueTask BuildPackAndPushFile(string gitDirectory, string libraryName, string targetFilePath, string sourceFilePath, string version, string nuGetToken,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Builds pack and push directory.
    /// </summary>
    /// <param name="gitDirectory">Git repository directory to inspect or update.</param>
    /// <param name="libraryName">Name of the library to load.</param>
    /// <param name="targetDirectory">Target Directory for the build pack and push directory operation.</param>
    /// <param name="sourceDirectory">source Directory to read or transform.</param>
    /// <param name="version">Version for the build pack and push directory operation.</param>
    /// <param name="nuGetToken">Nu Get Token for the build pack and push directory operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the build pack and push directory operation is complete.</returns>
    ValueTask BuildPackAndPushDirectory(string gitDirectory, string libraryName, string targetDirectory, string sourceDirectory, string version,
        string nuGetToken, CancellationToken cancellationToken = default);
}
