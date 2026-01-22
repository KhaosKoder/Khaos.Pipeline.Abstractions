namespace Khaos.Pipeline.Abstractions;

/// <summary>
/// Represents a composed processing pipeline that transforms input to output.
/// </summary>
/// <typeparam name="TIn">The input type.</typeparam>
/// <typeparam name="TOut">The output type.</typeparam>
public interface IProcessingPipeline<TIn, TOut>
{
    /// <summary>
    /// Processes a single input record through all pipeline steps.
    /// </summary>
    /// <param name="input">The input record to process.</param>
    /// <param name="context">Shared context for the pipeline execution.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The final outcome after all steps have processed.</returns>
    ValueTask<StepOutcome<TOut>> ProcessAsync(
        TIn input,
        IPipelineContext context,
        CancellationToken cancellationToken);
}
