using Core.Patterns;

namespace App.UI;

public class SaveReportCommand : ICommand
{
    public string Description => "Зберегти HTML-звіт та відкрити";

    public void Execute()
    {
        string filePath = "PerformanceReport.html";
        HtmlReportExporter.Instance.SaveToFile(filePath);
        
        Console.WriteLine($"\n[Успіх] Звіт збережено у файл: {filePath}");
        Console.WriteLine("Відкрийте його у будь-якому браузері.");
    }
}