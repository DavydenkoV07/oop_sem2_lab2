using Core.Algorithms;

namespace Core.Patterns;

public static class AlgorithmFactory
{
    public static IAlgorithmStrategy Create(string type)
    {
        return type.ToLower() switch
        {
            "rbtree" => new RedBlackTreeAlgorithm(),
            "btree" => new BTreeAlgorithm(),
            "multiway" => new MultiwayMergeSortAlgorithm(),
            _ => throw new ArgumentException("Unknown algorithm type")
        };
    }
}