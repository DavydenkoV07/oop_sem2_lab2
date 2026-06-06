namespace Core.Algorithms;

public class RedBlackTreeAlgorithm : AlgorithmTemplate
{
    public override string Name => "Red-Black Tree Insertion";
    public override string Complexity => "O(log n)";

    protected override void PerformAlgorithm(int[] data)
    {
        // Тут буде логіка вставки елементів у червоно-чорне дерево
        
        Array.Sort(data); 
    }
}