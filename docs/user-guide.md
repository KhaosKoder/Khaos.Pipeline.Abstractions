# Khaos.Pipeline.Abstractions – User Guide

This package provides the core interfaces for building high-performance data transformation pipelines in .NET.

## Installation

```bash
dotnet add package KhaosCode.Pipeline.Abstractions
```

## Overview

Pipelines are linear chains of transformation steps where each step transforms input to output. They're optimized for batch processing with support for parallelism.

### Key Concepts

| Concept | Description |
|---------|-------------|
| **StepOutcome** | Result of a step: Continue(value) or Abort() |
| **Pipeline Step** | A transformation: TIn → TOut |
| **Pipeline Context** | Shared state across all steps |
| **Batch Executor** | Processes multiple records efficiently |

## Core Types

### StepOutcome&lt;TOut&gt;

The result of processing a record:

```csharp
// Continue processing with a transformed value
return StepOutcome<Order>.Continue(processedOrder);

// Abort processing for this record (skip remaining steps)
return StepOutcome<Order>.Abort();
```

### IPipelineContext

Shared state for pipeline execution:

```csharp
public interface IPipelineContext
{
    IReadOnlyDictionary<string, object?> Items { get; }
    
    T Get<T>(string key);
    bool TryGet<T>(string key, out T? value);
    void Set(string key, object? value);
    bool Contains(string key);
    bool Remove(string key);
    void Clear();
}
```

### IPipelineStep&lt;TIn, TOut&gt;

A single transformation step:

```csharp
public interface IPipelineStep<TIn, TOut>
{
    ValueTask<StepOutcome<TOut>> InvokeAsync(
        TIn input,
        IPipelineContext context,
        CancellationToken cancellationToken);
}

// Example
public class ValidateOrderStep : IPipelineStep<RawOrder, RawOrder>
{
    public ValueTask<StepOutcome<RawOrder>> InvokeAsync(
        RawOrder input, IPipelineContext context, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(input.CustomerId))
            return ValueTask.FromResult(StepOutcome<RawOrder>.Abort());
            
        return ValueTask.FromResult(StepOutcome<RawOrder>.Continue(input));
    }
}
```

### IBatchAwareStep&lt;TIn, TOut&gt;

Optional interface for batch-optimized processing:

```csharp
public interface IBatchAwareStep<TIn, TOut>
{
    Task<IReadOnlyList<StepOutcome<TOut>>> InvokeBatchAsync(
        IReadOnlyList<TIn> inputs,
        IPipelineContext context,
        CancellationToken cancellationToken);
}

// Example: Bulk database insert
public class BulkInsertStep : IPipelineStep<Order, Order>, IBatchAwareStep<Order, Order>
{
    public ValueTask<StepOutcome<Order>> InvokeAsync(...)
        => ValueTask.FromResult(StepOutcome<Order>.Continue(input));
    
    public async Task<IReadOnlyList<StepOutcome<Order>>> InvokeBatchAsync(
        IReadOnlyList<Order> inputs, IPipelineContext context, CancellationToken ct)
    {
        await _db.BulkInsertAsync(inputs, ct);
        return inputs.Select(o => StepOutcome<Order>.Continue(o)).ToList();
    }
}
```

### IProcessingPipeline&lt;TIn, TOut&gt;

A composed pipeline:

```csharp
public interface IProcessingPipeline<TIn, TOut>
{
    ValueTask<StepOutcome<TOut>> ProcessAsync(
        TIn input,
        IPipelineContext context,
        CancellationToken cancellationToken);
}
```

### IBatchPipelineExecutor&lt;TIn, TOut&gt;

Executes pipelines over batches:

```csharp
public interface IBatchPipelineExecutor<TIn, TOut>
{
    Task ProcessBatchAsync(
        IReadOnlyList<TIn> inputs,
        IProcessingPipeline<TIn, TOut> pipeline,
        IPipelineContext context,
        PipelineExecutionOptions options,
        CancellationToken cancellationToken);
}
```

### PipelineExecutionOptions

Controls batch execution:

```csharp
public class PipelineExecutionOptions
{
    // Process records one at a time
    public bool IsSequential { get; set; }
    
    // Max parallel records (when IsSequential = false)
    public int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount;
}
```

## Usage with Context

```csharp
// Store batch metadata
context.Set("BatchId", Guid.NewGuid());
context.Set("Source", "kafka-topic-orders");

// Access in steps
public ValueTask<StepOutcome<TOut>> InvokeAsync(
    TIn input, IPipelineContext context, CancellationToken ct)
{
    var batchId = context.Get<Guid>("BatchId");
    
    if (context.TryGet<string>("Source", out var source))
    {
        _logger.Log($"Processing from {source}");
    }
    
    return StepOutcome<TOut>.Continue(result);
}
```

## Best Practices

1. **Keep steps small** – Each step should do one transformation.
2. **Use Abort wisely** – Abort skips remaining steps for that record only.
3. **Implement IBatchAwareStep** – When your step can benefit from batching (DB, HTTP).
4. **Handle cancellation** – Always respect the `CancellationToken`.

## Related Packages

- **KhaosCode.Pipeline** – Default implementation.
- **KhaosCode.Flow.Abstractions** – For branching workflows.
