using System;

namespace Core.Algorithms;

public class RedBlackTreeAlgorithm : AlgorithmTemplate
{
    public override string Name => "Red-Black Tree (Order Statistic)";
    public override string Complexity => "O(n log n)"; // O(log n) на кожну вставку

    private Node? _root;

    // Внутрішні структури з вашого C++ коду
    private enum NodeColor { Red, Black }

    private class Node
    {
        public int Key;
        public NodeColor Color;
        public Node? Left, Right, Parent;
        public int Size;

        public Node(int k)
        {
            Key = k;
            Color = NodeColor.Red;
            Left = null;
            Right = null;
            Parent = null;
            Size = 1;
        }
    }

    // Точка входу для нашого патерну Template Method
    protected override void PerformAlgorithm(int[] data)
    {
        _root = null; // Очищаємо дерево перед новим тестом
        
        // Вставляємо всі елементи масиву в дерево
        foreach (int key in data)
        {
            Insert(new Node(key));
        }
    }

    #region Допоміжні методи (Розмір та Обертання)

    private int GetSize(Node? x) => x?.Size ?? 0;

    private void UpdateSize(Node? x)
    {
        if (x != null) 
            x.Size = GetSize(x.Left) + GetSize(x.Right) + 1;
    }

    private void LeftRotate(Node x)
    {
        Node y = x.Right!;
        x.Right = y.Left;
        
        if (y.Left != null) 
            y.Left.Parent = x;
            
        y.Parent = x.Parent;
        
        if (x.Parent == null) 
            _root = y;
        else if (x == x.Parent.Left) 
            x.Parent.Left = y;
        else 
            x.Parent.Right = y;
            
        y.Left = x;
        x.Parent = y;
        
        UpdateSize(x);
        UpdateSize(y);
    }

    private void RightRotate(Node y)
    {
        Node x = y.Left!;
        y.Left = x.Right;
        
        if (x.Right != null) 
            x.Right.Parent = y;
            
        x.Parent = y.Parent;
        
        if (y.Parent == null) 
            _root = x;
        else if (y == y.Parent.Right) 
            y.Parent.Right = x;
        else 
            y.Parent.Left = x;
            
        x.Right = y;
        y.Parent = x;
        
        UpdateSize(y);
        UpdateSize(x);
    }

    #endregion

    #region Логіка вставки

    private void InsertFixup(Node z)
    {
        while (z.Parent != null && z.Parent.Color == NodeColor.Red)
        {
            if (z.Parent == z.Parent.Parent!.Left)
            {
                Node? y = z.Parent.Parent.Right;
                if (y != null && y.Color == NodeColor.Red) // Case 1
                {
                    z.Parent.Color = NodeColor.Black;
                    y.Color = NodeColor.Black;
                    z.Parent.Parent.Color = NodeColor.Red;
                    z = z.Parent.Parent;
                }
                else
                {
                    if (z == z.Parent.Right) // Case 2
                    {
                        z = z.Parent;
                        LeftRotate(z);
                    }
                    z.Parent!.Color = NodeColor.Black; // Case 3
                    z.Parent.Parent!.Color = NodeColor.Red;
                    RightRotate(z.Parent.Parent);
                }
            }
            else // Symmetric Case
            {
                Node? y = z.Parent.Parent.Left;
                if (y != null && y.Color == NodeColor.Red)
                {
                    z.Parent.Color = NodeColor.Black;
                    y.Color = NodeColor.Black;
                    z.Parent.Parent.Color = NodeColor.Red;
                    z = z.Parent.Parent;
                }
                else
                {
                    if (z == z.Parent.Left)
                    {
                        z = z.Parent;
                        RightRotate(z);
                    }
                    z.Parent!.Color = NodeColor.Black;
                    z.Parent.Parent!.Color = NodeColor.Red;
                    LeftRotate(z.Parent.Parent);
                }
            }
        }
        _root!.Color = NodeColor.Black; // Property 2
    }

    private void Insert(Node z)
    {
        Node? y = null;
        Node? x = _root;
        
        while (x != null)
        {
            y = x;
            x.Size++; // Increment sizes on the way down
            if (z.Key < x.Key) 
                x = x.Left;
            else 
                x = x.Right;
        }
        
        z.Parent = y;
        if (y == null) 
            _root = z;
        else if (z.Key < y.Key) 
            y.Left = z;
        else 
            y.Right = z;
            
        z.Left = null;
        z.Right = null;
        z.Color = NodeColor.Red;
        z.Size = 1;
        
        InsertFixup(z);
    }

    #endregion

    #region Order Statistic (Дерево порядкової статистики)

    // Ці методи перенесені з вашого C++ коду на випадок, якщо ви захочете їх викликати
    
    public int? OsSelect(int i)
    {
        Node? selected = OsSelectRecursive(_root, i);
        return selected?.Key;
    }

    private Node? OsSelectRecursive(Node? x, int i)
    {
        if (x == null) return null;
        int r = GetSize(x.Left) + 1;
        if (i == r) return x;
        if (i < r) return OsSelectRecursive(x.Left, i);
        return OsSelectRecursive(x.Right, i - r);
    }

    public int OsRank(int targetKey)
    {
        Node? x = _root;
        while (x != null && x.Key != targetKey)
        {
            if (targetKey < x.Key) x = x.Left;
            else x = x.Right;
        }

        if (x == null) return -1; // Не знайдено

        int r = GetSize(x.Left) + 1;
        Node y = x;
        while (y != _root && y.Parent != null)
        {
            if (y == y.Parent.Right) 
                r += GetSize(y.Parent.Left) + 1;
            y = y.Parent;
        }
        return r;
    }

    #endregion
}