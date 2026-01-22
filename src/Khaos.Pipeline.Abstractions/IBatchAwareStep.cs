namespace Khaos.Pipeline.Abstractions;

/// <summary>
/// Optional interface that enables a step to operate over an entire batch at once.
/// Implement this when batch processing is more efficient than per-record processing.
/// </summary>
/// <typeparam name="TIn">The input type.</typeparam>
/// <typeparam name="TOut">The output type.</typeparam>
public interface IBatchAwareStep<TIn, TOut>
{
    /// <summary>
    /// Processes an entire batch of inputs at once.
    /// </summary>
    /// <param name="inputs">The batch of input records.</param>
    /// <param name="context">Shared context for the pipeline execution.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of outcomes, one per input record.</returns>
    Task<IReadOnlyList<StepOutcome<TOut>>> InvokeBatchAsync(
        IReadOnlyList<TIn> inputs,
        IPipelineContext context,
        CancellationToken cancellationToken);
}
