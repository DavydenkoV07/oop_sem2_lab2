using System.Text;

namespace Core.Patterns;

public class HtmlReportExporter
{
    private static HtmlReportExporter? _instance;
    private readonly StringBuilder _tableRows;

    // Приватний конструктор — ключова фішка Singleton
    private HtmlReportExporter()
    {
        _tableRows = new StringBuilder();
    }

    public static HtmlReportExporter Instance => _instance ??= new HtmlReportExporter();

    // Додаємо рядок з результатом
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

    // Генеруємо фінальний файл
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