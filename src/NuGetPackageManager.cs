using Soenneker.Managers.NuGetPackage.Abstract;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Logging;
using Soenneker.Utils.Directory.Abstract;
using Soenneker.Utils.Dotnet.Abstract;
using Soenneker.Utils.Dotnet.NuGet.Abstract;
using Soenneker.Utils.FileSync.Abstract;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Managers.NuGetPackage;

/// <inheritdoc cref="INuGetPackageManager"/>
public class NuGetPackageManager : INuGetPackageManager
{
    private readonly ILogger<NuGetPackageManager> _logger;
    private readonly IDotnetUtil _dotnetUtil;
    private readonly IDotnetNuGetUtil _dotnetNuGetUtil;
    private readonly IDirectoryUtil _directoryUtil;
    private readonly IFileUtilSync _fileUtilSync;

    public NuGetPackageManager(ILogger<NuGetPackageManager> logger, IDotnetUtil dotnetUtil, IDotnetNuGetUtil dotnetNuGetUtil, IDirectoryUtil directoryUtil, IFileUtilSync fileUtilSync)
    {
        _logger = logger;
        _dotnetUtil = dotnetUtil;
        _dotnetNuGetUtil = dotnetNuGetUtil;
        _directoryUtil = directoryUtil;
        _fileUtilSync = fileUtilSync;
    }

    public async ValueTask BuildPackAndPushExe(string gitDirectory, string targetCsProj, string targetExePath, string filePath, string version, string nuGetToken, CancellationToken cancellationToken)
    {
        // Delete if old file exists
        _fileUtilSync.DeleteIfExists(targetExePath);

        // Ensure resource folder is created
        string resourcesDir = Path.Combine(gitDirectory, "src", "Resources");
        _directoryUtil.CreateIfDoesNotExist(resourcesDir);

        // Move new file
        _fileUtilSync.Move(filePath, targetExePath);

        // Build .csproj path
        string projFilePath = Path.Combine(gitDirectory, "src", $"{targetCsProj}.csproj");

        // Dotnet restore
        await _dotnetUtil.Restore(projFilePath, cancellationToken: cancellationToken);

        // Dotnet build
        bool successful = await _dotnetUtil.Build(
            projFilePath,
            configuration: "Release",
            cancellationToken: cancellationToken);

        if (!successful)
            throw new Exception("Build was not successful, exiting...");

        // Dotnet pack
        await _dotnetUtil.Pack(
            projFilePath,
            version,
            configuration: "Release",
            restore: false,
            output: gitDirectory,
            cancellationToken: cancellationToken).NoSync();

        // Build .nupkg file path
        string nuGetPackagePath = Path.Combine(gitDirectory, $"{targetCsProj}.{version}.nupkg");

        // Push package
        await _dotnetNuGetUtil.Push(nuGetPackagePath, apiKey: nuGetToken, cancellationToken: cancellationToken).NoSync();

        _logger.LogInformation("Package pushed to NuGet successfully.");
    }
}