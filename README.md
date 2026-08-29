[![](https://img.shields.io/nuget/v/soenneker.managers.nugetpackage.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.managers.nugetpackage/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.managers.nugetpackage/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.managers.nugetpackage/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.managers.nugetpackage.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.managers.nugetpackage/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.managers.nugetpackage/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.managers.nugetpackage/actions/workflows/codeql.yml)

# Soenneker.Managers.NuGetPackage

Handles building, packaging, and publishing .NET projects to NuGet.

## Install

```bash
dotnet add package Soenneker.Managers.NuGetPackage
```

## Quick start

```csharp
using Soenneker.Managers.NuGetPackage.Registrars;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var result = services.AddNuGetPackageManagerAsSingleton();
```

Adds `INuGetPackageManager` as a singleton service.

## What you get

- `INuGetPackageManager` — Handles building, packaging, and publishing .NET projects to NuGet.
- `NuGetPackageManagerRegistrar` — Handles building, packaging, and publishing .NET projects to NuGet.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `NuGetPackageManagerRegistrar.AddNuGetPackageManagerAsSingleton(services)` | Adds `INuGetPackageManager` as a singleton service. | The same service collection, so additional registrations can be chained. |
| `NuGetPackageManagerRegistrar.AddNuGetPackageManagerAsScoped(services)` | Adds `INuGetPackageManager` as a scoped service. | The same service collection, so additional registrations can be chained. |
