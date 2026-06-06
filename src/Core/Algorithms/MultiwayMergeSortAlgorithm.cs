namespace Core.Algorithms;

public class MultiwayMergeSortAlgorithm : AlgorithmTemplate
{
    public override string Name => "Multiway Merge Sort";
    public override string Complexity => "O(log n)";

    protected override void PerformAlgorithm(int[] data)
    {
        // Тут буде логіка вставки елементів у червоно-чорне дерево
        
        Array.Sort(data); 
    }
}