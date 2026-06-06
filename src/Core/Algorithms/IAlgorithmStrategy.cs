namespace Core.Algorithms;

/// <summary>
/// An interface for implementing the Strategy pattern.
/// Provides a single contract for all algorithms in the system.
/// 
/// </summary>
public interface IAlgorithmStrategy
{
    /// <summary>
    /// Algorithm name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Theoretical complexity of the algorithm
    /// </summary>
    string Complexity { get; }

    /// <summary>
    /// Executes an algorithm on the passed data array.
    /// </summary>
    /// <param name="data">An array of integers to process.</param>
    void Execute(int[] data); 
}