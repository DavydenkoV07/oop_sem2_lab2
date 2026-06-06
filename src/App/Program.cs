using App.UI;

class Program
{
    static void Main()
    {
        // Це наше "меню" (імітація кнопок інтерфейсу)
        var menu = new Dictionary<string, ICommand>
        {
            { "1", new RunAlgorithmCommand("rbtree", 50000, "Вставка в Red-Black Tree") },
            { "2", new RunAlgorithmCommand("btree", 50000, "Вставка в B-Tree") },
            { "3", new RunAlgorithmCommand("multiway", 50000, "Multiway Merge Sort") },
            // Сюди додамо команду збереження HTML
        };

        while (true)
        {
            Console.WriteLine("\n=== СИСТЕМА АНАЛІЗУ АЛГОРИТМІВ ===");
            foreach (var item in menu)
            {
                Console.WriteLine($"[{item.Key}] - {item.Value.Description}");
            }
            Console.WriteLine("[0] - Вихід");
            Console.Write("Оберіть дію: ");

            string choice = Console.ReadLine();

            if (choice == "0") break;

            if (menu.ContainsKey(choice))
            {
                menu[choice].Execute(); // Виклик патерну Command
            }
            else
            {
                Console.WriteLine("Невідома команда. Спробуйте ще раз.");
            }
        }
    }
}