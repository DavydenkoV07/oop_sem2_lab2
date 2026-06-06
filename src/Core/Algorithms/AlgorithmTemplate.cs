namespace Core.Algorithms;

/// <summary>
/// A base abstract class that implements the Template Method pattern.
/// Defines the general skeleton of algorithm execution with prior data validation.
/// </summary>
public abstract class AlgorithmTemplate : IAlgorithmStrategy
{
    public abstract string Name { get; }
    public abstract string Complexity { get; }

    /// <summary>
    /// A template method that calls validation and then directly the algorithm logic.
    /// </summary>
    /// <param name="data">Input data array.</param>
    public void Execute(int[] data)
    {
        if (data == null || data.Length == 0) return; // Валідація
        PerformAlgorithm(data); // Основний крок
    }

    /// <summary>
    /// An abstract method that specific classes of algorithms must implement.
    /// </summary>
    /// <param name="data">Input data array.</param>
    protected abstract void PerformAlgorithm(int[] data);
}