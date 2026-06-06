using Core.Patterns;
using Core.Algorithms;

namespace App.UI;

public class RunAlgorithmCommand : ICommand
{
    private readonly string _algorithmType;
    private readonly int _dataSize;
    public string Description { get; }

    public RunAlgorithmCommand(string algorithmType, int dataSize, string description)
    {
        _algorithmType = algorithmType;
        _dataSize = dataSize;
        Description = description;
    }

    public void Execute()
    {
        Console.WriteLine($"\n> Виконується: {Description} (Елементів: {_dataSize})...");
        
        // Генеруємо випадкові дані
        int[] data = GenerateRandomData(_dataSize);
        
        // Використовуємо Фабрику та Декоратор
        IAlgorithmStrategy algo = AlgorithmFactory.Create(_algorithmType);
        TimerDecorator timer = new TimerDecorator(algo);
        
        timer.Execute(data);
        
        Console.WriteLine($"[Успіх] Час виконання: {timer.LastExecutionTimeMs} мс. Складність: {timer.Complexity}");
        
        // Записуємо результат у Singleton
        HtmlReportExporter.Instance.AddResult(timer.Name, _dataSize, timer.Complexity, timer.LastExecutionTimeMs);
    }

    private int[] GenerateRandomData(int size)
    {
        Random rnd = new Random();
        return Enumerable.Range(0, size).Select(_ => rnd.Next(1, 10000)).ToArray();
    }
}