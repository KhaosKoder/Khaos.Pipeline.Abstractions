# Khaos.Pipeline.Abstractions – Developer Guide

This document explains how to extend and maintain the pipeline abstractions package.

## Solution Layout

- `src/Khaos.Pipeline.Abstractions`: Core interfaces and types. Keep the API surface minimal and stable.
- `tests/Khaos.Pipeline.Abstractions.Tests`: xUnit test suite for all public types.
- `scripts/`: PowerShell helper scripts for common workflows.
- `docs/`: Markdown documentation bundled inside the NuGet package.

## Key Types

| Type | Description |
|------|-------------|
| `StepOutcome<TOut>` | Result of a step: Continue(value) or Abort() |
| `StepOutcomeKind` | Enum: Continue, Abort |
| `IPipelineContext` | Shared state for pipeline execution |
| `IPipelineStep<TIn, TOut>` | Single transformation step |
| `IBatchAwareStep<TIn, TOut>` | Optional batch processing optimization |
| `IProcessingPipeline<TIn, TOut>` | Composed pipeline |
| `IBatchPipelineExecutor<TIn, TOut>` | Executes pipelines over batches |
| `PipelineExecutionOptions` | Parallelism and execution settings |

## Coding Guidelines

1. **Stability First**
   - This is an abstractions package—changes affect all consumers.
   - Prefer adding new interfaces over modifying existing ones.
   - Use `[Obsolete]` before removing anything.

2. **Minimal Dependencies**
   - No runtime dependencies beyond the BCL.
   - Implementers should be able to use this without pulling in extra packages.

3. **ValueTask for Hot Paths**
   - `IPipelineStep.InvokeAsync` returns `ValueTask<StepOutcome<TOut>>` for performance.
   - `IProcessingPipeline.ProcessAsync` also returns `ValueTask`.
   - Batch methods return `Task` since they're inherently more heavyweight.

4. **Context Design**
   - `IPipelineContext` uses string keys for flexibility.
   - Implementations should be thread-safe for parallel execution.
   - The `Items` property provides read-only access to all stored data.

5. **StepOutcome as Value Type**
   - `StepOutcome<TOut>` is a `readonly struct` for zero-allocation in hot paths.
   - Factory methods `Continue()` and `Abort()` are the only constructors.

## Testing

- Run tests: `pwsh ./scripts/Test.ps1`
- All public types should have test coverage.
- Test edge cases: null values, empty batches, cancellation.

## Build & Packaging

- `pwsh ./scripts/Build.ps1`: Restore + build in Release.
- `pwsh ./scripts/Clean.ps1`: Remove TestResults, artifacts.
- `pwsh ./scripts/Pack.ps1`: Create NuGet package.
- Uses **MinVer** with prefix `Khaos.Pipeline.Abstractions/v`.

## Versioning

- Follow SemVer strictly:
  - **Major**: Breaking changes to interfaces.
  - **Minor**: New interfaces or methods.
  - **Patch**: Documentation or non-breaking fixes.
- See `docs/versioning-guide.md` for tagging workflow.

## Related Packages

- **KhaosCode.Pipeline**: Default implementation.
- **KhaosCode.Flow.Abstractions**: Flow interfaces (can be combined).
- **KhaosCode.Processing.Pipelines**: Extended implementation with instrumentation.
