[![](https://img.shields.io/nuget/v/soenneker.managers.nugetpackage.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.managers.nugetpackage/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.managers.nugetpackage/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.managers.nugetpackage/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.managers.nugetpackage.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.managers.nugetpackage/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.managers.nugetpackage/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.managers.nugetpackage/actions/workflows/codeql.yml)

# Soenneker.Managers.NuGetPackage

Stages a file or directory into a library's `Resources` folder, builds and packs the project, then pushes the resulting package to NuGet.

## Install

```bash
dotnet add package Soenneker.Managers.NuGetPackage
```

## Usage

```csharp
using Microsoft.Extensions.DependencyInjection;
using Soenneker.Managers.NuGetPackage.Abstract;
using Soenneker.Managers.NuGetPackage.Registrars;

services.AddNuGetPackageManagerAsSingleton();

INuGetPackageManager packages =
    serviceProvider.GetRequiredService<INuGetPackageManager>();

await packages.BuildPackAndPushFile(
    gitDirectory: repositoryPath,
    libraryName: "Soenneker.Libraries.Tool.Windows",
    targetFilePath: Path.Combine(repositoryPath, "src", "Soenneker.Libraries.Tool.Windows", "Resources", "tool.exe"),
    sourceFilePath: downloadedToolPath,
    version: buildVersion,
    nuGetToken: nugetToken,
    cancellationToken);
```

This is a publishing operation, not a package-building preview: a successful call pushes to the configured NuGet source.

## What you get

- `INuGetPackageManager` — Handles building, packaging, and publishing .NET projects to NuGet.
- `NuGetPackageManagerRegistrar` — Handles building, packaging, and publishing .NET projects to NuGet.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `NuGetPackageManagerRegistrar.AddNuGetPackageManagerAsSingleton(services)` | Adds `INuGetPackageManager` as a singleton service. | The same service collection, so additional registrations can be chained. |
| `NuGetPackageManagerRegistrar.AddNuGetPackageManagerAsScoped(services)` | Adds `INuGetPackageManager` as a scoped service. | The same service collection, so additional registrations can be chained. |

## Important behavior

- The target file or directory is deleted and replaced before restore/build/pack begins.
- Targets, the project file, and the expected `.nupkg` must resolve inside `gitDirectory`; resource targets must remain beneath the selected library's `Resources` folder.
- The source file or directory may be outside the checkout and is never deleted by this manager.
- Packages are built in Release configuration and written to the checkout root as `<libraryName>.<version>.nupkg`.
- Keep the NuGet token in a secret provider. The token is passed only to the push operation.
- Cancellation or a failed build can leave the dedicated checkout modified. Use a disposable checkout and clean it up at the workflow boundary.
