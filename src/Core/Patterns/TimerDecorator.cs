using System.Diagnostics;
using Core.Algorithms;

namespace Core.Patterns;

/// <summary>
/// /// A class that implements the Decorator pattern.
/// Wraps an algorithm to measure its practical execution time,
/// without changing the source code of the algorithm itself.
/// </summary>
public class TimerDecorator : IAlgorithmStrategy
{
    private readonly IAlgorithmStrategy _algorithm;
    public long LastExecutionTimeMs { get; private set; }

    public TimerDecorator(IAlgorithmStrategy algorithm)
    {
        _algorithm = algorithm;
    }

    public string Name => _algorithm.Name;
    public string Complexity => _algorithm.Complexity;

    public void Execute(int[] data)
    {
        var stopwatch = Stopwatch.StartNew();
        _algorithm.Execute(data);
        stopwatch.Stop();
        LastExecutionTimeMs = stopwatch.ElapsedMilliseconds;
    }
}