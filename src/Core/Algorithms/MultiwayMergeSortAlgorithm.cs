using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Algorithms;

/// <summary>
/// Implementation of the Multiway Merge Sort algorithm.
/// Uses dividing the array into K parts and then merging them
/// using the PriorityQueue data structure (Minimum Heap).
/// </summary>
public class MultiwayMergeSortAlgorithm : AlgorithmTemplate
{
    public override string Name => "Multiway Merge Sort";
    public override string Complexity => "O(n log n)";

    // Кількість частин для розбиття
    private readonly int _k;

    public MultiwayMergeSortAlgorithm(int k = 3)
    {
        _k = k;
    }

    // Точка входу патерну Template Method
    protected override void PerformAlgorithm(int[] data)
    {
        // Перетворюємо вхідний масив на List для зручності роботи з підмасивами
        List<int> dataList = data.ToList();
        
        // Виконуємо рекурсивне сортування
        List<int> sortedList = MultiwayMergeSort(dataList, _k);
        
        // Копіюємо результат назад у вхідний масив, щоб змінити його "на місці"
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = sortedList[i];
        }
    }

    private void InsertSort(List<int> A)
    {
        int n = A.Count;
        for (int i = 1; i < n; i++)
        {
            int num = A[i];
            int j = i - 1;
            while (j >= 0 && A[j] > num)
            {
                A[j + 1] = A[j];
                j--;
            }
            A[j + 1] = num;
        }
    }

    private List<List<int>> Divide(List<int> A, int k)
    {
        int n = A.Count;
        int baseSize = n / k;
        int extra = n % k;

        int index = 0;
        List<List<int>> divide = new List<List<int>>(k);

        for (int i = 0; i < k; i++)
        {
            // враховуємо лишок
            int size = baseSize + (i < extra ? 1 : 0); 
            List<int> sublist = new List<int>(size);
            
            for (int j = 0; j < size; j++)
            {
                sublist.Add(A[index++]);
            }
            divide.Add(sublist);
        }

        return divide;
    }

    private List<int> MergeKSorted(List<List<int>> divide)
    {
        // Використовуємо вбудовану в .NET PriorityQueue. 
        // Елемент черги — це кортеж: (значення, індекс_списку, індекс_елемента_у_списку). 
        // Пріоритетом виступає саме значення (чим менше, тим вищий пріоритет).
        var pq = new PriorityQueue<(int val, int from, int idx), int>();
        List<int> result = new List<int>();

        int k = divide.Count;

        // Додаємо перші елементи кожного підмасиву
        for (int i = 0; i < k; i++)
        {
            if (divide[i].Count > 0)
            {
                pq.Enqueue((divide[i][0], i, 0), divide[i][0]);
            }
        }

        while (pq.Count > 0)
        {
            var top = pq.Dequeue();
            result.Add(top.val);

            int nextIndex = top.idx + 1;
            if (nextIndex < divide[top.from].Count)
            {
                int nextVal = divide[top.from][nextIndex];
                pq.Enqueue((nextVal, top.from, nextIndex), nextVal);
            }
        }

        return result;
    }

    private List<int> MultiwayMergeSort(List<int> A, int k)
    {
        int n = A.Count;
        if (n <= 1)
            return A;

        // якщо підмасив менше k, сортуємо його простим способом
        if (n < k)
        {
            InsertSort(A);
            return A;
        }

        // розділяємо масив на k частин
        List<List<int>> divide = Divide(A, k);

        // рекурсивно сортуємо кожен підмасив
        for (int i = 0; i < divide.Count; i++)
        {
            divide[i] = MultiwayMergeSort(divide[i], k);
        }

        // зливаємо відсортовані підмасиви
        return MergeKSorted(divide);
    }
}