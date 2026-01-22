using Khaos.Pipeline.Abstractions;

namespace Khaos.Pipeline.Abstractions.Tests;

public class PipelineExecutionOptionsTests
{
    [Fact]
    public void DefaultValues_AreCorrect()
    {
        var options = new PipelineExecutionOptions();
        
        Assert.False(options.IsSequential);
        Assert.Equal(Environment.ProcessorCount, options.MaxDegreeOfParallelism);
    }

    [Fact]
    public void CanSetIsSequential()
    {
        var options = new PipelineExecutionOptions { IsSequential = true };
        
        Assert.True(options.IsSequential);
    }

    [Fact]
    public void CanSetMaxDegreeOfParallelism()
    {
        var options = new PipelineExecutionOptions { MaxDegreeOfParallelism = 4 };
        
        Assert.Equal(4, options.MaxDegreeOfParallelism);
    }
}
