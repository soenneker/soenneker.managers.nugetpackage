using Soenneker.Managers.NuGetPackage.Abstract;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Logging;
using Soenneker.Utils.Directory.Abstract;
using Soenneker.Utils.Dotnet.Abstract;
using Soenneker.Utils.Dotnet.NuGet.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.File.Abstract;

namespace Soenneker.Managers.NuGetPackage;

public sealed class NuGetPackageManager : INuGetPackageManager
{
    private readonly ILogger<NuGetPackageManager> _logger;
    private readonly IDotnetUtil _dotnetUtil;
    private readonly IDotnetNuGetUtil _dotnetNuGetUtil;
    private readonly IDirectoryUtil _directoryUtil;
    private readonly IFileUtil _fileUtil;

    public NuGetPackageManager(ILogger<NuGetPackageManager> logger, IDotnetUtil dotnetUtil, IDotnetNuGetUtil dotnetNuGetUtil, IDirectoryUtil directoryUtil,
        IFileUtil fileUtil)
    {
        _logger = logger;
        _dotnetUtil = dotnetUtil;
        _dotnetNuGetUtil = dotnetNuGetUtil;
        _directoryUtil = directoryUtil;
        _fileUtil = fileUtil;
    }

    public async ValueTask BuildPackAndPushFile(string gitDirectory, string libraryName, string targetFilePath, string sourceFilePath, string version,
        string nuGetToken, CancellationToken cancellationToken = default)
    {
        string resourcesDir = GetPathWithin(gitDirectory, Path.Combine("src", libraryName, "Resources"), "Resources directory");
        string safeTargetFilePath = GetPathWithin(resourcesDir, targetFilePath, "Target file");

        await _fileUtil.DeleteIfExists(safeTargetFilePath, cancellationToken: cancellationToken)
                       .NoSync();

        await _directoryUtil.Create(resourcesDir, cancellationToken: cancellationToken)
                            .NoSync();

        await _fileUtil.Copy(sourceFilePath, safeTargetFilePath, true, cancellationToken)
                       .NoSync();

        string projFilePath = GetPathWithin(gitDirectory, Path.Combine("src", libraryName, $"{libraryName}.csproj"), "Project file");

        await _dotnetUtil.Restore(projFilePath, cancellationToken: cancellationToken)
                         .NoSync();

        bool successful = await _dotnetUtil.Build(projFilePath, configuration: "Release", cancellationToken: cancellationToken)
                                           .NoSync();

        if (!successful)
            throw new InvalidOperationException("Build was not successful, exiting...");

        await _dotnetUtil.Pack(projFilePath, version, configuration: "Release", restore: false, output: gitDirectory, cancellationToken: cancellationToken)
                         .NoSync();

        string nuGetPackagePath = GetPathWithin(gitDirectory, $"{libraryName}.{version}.nupkg", "NuGet package");

        await _dotnetNuGetUtil.Push(nuGetPackagePath, apiKey: nuGetToken, cancellationToken: cancellationToken)
                              .NoSync();

        _logger.LogInformation("Package pushed to NuGet successfully.");
    }

    public async ValueTask BuildPackAndPushDirectory(string gitDirectory, string libraryName, string targetDirectory, string sourceDirectory, string version,
        string nuGetToken, CancellationToken cancellationToken = default)
    {
        string resourcesDir = GetPathWithin(gitDirectory, Path.Combine("src", libraryName, "Resources"), "Resources directory");
        string safeTargetDirectory = GetPathWithin(resourcesDir, targetDirectory, "Target directory");

        await _directoryUtil.DeleteIfExists(safeTargetDirectory, cancellationToken)
                            .NoSync();
        await _directoryUtil.Create(safeTargetDirectory, cancellationToken: cancellationToken)
                            .NoSync();

        await _fileUtil.CopyRecursively(sourceDirectory, safeTargetDirectory, true, cancellationToken)
                       .NoSync();

        string projFilePath = GetPathWithin(gitDirectory, Path.Combine("src", libraryName, $"{libraryName}.csproj"), "Project file");

        await _dotnetUtil.Restore(projFilePath, cancellationToken: cancellationToken)
                         .NoSync();

        bool successful = await _dotnetUtil.Build(projFilePath, configuration: "Release", cancellationToken: cancellationToken)
                                           .NoSync();

        if (!successful)
            throw new InvalidOperationException("Build was not successful, exiting...");

        await _dotnetUtil.Pack(projFilePath, version, configuration: "Release", restore: false, output: gitDirectory, cancellationToken: cancellationToken)
                         .NoSync();

        string nuGetPackagePath = GetPathWithin(gitDirectory, $"{libraryName}.{version}.nupkg", "NuGet package");

        await _dotnetNuGetUtil.Push(nuGetPackagePath, apiKey: nuGetToken, cancellationToken: cancellationToken)
                              .NoSync();

        _logger.LogInformation("Package pushed to NuGet successfully.");
    }

    private static string GetPathWithin(string rootDirectory, string path, string description)
    {
        string root = Path.GetFullPath(rootDirectory);
        string candidate = Path.GetFullPath(path, root);
        string rootPrefix = Path.TrimEndingDirectorySeparator(root) + Path.DirectorySeparatorChar;

        if (!candidate.StartsWith(rootPrefix, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"{description} must be located within {root}.");

        return candidate;
    }
}
