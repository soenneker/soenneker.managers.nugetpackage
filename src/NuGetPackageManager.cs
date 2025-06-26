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
using Soenneker.Utils.File.Abstract;

namespace Soenneker.Managers.NuGetPackage;

/// <inheritdoc cref="INuGetPackageManager"/>
public sealed class NuGetPackageManager : INuGetPackageManager
{
    private readonly ILogger<NuGetPackageManager> _logger;
    private readonly IDotnetUtil _dotnetUtil;
    private readonly IDotnetNuGetUtil _dotnetNuGetUtil;
    private readonly IDirectoryUtil _directoryUtil;
    private readonly IFileUtilSync _fileUtilSync;
    private readonly IFileUtil _fileUtil;

    public NuGetPackageManager(ILogger<NuGetPackageManager> logger, IDotnetUtil dotnetUtil, IDotnetNuGetUtil dotnetNuGetUtil, IDirectoryUtil directoryUtil,
        IFileUtilSync fileUtilSync, IFileUtil fileUtil)
    {
        _logger = logger;
        _dotnetUtil = dotnetUtil;
        _dotnetNuGetUtil = dotnetNuGetUtil;
        _directoryUtil = directoryUtil;
        _fileUtilSync = fileUtilSync;
        _fileUtil = fileUtil;
    }

    public async ValueTask BuildPackAndPushFile(string gitDirectory, string libraryName, string targetFilePath, string sourceFilePath, string version,
        string nuGetToken, CancellationToken cancellationToken = default)
    {
        _fileUtilSync.DeleteIfExists(targetFilePath);

        string resourcesDir = Path.Combine(gitDirectory, "src", "Resources");
        _directoryUtil.CreateIfDoesNotExist(resourcesDir);

        await _fileUtil.Copy(sourceFilePath, targetFilePath, true, cancellationToken).NoSync();

        string projFilePath = Path.Combine(gitDirectory, "src", $"{libraryName}.csproj");

        await _dotnetUtil.Restore(projFilePath, cancellationToken: cancellationToken).NoSync();

        bool successful = await _dotnetUtil.Build(projFilePath, configuration: "Release", cancellationToken: cancellationToken).NoSync();

        if (!successful)
            throw new Exception("Build was not successful, exiting...");

        await _dotnetUtil.Pack(projFilePath, version, configuration: "Release", restore: false, output: gitDirectory, cancellationToken: cancellationToken)
                         .NoSync();

        string nuGetPackagePath = Path.Combine(gitDirectory, $"{libraryName}.{version}.nupkg");

        await _dotnetNuGetUtil.Push(nuGetPackagePath, apiKey: nuGetToken, cancellationToken: cancellationToken).NoSync();

        _logger.LogInformation("Package pushed to NuGet successfully.");
    }

    public async ValueTask BuildPackAndPushDirectory(string gitDirectory, string libraryName, string targetDirectory, string sourceDirectory, string version,
        string nuGetToken, CancellationToken cancellationToken = default)
    {
        _directoryUtil.DeleteIfExists(targetDirectory);
        _directoryUtil.CreateIfDoesNotExist(targetDirectory);

        await _fileUtil.Copy(sourceDirectory, targetDirectory, true, cancellationToken).NoSync();

        string projFilePath = Path.Combine(gitDirectory, "src", $"{libraryName}.csproj");

        await _dotnetUtil.Restore(projFilePath, cancellationToken: cancellationToken).NoSync();

        bool successful = await _dotnetUtil.Build(projFilePath, configuration: "Release", cancellationToken: cancellationToken).NoSync();

        if (!successful)
            throw new Exception("Build was not successful, exiting...");

        await _dotnetUtil.Pack(projFilePath, version, configuration: "Release", restore: false, output: gitDirectory, cancellationToken: cancellationToken)
                         .NoSync();

        string nuGetPackagePath = Path.Combine(gitDirectory, $"{libraryName}.{version}.nupkg");

        await _dotnetNuGetUtil.Push(nuGetPackagePath, apiKey: nuGetToken, cancellationToken: cancellationToken).NoSync();

        _logger.LogInformation("Package pushed to NuGet successfully.");
    }
}