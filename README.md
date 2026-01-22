# Khaos.Pipeline.Abstractions

Core abstractions for composable data transformation pipelines with batch processing support.

## Overview

This package provides the interfaces and core types for defining processing pipelines - linear chains of transformation steps where each step transforms input to output.

**Key characteristics of Pipelines:**
- **Transformation**: Steps transform `TIn` → `TOut`
- **Deterministic**: Every input produces an output (Continue) or explicit abort
- **Batch-aware**: Steps can optionally process entire batches for efficiency
- **Context-based**: Steps share a `PipelineContext` for state

## Key Types

| Type | Description |
|------|-------------|
| `IPipelineStep<TIn, TOut>` | A single transformation step |
| `IBatchAwareStep<TIn, TOut>` | Optional interface for batch processing |
| `IProcessingPipeline<TIn, TOut>` | A composed pipeline |
| `StepOutcome<TOut>` | The result: Continue(value) or Abort() |
| `IPipelineContext` | Shared state interface for pipeline execution |

## Usage

```csharp
// Define a pipeline step
public class ValidateRecordStep : IPipelineStep<RawRecord, ValidatedRecord>
{
    public ValueTask<StepOutcome<ValidatedRecord>> InvokeAsync(
        RawRecord input, 
        IPipelineContext context, 
        CancellationToken ct)
    {
        if (!IsValid(input))
            return ValueTask.FromResult(StepOutcome<ValidatedRecord>.Abort());
            
        return ValueTask.FromResult(
            StepOutcome<ValidatedRecord>.Continue(new ValidatedRecord(input)));
    }
}
```

## When to Use Pipelines vs Flows

| Use Pipelines When | Use Flows When |
|--------------------|----------------|
| Processing messages/records | Need branching logic |
| Linear transformation chain | Orchestrating workflows |
| Every input needs an output | Steps may take different paths |
| Data transformation | State-machine workflows |

## Related Packages

- `KhaosCode.Pipeline` - Implementation of pipeline execution
- `KhaosCode.Flow.Abstractions` - Flow abstractions (complementary pattern)
- `KhaosCode.Flow` - Flow implementation

## License

MIT License - see [LICENSE.md](LICENSE.md)
