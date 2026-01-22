using Khaos.Pipeline.Abstractions;

namespace Khaos.Pipeline.Abstractions.Tests;

public class StepOutcomeTests
{
    [Fact]
    public void Continue_ReturnsCorrectKindAndValue()
    {
        var outcome = StepOutcome<string>.Continue("test");
        
        Assert.Equal(StepOutcomeKind.Continue, outcome.Kind);
        Assert.Equal("test", outcome.Value);
        Assert.True(outcome.IsContinue);
        Assert.False(outcome.IsAbort);
    }

    [Fact]
    public void Abort_ReturnsCorrectKind()
    {
        var outcome = StepOutcome<string>.Abort();
        
        Assert.Equal(StepOutcomeKind.Abort, outcome.Kind);
        Assert.Null(outcome.Value);
        Assert.False(outcome.IsContinue);
        Assert.True(outcome.IsAbort);
    }

    [Fact]
    public void Continue_WithNullValue_IsAllowed()
    {
        var outcome = StepOutcome<string?>.Continue(null);
        
        Assert.Equal(StepOutcomeKind.Continue, outcome.Kind);
        Assert.Null(outcome.Value);
        Assert.True(outcome.IsContinue);
    }

    [Fact]
    public void Continue_WithValueType_ReturnsValue()
    {
        var outcome = StepOutcome<int>.Continue(42);
        
        Assert.Equal(StepOutcomeKind.Continue, outcome.Kind);
        Assert.Equal(42, outcome.Value);
    }
}
