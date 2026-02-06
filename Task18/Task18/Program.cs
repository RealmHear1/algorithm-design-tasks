using System;
using System.Collections.Generic;

public delegate int TreeMapComparator<K>(K a, K b);

public class MyTreeMap<K, V>
{
    private class Node
    {
        public K Key;
        public V Value;
        public Node Left;
        public Node Right;
        public Node Parent;

        public Node(K key, V value, Node parent)
        {
            Key = key;
            Value = value;
            Parent = parent;
        }
    }

    private TreeMapComparator<K> comparator;
    private Node root;
    private int size;

    // 1. Конструктор по умолчанию (естественный порядок)
    public MyTreeMap()
    {
        comparator = null;
        root = null;
        size = 0;
    }

    // 2. Конструктор с компаратором
    public MyTreeMap(TreeMapComparator<K> comp)
    {
        comparator = comp;
        root = null;
        size = 0;
    }

    private int Compare(K a, K b)
    {
        if (comparator != null)
            return comparator(a, b);
        return ((IComparable<K>)a).CompareTo(b);
    }

    // 3. clear()
    public void Clear()
    {
        root = null;
        size = 0;
    }

    // Поиск узла
    private Node FindNode(K key)
    {
        Node current = root;
        while (current != null)
        {
            int cmp = Compare(key, current.Key);
            if (cmp == 0)
                return current;
            if (cmp < 0)
                current = current.Left;
            else
                current = current.Right;
        }
        return null;
    }

    // 4. containsKey
    public bool ContainsKey(object key)
    {
        if (key == null) return false;
        return FindNode((K)key) != null;
    }

    // 5. containsValue
    public bool ContainsValue(object value)
    {
        return ContainsValueRec(root, value);
    }

    private bool ContainsValueRec(Node node, object value)
    {
        if (node == null) return false;
        if (Equals(node.Value, value)) return true;
        return ContainsValueRec(node.Left, value) || ContainsValueRec(node.Right, value);
    }

    // 6. entrySet
    public List<KeyValuePair<K, V>> EntrySet()
    {
        List<KeyValuePair<K, V>> list = new List<KeyValuePair<K, V>>();
        InOrder(root, list);
        return list;
    }

    private void InOrder(Node node, List<KeyValuePair<K, V>> list)
    {
        if (node == null) return;
        InOrder(node.Left, list);
        list.Add(new KeyValuePair<K, V>(node.Key, node.Value));
        InOrder(node.Right, list);
    }

    // 7. get
    public V Get(object key)
    {
        if (key == null) return default(V);
        Node n = FindNode((K)key);
        if (n == null) return default(V);
        return n.Value;
    }

    // 8. isEmpty
    public bool IsEmpty()
    {
        return size == 0;
    }

    // 9. keySet
    public List<K> KeySet()
    {
        List<K> list = new List<K>();
        KeyInOrder(root, list);
        return list;
    }

    private void KeyInOrder(Node node, List<K> list)
    {
        if (node == null) return;
        KeyInOrder(node.Left, list);
        list.Add(node.Key);
        KeyInOrder(node.Right, list);
    }

    // 10. put
    public void Put(K key, V value)
    {
        if (key == null)
            throw new ArgumentException("Ключ не может быть null");

        if (root == null)
        {
            root = new Node(key, value, null);
            size++;
            return;
        }

        Node current = root;
        Node parent = null;
        int cmp = 0;

        while (current != null)
        {
            parent = current;
            cmp = Compare(key, current.Key);
            if (cmp == 0)
            {
                current.Value = value;
                return;
            }
            if (cmp < 0)
                current = current.Left;
            else
                current = current.Right;
        }

        Node newNode = new Node(key, value, parent);
        if (cmp < 0)
            parent.Left = newNode;
        else
            parent.Right = newNode;

        size++;
    }

    // Минимальный узел
    private Node Min(Node node)
    {
        if (node == null) return null;
        while (node.Left != null)
            node = node.Left;
        return node;
    }

    // Максимальный узел
    private Node Max(Node node)
    {
        if (node == null) return null;
        while (node.Right != null)
            node = node.Right;
        return node;
    }

    // Замена узла
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

    // 11. remove
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
            if (min.Parent != node)
            {
                Replace(min, min.Right);
                min.Right = node.Right;
                min.Right.Parent = min;
            }
            Replace(node, min);
            min.Left = node.Left;
            min.Left.Parent = min;
        }

        size--;
        return true;
    }

    // 12. size
    public int Size()
    {
        return size;
    }

    // 13. firstKey
    public K FirstKey()
    {
        if (root == null)
            throw new InvalidOperationException("Отображение пусто");
        return Min(root).Key;
    }

    // 14. lastKey
    public K LastKey()
    {
        if (root == null)
            throw new InvalidOperationException("Отображение пусто");
        return Max(root).Key;
    }

    // 15. headMap
    public MyTreeMap<K, V> HeadMap(K end)
    {
        MyTreeMap<K, V> map = new MyTreeMap<K, V>(comparator);
        foreach (var kv in EntrySet())
            if (Compare(kv.Key, end) < 0)
                map.Put(kv.Key, kv.Value);
        return map;
    }

    // 16. subMap
    public MyTreeMap<K, V> SubMap(K start, K end)
    {
        MyTreeMap<K, V> map = new MyTreeMap<K, V>(comparator);
        foreach (var kv in EntrySet())
            if (Compare(kv.Key, start) >= 0 && Compare(kv.Key, end) < 0)
                map.Put(kv.Key, kv.Value);
        return map;
    }

    // 17. tailMap
    public MyTreeMap<K, V> TailMap(K start)
    {
        MyTreeMap<K, V> map = new MyTreeMap<K, V>(comparator);
        foreach (var kv in EntrySet())
            if (Compare(kv.Key, start) > 0)
                map.Put(kv.Key, kv.Value);
        return map;
    }

    // Поиск ближайших узлов
    private KeyValuePair<K, V>? FindLower(K key, bool equal)
    {
        Node current = root;
        Node result = null;

        while (current != null)
        {
            int cmp = Compare(key, current.Key);
            if (cmp > 0 || (equal && cmp == 0))
            {
                result = current;
                current = current.Right;
            }
            else
                current = current.Left;
        }

        if (result == null) return null;
        return new KeyValuePair<K, V>(result.Key, result.Value);
    }

    private KeyValuePair<K, V>? FindHigher(K key, bool equal)
    {
        Node current = root;
        Node result = null;

        while (current != null)
        {
            int cmp = Compare(key, current.Key);
            if (cmp < 0 || (equal && cmp == 0))
            {
                result = current;
                current = current.Left;
            }
            else
                current = current.Right;
        }

        if (result == null) return null;
        return new KeyValuePair<K, V>(result.Key, result.Value);
    }

    // 18–21 Entry методы
    public KeyValuePair<K, V>? LowerEntry(K key) { return FindLower(key, false); }
    public KeyValuePair<K, V>? FloorEntry(K key) { return FindLower(key, true); }
    public KeyValuePair<K, V>? HigherEntry(K key) { return FindHigher(key, false); }
    public KeyValuePair<K, V>? CeilingEntry(K key) { return FindHigher(key, true); }

    // 22–25 Key методы
    public K LowerKey(K key)
    {
        var e = LowerEntry(key);
        return e.HasValue ? e.Value.Key : default(K);
    }

    public K FloorKey(K key)
    {
        var e = FloorEntry(key);
        return e.HasValue ? e.Value.Key : default(K);
    }

    public K HigherKey(K key)
    {
        var e = HigherEntry(key);
        return e.HasValue ? e.Value.Key : default(K);
    }

    public K CeilingKey(K key)
    {
        var e = CeilingEntry(key);
        return e.HasValue ? e.Value.Key : default(K);
    }

    // 26. pollFirstEntry
    public KeyValuePair<K, V>? PollFirstEntry()
    {
        if (root == null) return null;
        Node min = Min(root);
        KeyValuePair<K, V> res = new KeyValuePair<K, V>(min.Key, min.Value);
        Remove(min.Key);
        return res;
    }

    // 27. pollLastEntry
    public KeyValuePair<K, V>? PollLastEntry()
    {
        if (root == null) return null;
        Node max = Max(root);
        KeyValuePair<K, V> res = new KeyValuePair<K, V>(max.Key, max.Value);
        Remove(max.Key);
        return res;
    }

    // 28. firstEntry
    public KeyValuePair<K, V>? FirstEntry()
    {
        if (root == null) return null;
        Node min = Min(root);
        return new KeyValuePair<K, V>(min.Key, min.Value);
    }

    // 29. lastEntry
    public KeyValuePair<K, V>? LastEntry()
    {
        if (root == null) return null;
        Node max = Max(root);
        return new KeyValuePair<K, V>(max.Key, max.Value);
    }
}

public class Program
{
    public static void Main()
    {
        MyTreeMap<int, string> map = new MyTreeMap<int, string>();

        map.Put(5, "Пять");
        map.Put(2, "Два");
        map.Put(8, "Восемь");
        map.Put(1, "Один");
        map.Put(3, "Три");

        Console.WriteLine("Размер: " + map.Size());
        Console.WriteLine("Первый ключ: " + map.FirstKey());
        Console.WriteLine("Последний ключ: " + map.LastKey());
        Console.WriteLine("Значение по ключу 3: " + map.Get(3));

        var first = map.PollFirstEntry();
        Console.WriteLine("Удалён первый: " + first.Value.Key + " -> " + first.Value.Value);

        Console.WriteLine("Новый первый ключ: " + map.FirstKey());
    }
}
