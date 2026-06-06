namespace Core.Algorithms;

public abstract class AlgorithmTemplate : IAlgorithmStrategy
{
    public abstract string Name { get; }
    public abstract string Complexity { get; }

    public void Execute(int[] data)
    {
        if (data == null || data.Length == 0) return; // Валідація
        PerformAlgorithm(data); // Основний крок
    }

    protected abstract void PerformAlgorithm(int[] data);
}