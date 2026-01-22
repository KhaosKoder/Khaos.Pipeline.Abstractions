namespace Khaos.Pipeline.Abstractions;

/// <summary>
/// Executes a processing pipeline over batches of records.
/// </summary>
/// <typeparam name="TIn">The input type.</typeparam>
/// <typeparam name="TOut">The output type.</typeparam>
public interface IBatchPipelineExecutor<TIn, TOut>
{
    /// <summary>
    /// Processes a batch of records through the pipeline.
    /// </summary>
    /// <param name="inputs">The batch of input records.</param>
    /// <param name="pipeline">The pipeline to execute.</param>
    /// <param name="context">Shared context for the pipeline execution.</param>
    /// <param name="options">Execution options (parallelism, etc.).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the batch processing operation.</returns>
    Task ProcessBatchAsync(
        IReadOnlyList<TIn> inputs,
        IProcessingPipeline<TIn, TOut> pipeline,
        IPipelineContext context,
        PipelineExecutionOptions options,
        CancellationToken cancellationToken);
}

/// <summary>
/// Options for controlling pipeline execution.
/// </summary>
public class PipelineExecutionOptions
{
    /// <summary>
    /// Gets or sets whether to process records sequentially.
    /// </summary>
    public bool IsSequential { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of records to process in parallel.
    /// Only applies when <see cref="IsSequential"/> is false.
    /// </summary>
    public int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount;
}
