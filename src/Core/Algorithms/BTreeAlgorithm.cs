using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Algorithms;

/// <summary>
/// Implementation of a highly branched tree (B-Tree).
/// Optimized for systems that read and write large blocks of data (simulate disk operation).
/// Supports automatic splitting of nodes when they are full.
/// </summary>
public class BTreeAlgorithm : AlgorithmTemplate
{
    public override string Name => "B-Tree Insertion";
    public override string Complexity => "O(log n)";

    // Ступінь дерева (t)
    private int _degree = 3; 
    private int _rootId;
    
    // Імітація віртуального диска
    private readonly List<BTreeNode> _virtualDisk;

    public BTreeAlgorithm()
    {
        _virtualDisk = new List<BTreeNode>();
    }

    // Точка входу патерну Template Method
    protected override void PerformAlgorithm(int[] data)
    {
        _virtualDisk.Clear();
        _degree = 3; 
        
        // Алгоритм B_TREE_CREATE (T)
        int xId = AllocateNode();
        BTreeNode x = DiskRead(xId);
        x.IsLeaf = true;
        x.N = 0;
        DiskWrite(xId);
        _rootId = xId;

        // Вставляємо всі елементи масиву в дерево
        foreach (int key in data)
        {
            Insert(key);
        }
    }

    // Внутрішній клас для вузлів дерева
    private class BTreeNode
    {
        public int N { get; set; }
        public bool IsLeaf { get; set; }
        public List<int> Keys { get; set; }
        public List<int> Children { get; set; }

        public BTreeNode()
        {
            IsLeaf = true;
            N = 0;
            Keys = new List<int>();
            Children = new List<int>();
        }
    }

    #region Операції "Віртуального Диску"

    private BTreeNode? DiskRead(int id)
    {
        if (id < 0 || id >= _virtualDisk.Count) return null;
        return _virtualDisk[id];
    }

    private void DiskWrite(int id)
    {
        // Імітація запису на диск. У пам'яті об'єкт вже оновлено.
    }

    private int AllocateNode()
    {
        var newNode = new BTreeNode();
        _virtualDisk.Add(newNode);
        return _virtualDisk.Count - 1;
    }

    #endregion

    #region Логіка B-Дерева (Вставка)

    private void SplitChild(int xId, int i, int yId)
    {
        BTreeNode x = DiskRead(xId)!;
        BTreeNode y = DiskRead(yId)!;

        int zId = AllocateNode();
        BTreeNode z = DiskRead(zId)!;
        z.IsLeaf = y.IsLeaf;
        z.N = _degree - 1;

        // Копіюємо останні (t-1) ключів з y до z
        for (int j = 0; j < _degree - 1; j++)
        {
            z.Keys.Add(y.Keys[j + _degree]);
        }

        // Якщо y не листок, копіюємо його останніх t дітей до z
        if (!y.IsLeaf)
        {
            for (int j = 0; j < _degree; j++)
            {
                z.Children.Add(y.Children[j + _degree]);
            }
        }

        int median = y.Keys[_degree - 1];

        // Обрізаємо y
        y.Keys.RemoveRange(_degree - 1, y.Keys.Count - (_degree - 1));
        y.N = _degree - 1;
        if (!y.IsLeaf)
        {
            y.Children.RemoveRange(_degree, y.Children.Count - _degree);
        }

        x.Children.Insert(i + 1, zId);
        x.Keys.Insert(i, median);
        x.N++;

        DiskWrite(yId);
        DiskWrite(zId);
        DiskWrite(xId);
    }

    private void Insert(int k)
    {
        BTreeNode r = DiskRead(_rootId)!;

        if (r.N == 2 * _degree - 1)
        {
            int sId = AllocateNode();
            BTreeNode s = DiskRead(sId)!;

            s.IsLeaf = false;
            s.N = 0;
            s.Children.Add(_rootId);

            int oldRootId = _rootId;
            _rootId = sId;

            SplitChild(sId, 0, oldRootId);
            InsertNonFull(sId, k);
        }
        else
        {
            InsertNonFull(_rootId, k);
        }
    }

    private void InsertNonFull(int xId, int k)
    {
        BTreeNode x = DiskRead(xId)!;
        int i = x.N - 1;

        if (x.IsLeaf)
        {
            x.Keys.Add(0); // Розширюємо розмір списку

            while (i >= 0 && k < x.Keys[i])
            {
                x.Keys[i + 1] = x.Keys[i];
                i--;
            }

            x.Keys[i + 1] = k;
            x.N++;
            DiskWrite(xId);
        }
        else
        {
            while (i >= 0 && k < x.Keys[i])
            {
                i--;
            }
            i++;

            BTreeNode child = DiskRead(x.Children[i])!;

            if (child.N == 2 * _degree - 1)
            {
                SplitChild(xId, i, x.Children[i]);

                if (k > x.Keys[i])
                {
                    i++;
                }
            }
            InsertNonFull(x.Children[i], k);
        }
    }

    #endregion

    #region Логіка B-Дерева (Видалення)

    private void BorrowFromPrev(int xId, int idx)
    {
        BTreeNode x = DiskRead(xId)!;
        int childId = x.Children[idx];
        int siblingId = x.Children[idx - 1];

        BTreeNode child = DiskRead(childId)!;
        BTreeNode sibling = DiskRead(siblingId)!;

        child.Keys.Insert(0, x.Keys[idx - 1]);

        if (!child.IsLeaf)
        {
            child.Children.Insert(0, sibling.Children.Last());
            sibling.Children.RemoveAt(sibling.Children.Count - 1);
        }

        x.Keys[idx - 1] = sibling.Keys.Last();
        sibling.Keys.RemoveAt(sibling.Keys.Count - 1);

        child.N++;
        sibling.N--;

        DiskWrite(childId);
        DiskWrite(siblingId);
        DiskWrite(xId);
    }

    private void BorrowFromNext(int xId, int idx)
    {
        BTreeNode x = DiskRead(xId)!;
        int childId = x.Children[idx];
        int nextSiblingId = x.Children[idx + 1];

        BTreeNode child = DiskRead(childId)!;
        BTreeNode nextSibling = DiskRead(nextSiblingId)!;

        child.Keys.Add(x.Keys[idx]);

        if (!child.IsLeaf)
        {
            child.Children.Add(nextSibling.Children.First());
            nextSibling.Children.RemoveAt(0);
        }

        x.Keys[idx] = nextSibling.Keys.First();
        nextSibling.Keys.RemoveAt(0);

        child.N++;
        nextSibling.N--;

        DiskWrite(childId);
        DiskWrite(nextSiblingId);
        DiskWrite(xId);
    }

    private void MergeNodes(int xId, int idx)
    {
        BTreeNode x = DiskRead(xId)!;
        int yId = x.Children[idx];
        int zId = x.Children[idx + 1];

        BTreeNode y = DiskRead(yId)!;
        BTreeNode z = DiskRead(zId)!;

        y.Keys.Add(x.Keys[idx]);
        y.Keys.AddRange(z.Keys);

        if (!y.IsLeaf)
        {
            y.Children.AddRange(z.Children);
        }

        y.N = y.Keys.Count;

        x.Keys.RemoveAt(idx);
        x.Children.RemoveAt(idx + 1);
        x.N--;

        DiskWrite(yId);
        DiskWrite(xId);
    }

    private int GetPredecessor(BTreeNode node)
    {
        BTreeNode curr = node;
        while (!curr.IsLeaf)
        {
            curr = DiskRead(curr.Children[curr.N])!;
        }
        return curr.Keys.Last();
    }

    private int GetSuccessor(BTreeNode node)
    {
        BTreeNode curr = node;
        while (!curr.IsLeaf)
        {
            curr = DiskRead(curr.Children[0])!;
        }
        return curr.Keys.First();
    }

    public void DeleteKey(int xId, int k)
    {
        BTreeNode x = DiskRead(xId)!;
        int i = 0;
        while (i < x.N && k > x.Keys[i]) i++;

        if (i < x.N && x.Keys[i] == k)
        {
            if (x.IsLeaf)
            {
                x.Keys.RemoveAt(i);
                x.N--;
                DiskWrite(xId);
            }
            else
            {
                int yId = x.Children[i];
                int zId = x.Children[i + 1];
                BTreeNode y = DiskRead(yId)!;
                BTreeNode z = DiskRead(zId)!;

                if (y.N >= _degree)
                {
                    int pred = GetPredecessor(y);
                    x.Keys[i] = pred;
                    DeleteKey(yId, pred);
                }
                else if (z.N >= _degree)
                {
                    int succ = GetSuccessor(z);
                    x.Keys[i] = succ;
                    DeleteKey(zId, succ);
                }
                else
                {
                    MergeNodes(xId, i);
                    DeleteKey(yId, k);
                }
            }
        }
        else
        {
            if (x.IsLeaf) return;

            int childId = x.Children[i];
            BTreeNode child = DiskRead(childId)!;

            if (child.N < _degree)
            {
                if (i > 0 && DiskRead(x.Children[i - 1])!.N >= _degree)
                    BorrowFromPrev(xId, i);
                else if (i < x.N && DiskRead(x.Children[i + 1])!.N >= _degree)
                    BorrowFromNext(xId, i);
                else
                {
                    if (i < x.N) MergeNodes(xId, i);
                    else
                    {
                        MergeNodes(xId, i - 1);
                        i--;
                    }
                }
            }

            if (xId == _rootId && x.N == 0)
            {
                _rootId = x.Children[0];
            }

            x = DiskRead(xId)!;
            DeleteKey(x.Children[i], k);
        }
    }

    #endregion
}