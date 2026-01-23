# Khaos.Pipeline.Abstractions – Versioning Guide

This document describes how versions are managed for the Pipeline Abstractions package.

## Version Strategy

This package uses **MinVer** for automatic semantic versioning based on Git tags.

### Configuration

In `Directory.Build.props`:

```xml
<MinVerTagPrefix>Khaos.Pipeline.Abstractions/v</MinVerTagPrefix>
<MinVerDefaultPreReleaseIdentifiers>alpha.0</MinVerDefaultPreReleaseIdentifiers>
```

### Tag Format

Tags follow the pattern: `Khaos.Pipeline.Abstractions/vX.Y.Z`

Examples:
- `Khaos.Pipeline.Abstractions/v1.0.0` → Version 1.0.0
- `Khaos.Pipeline.Abstractions/v1.1.0` → Version 1.1.0
- `Khaos.Pipeline.Abstractions/v2.0.0` → Version 2.0.0 (breaking change)

## Semantic Versioning for Abstractions

### Major Version (X.0.0)
- Breaking changes to existing interfaces
- Removing interface members
- Changing method signatures
- Changing `StepOutcome<T>` structure

### Minor Version (0.X.0)
- New interfaces added
- New methods on `IPipelineContext`
- New properties on `PipelineExecutionOptions`

### Patch Version (0.0.X)
- Documentation updates
- XML comment improvements
- Non-breaking bug fixes

## Release Workflow

### 1. Check Current Version

```powershell
cd Khaos.Pipeline.Abstractions
.\scripts\Get-Version.ps1
```

### 2. Create Release Tag

```powershell
# For a new minor release
git tag Khaos.Pipeline.Abstractions/v1.1.0
git push origin Khaos.Pipeline.Abstractions/v1.1.0
```

### 3. Build and Pack

```powershell
.\scripts\Build.ps1
.\scripts\Pack.ps1
```

### 4. Publish

```powershell
dotnet nuget push artifacts/*.nupkg --source nuget.org --api-key YOUR_KEY
```

## Pre-release Versions

Between tags, MinVer generates pre-release versions:
- After `v1.0.0`: `1.0.1-alpha.0.{commits}`

## Coordination with Implementations

When changing abstractions:

1. **Update abstractions first** – Tag and release new version.
2. **Update implementations** – KhaosCode.Pipeline, KhaosCode.Processing.Pipelines.
3. **Update consumers** – Kafka.Consumer and other dependents.

## Guidelines

1. **Never manually set Version** in project files
2. **Tag releases** from the main branch only
3. **Document breaking changes** in release notes
4. **Test with all implementations** before releasing major versions
