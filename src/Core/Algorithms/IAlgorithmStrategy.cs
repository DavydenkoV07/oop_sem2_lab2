namespace Core.Algorithms;

public interface IAlgorithmStrategy
{
    string Name { get; }
    string Complexity { get; }
    void Execute(int[] data); 
}