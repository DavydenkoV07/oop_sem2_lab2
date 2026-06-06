using Core.Algorithms;

namespace Core.Patterns;

/// <summary>
/// A class that implements the Factory Method pattern.
/// Centrally creates instances of algorithms based on a text key.
/// Allows you to easily add new algorithms without changing client code.
/// </summary>
public static class AlgorithmFactory
{
    /// <summary>
    /// Factory method for creating an algorithm object.
    /// </summary>
    /// <param name="type">The string identifier of the algorithm (e.g. "rbtree", "btree").</param>
    /// <returns>An instance of an algorithm that implements the IAlgorithmStrategy interface.</returns>
    /// <exception cref="ArgumentException">Thrown if an unknown algorithm type is passed.</exception>
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