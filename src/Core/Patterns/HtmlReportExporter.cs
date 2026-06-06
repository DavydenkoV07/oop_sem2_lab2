using System.Text;

namespace Core.Patterns;

/// <summary>
/// A class that implements the Singleton pattern.
/// Responsible for accumulating test results and generating the final HTML report.
/// Guarantees that there is only one instance of the report generator in the system.
/// </summary>
public class HtmlReportExporter
{
    private static HtmlReportExporter? _instance;
    private readonly StringBuilder _tableRows;

    
    private HtmlReportExporter()
    {
        _tableRows = new StringBuilder();
    }

    public static HtmlReportExporter Instance => _instance ??= new HtmlReportExporter();

    /// <summary>
    /// Adds a new line with the results of the algorithm execution to a future report.
    /// </summary>
    /// <param name="algorithmName">Name of the algorithm.</param>
    /// <param name="elementsCount">Number of elements in array</param>
    /// <param name="complexity">Theoretical complexity</param>
    /// <param name="timeMs">Execution time in milliseconds.</param>
    public void AddResult(string algorithmName, int elementsCount, string complexity, long timeMs)
    {
        _tableRows.AppendLine($@"
            <tr>
                <td>{algorithmName}</td>
                <td>{elementsCount}</td>
                <td>{complexity}</td>
                <td>{timeMs} мс</td>
            </tr>");
    }

    /// <summary>
    /// Generates an HTML document using Bootstrap 5 and saves it to a file.
    /// </summary>
    /// <param name="filePath">The path to the file where the report will be saved.</param>
    public void SaveToFile(string filePath)
    {
        string html = $@"
<!DOCTYPE html>
<html lang='uk'>
<head>
    <meta charset='UTF-8'>
    <title>Звіт продуктивності алгоритмів</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body class='bg-light'>
    <div class='container mt-5'>
        <h2 class='mb-4'>Результати тестування патернів та алгоритмів</h2>
        <table class='table table-striped table-bordered shadow-sm'>
            <thead class='table-dark'>
                <tr>
                    <th>Алгоритм</th>
                    <th>Кількість елементів</th>
                    <th>Складність</th>
                    <th>Час виконання</th>
                </tr>
            </thead>
            <tbody>
                {_tableRows}
            </tbody>
        </table>
    </div>
</body>
</html>";
        File.WriteAllText(filePath, html);
    }
}