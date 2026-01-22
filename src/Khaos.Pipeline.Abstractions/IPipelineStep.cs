namespace Khaos.Pipeline.Abstractions;

/// <summary>
/// Represents a per-record pipeline step that transforms input to output.
/// </summary>
/// <typeparam name="TIn">The input type.</typeparam>
/// <typeparam name="TOut">The output type.</typeparam>
public interface IPipelineStep<TIn, TOut>
{
    /// <summary>
    /// Processes a single input record.
    /// </summary>
    /// <param name="input">The input record to process.</param>
    /// <param name="context">Shared context for the pipeline execution.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The outcome indicating whether to continue or abort.</returns>
    ValueTask<StepOutcome<TOut>> InvokeAsync(
        TIn input,
        IPipelineContext context,
        CancellationToken cancellationToken);
}
