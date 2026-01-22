namespace Khaos.Pipeline.Abstractions;

/// <summary>
/// Outcome classification values for pipeline steps.
/// </summary>
public enum StepOutcomeKind
{
    /// <summary>
    /// The step completed successfully and produced a value to pass downstream.
    /// </summary>
    Continue,

    /// <summary>
    /// The step decided to abort processing for this record.
    /// Downstream steps will be skipped.
    /// </summary>
    Abort
}

/// <summary>
/// Represents the decision returned from a pipeline step.
/// </summary>
/// <typeparam name="TOut">The output type for the step.</typeparam>
public readonly struct StepOutcome<TOut>
{
    private StepOutcome(StepOutcomeKind kind, TOut? value)
    {
        Kind = kind;
        Value = value;
    }

    /// <summary>
    /// Gets the outcome classification.
    /// </summary>
    public StepOutcomeKind Kind { get; }

    /// <summary>
    /// Gets the value produced by the step when <see cref="Kind"/> is <see cref="StepOutcomeKind.Continue"/>.
    /// </summary>
    public TOut? Value { get; }

    /// <summary>
    /// Creates a continue outcome with the provided value.
    /// </summary>
    /// <param name="value">The value to pass to the next step.</param>
    /// <returns>A new continue outcome.</returns>
    public static StepOutcome<TOut> Continue(TOut value) => new(StepOutcomeKind.Continue, value);

    /// <summary>
    /// Creates an abort outcome, signalling downstream steps should be skipped for the current record.
    /// </summary>
    /// <returns>A new abort outcome.</returns>
    public static StepOutcome<TOut> Abort() => new(StepOutcomeKind.Abort, default);

    /// <summary>
    /// Returns whether this outcome represents a continue decision.
    /// </summary>
    public bool IsContinue => Kind == StepOutcomeKind.Continue;

    /// <summary>
    /// Returns whether this outcome represents an abort decision.
    /// </summary>
    public bool IsAbort => Kind == StepOutcomeKind.Abort;
}
