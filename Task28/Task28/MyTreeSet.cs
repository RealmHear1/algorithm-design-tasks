using System;
using System.Collections.Generic;

public delegate int TreeMapComparator<K>(K a, K b);

public class MyTreeMap<K, V>
{
    private enum Color { Red, Black }

    private class Node
    {
        public K Key;
        public V Value;
        public Node Left;
        public Node Right;
        public Node Parent;
        public Color Color;

        public Node(K key, V value, Node parent)
        {
            Key = key;
            Value = value;
            Parent = parent;
            Color = Color.Red;
        }
    }

    private TreeMapComparator<K> comparator;
    private Node root;
    private int size;

    public MyTreeMap()
    {
        comparator = null;
    }

    public MyTreeMap(TreeMapComparator<K> comp)
    {
        comparator = comp;
    }

    private int Compare(K a, K b)
    {
        if (comparator != null)
            return comparator(a, b);
        return ((IComparable<K>)a).CompareTo(b);
    }

    public void Clear()
    {
        root = null;
        size = 0;
    }

    public bool IsEmpty() => size == 0;
    public int Size() => size;

    private Node FindNode(K key)
    {
        Node current = root;
        while (current != null)
        {
            int cmp = Compare(key, current.Key);
            if (cmp == 0) return current;
            current = cmp < 0 ? current.Left : current.Right;
        }
        return null;
    }

    public bool ContainsKey(object key)
    {
        if (key == null) return false;
        return FindNode((K)key) != null;
    }

    public List<K> KeySet()
    {
        List<K> list = new List<K>();
        InOrder(root, list);
        return list;
    }

    private void InOrder(Node node, List<K> list)
    {
        if (node == null) return;
        InOrder(node.Left, list);
        list.Add(node.Key);
        InOrder(node.Right, list);
    }

    private void RotateLeft(Node x)
    {
        Node y = x.Right;
        x.Right = y.Left;
        if (y.Left != null) y.Left.Parent = x;

        y.Parent = x.Parent;

        if (x.Parent == null)
            root = y;
        else if (x == x.Parent.Left)
            x.Parent.Left = y;
        else
            x.Parent.Right = y;

        y.Left = x;
        x.Parent = y;
    }

    private void RotateRight(Node x)
    {
        Node y = x.Left;
        x.Left = y.Right;
        if (y.Right != null) y.Right.Parent = x;

        y.Parent = x.Parent;

        if (x.Parent == null)
            root = y;
        else if (x == x.Parent.Right)
            x.Parent.Right = y;
        else
            x.Parent.Left = y;

        y.Right = x;
        x.Parent = y;
    }


    private void FixInsert(Node z)
    {
        while (z.Parent != null && z.Parent.Color == Color.Red)
        {
            if (z.Parent == z.Parent.Parent.Left)
            {
                Node y = z.Parent.Parent.Right;

                if (y != null && y.Color == Color.Red)
                {
                    z.Parent.Color = Color.Black;
                    y.Color = Color.Black;
                    z.Parent.Parent.Color = Color.Red;
                    z = z.Parent.Parent;
                }
                else
                {
                    if (z == z.Parent.Right)
                    {
                        z = z.Parent;
                        RotateLeft(z);
                    }

                    z.Parent.Color = Color.Black;
                    z.Parent.Parent.Color = Color.Red;
                    RotateRight(z.Parent.Parent);
                }
            }
            else
            {
                Node y = z.Parent.Parent.Left;

                if (y != null && y.Color == Color.Red)
                {
                    z.Parent.Color = Color.Black;
                    y.Color = Color.Black;
                    z.Parent.Parent.Color = Color.Red;
                    z = z.Parent.Parent;
                }
                else
                {
                    if (z == z.Parent.Left)
                    {
                        z = z.Parent;
                        RotateRight(z);
                    }

                    z.Parent.Color = Color.Black;
                    z.Parent.Parent.Color = Color.Red;
                    RotateLeft(z.Parent.Parent);
                }
            }
        }

        root.Color = Color.Black;
    }


    public void Put(K key, V value)
    {
        if (key == null)
            throw new ArgumentException("Key is null");

        Node parent = null;
        Node current = root;

        while (current != null)
        {
            parent = current;
            int cmp = Compare(key, current.Key);

            if (cmp == 0)
            {
                current.Value = value;
                return;
            }
            current = cmp < 0 ? current.Left : current.Right;
        }

        Node newNode = new Node(key, value, parent);

        if (parent == null)
            root = newNode;
        else if (Compare(key, parent.Key) < 0)
            parent.Left = newNode;
        else
            parent.Right = newNode;

        FixInsert(newNode);
        size++;
    }


    private Node Min(Node n)
    {
        while (n.Left != null) n = n.Left;
        return n;
    }

    private Node Max(Node n)
    {
        while (n.Right != null) n = n.Right;
        return n;
    }

    public K FirstKey() => Min(root).Key;
    public K LastKey() => Max(root).Key;


    public bool Remove(object key)
    {
        if (key == null) return false;
        Node node = FindNode((K)key);
        if (node == null) return false;

        if (node.Left == null)
            Replace(node, node.Right);
        else if (node.Right == null)
            Replace(node, node.Left);
        else
        {
            Node min = Min(node.Right);
            node.Key = min.Key;
            node.Value = min.Value;
            Replace(min, min.Right);
        }

        size--;
        return true;
    }

    private void Replace(Node u, Node v)
    {
        if (u.Parent == null)
            root = v;
        else if (u == u.Parent.Left)
            u.Parent.Left = v;
        else
            u.Parent.Right = v;

        if (v != null)
            v.Parent = u.Parent;
    }

    public K HigherKey(K key)
    {
        foreach (var k in KeySet())
            if (Compare(k, key) > 0)
                return k;
        return default;
    }

    public K LowerKey(K key)
    {
        K res = default;
        foreach (var k in KeySet())
            if (Compare(k, key) < 0)
                res = k;
        return res;
    }

    public K CeilingKey(K key)
    {
        foreach (var k in KeySet())
            if (Compare(k, key) >= 0)
                return k;
        return default;
    }

    public K FloorKey(K key)
    {
        K res = default;
        foreach (var k in KeySet())
            if (Compare(k, key) <= 0)
                res = k;
        return res;
    }

    public KeyValuePair<K, V>? PollFirstEntry()
    {
        if (root == null) return null;
        Node min = Min(root);
        var res = new KeyValuePair<K, V>(min.Key, min.Value);
        Remove(min.Key);
        return res;
    }

    public KeyValuePair<K, V>? PollLastEntry()
    {
        if (root == null) return null;
        Node max = Max(root);
        var res = new KeyValuePair<K, V>(max.Key, max.Value);
        Remove(max.Key);
        return res;
    }
}

public class MyTreeSet<E>
{
    private MyTreeMap<E, object> m;
    private static readonly object PRESENT = new object();

    public MyTreeSet()
    {
        m = new MyTreeMap<E, object>();
    }

    public bool Add(E e)
    {
        if (m.ContainsKey(e))
            return false;
        m.Put(e, PRESENT);
        return true;
    }

    public int Size() => m.Size();
    public E First() => m.FirstKey();
    public E Last() => m.LastKey();
    public E Higher(E obj) => m.HigherKey(obj);
    public E Lower(E obj) => m.LowerKey(obj);
    public E Ceiling(E obj) => m.CeilingKey(obj);
    public E Floor(E obj) => m.FloorKey(obj);
    public bool Remove(object o) => m.Remove(o);

    public E PollFirst()
    {
        var e = m.PollFirstEntry();
        return e.HasValue ? e.Value.Key : default;
    }

    public E PollLast()
    {
        var e = m.PollLastEntry();
        return e.HasValue ? e.Value.Key : default;
    }
    public MyIterator<E> iterator()
    {
        return new MyItr(this);
    }

    private class MyItr : MyIterator<E>
    {
        private List<E> data;
        private MyTreeSet<E> set;
        private int cursor = 0;
        private int lastRet = -1;

        public MyItr(MyTreeSet<E> s)
        {
            set = s;
            data = new List<E>(s.m.KeySet());
        }

        public bool hasNext()
        {
            return cursor < data.Count;
        }

        public E next()
        {
            if (!hasNext())
                throw new NoSuchElementException();

            lastRet = cursor;
            return data[cursor++];
        }

        public void remove()
        {
            if (lastRet < 0)
                throw new IllegalStateException();

            set.Remove(data[lastRet]);
            data.RemoveAt(lastRet);
            cursor = lastRet;
            lastRet = -1;
        }
    }
}

