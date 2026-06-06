namespace Core.Algorithms;

public class BTreeAlgorithm : AlgorithmTemplate
{
    public override string Name => "B-Tree Insertion";
    public override string Complexity => "O(log n)";

    protected override void PerformAlgorithm(int[] data)
    {
        // Тут буде логіка вставки елементів у червоно-чорне дерево
        
        Array.Sort(data); 
    }
}