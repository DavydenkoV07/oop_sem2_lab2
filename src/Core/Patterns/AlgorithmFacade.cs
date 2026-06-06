using Core.Algorithms;
using System;

namespace Core.Patterns;

/// <summary>
/// A class that implements the Facade pattern.
/// Provides a simple, single interface for a complex subsystem,
/// hiding the interaction logic of Factory, Decorator, and Singleton.
/// </summary>
public class AlgorithmFacade
{
    public void RunAndLogAlgorithm(string algorithmType, int[] data)
    {
        // 1. Створюємо алгоритм через фабрику
        IAlgorithmStrategy algo = AlgorithmFactory.Create(algorithmType);
        
        // 2. Обгортаємо таймером
        TimerDecorator timer = new TimerDecorator(algo);
        
        // 3. Виконуємо алгоритм
        timer.Execute(data);
        
        // 4. Логуємо результат у Singleton
        HtmlReportExporter.Instance.AddResult(timer.Name, data.Length, timer.Complexity, timer.LastExecutionTimeMs);
        
        Console.WriteLine($"[Успіх] Час виконання: {timer.LastExecutionTimeMs} мс. Складність: {timer.Complexity}");
    }
}