namespace Khaos.Pipeline.Abstractions;

/// <summary>
/// Fluent builder for creating processing pipelines.
/// </summary>
/// <typeparam name="TIn">The initial input type.</typeparam>
/// <typeparam name="TCurrent">The current type in the chain.</typeparam>
public interface IPipelineBuilder<TIn, TCurrent>
{
    /// <summary>
    /// Adds a step to the pipeline.
    /// </summary>
    /// <typeparam name="TNext">The output type of the step.</typeparam>
    /// <param name="step">The step to add.</param>
    /// <returns>A new builder for the extended pipeline.</returns>
    IPipelineBuilder<TIn, TNext> UseStep<TNext>(IPipelineStep<TCurrent, TNext> step);

    /// <summary>
    /// Builds the pipeline.
    /// </summary>
    /// <returns>The composed pipeline.</returns>
    IProcessingPipeline<TIn, TCurrent> Build();
}
