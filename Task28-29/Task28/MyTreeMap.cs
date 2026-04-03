using System;
using System.Collections.Generic;

public delegate int TreeMapComparator<K>(K a, K b);

public class MyTreeMap<K, V> : MyNavigableMap<K, V>
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

    private readonly TreeMapComparator<K> comparator;
    private Node root;
    private int count;

    public MyTreeMap()
        : this((TreeMapComparator<K>)null)
    {
    }

    public MyTreeMap(TreeMapComparator<K> comp)
    {
        comparator = comp;
    }

    public MyTreeMap(MyMap<K, V> m)
        : this()
    {
        putAll(m);
    }

    public MyTreeMap(MySortedMap<K, V> sm)
        : this()
    {
        putAll(sm);
    }

    private int Compare(K a, K b)
    {
        if (comparator != null)
            return comparator(a, b);

        return ((IComparable<K>)a).CompareTo(b);
    }

    private Node FindNode(K key)
    {
        Node current = root;

        while (current != null)
        {
            int cmp = Compare(key, current.Key);
            if (cmp == 0)
                return current;

            current = cmp < 0 ? current.Left : current.Right;
        }

        return null;
    }

    private void FillKeys(Node node, List<K> keys)
    {
        if (node == null)
            return;

        FillKeys(node.Left, keys);
        keys.Add(node.Key);
        FillKeys(node.Right, keys);
    }

    private void FillEntries(Node node, List<KeyValuePair<K, V>> entries)
    {
        if (node == null)
            return;

        FillEntries(node.Left, entries);
        entries.Add(new KeyValuePair<K, V>(node.Key, node.Value));
        FillEntries(node.Right, entries);
    }

    private Node Min(Node node)
    {
        if (node == null)
            return null;

        while (node.Left != null)
            node = node.Left;

        return node;
    }

    private Node Max(Node node)
    {
        if (node == null)
            return null;

        while (node.Right != null)
            node = node.Right;

        return node;
    }

    private void RotateLeft(Node x)
    {
        Node y = x.Right;
        x.Right = y.Left;
        if (y.Left != null)
            y.Left.Parent = x;

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
        if (y.Right != null)
            y.Right.Parent = x;

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

    private void Replace(Node oldNode, Node newNode)
    {
        if (oldNode.Parent == null)
            root = newNode;
        else if (oldNode == oldNode.Parent.Left)
            oldNode.Parent.Left = newNode;
        else
            oldNode.Parent.Right = newNode;

        if (newNode != null)
            newNode.Parent = oldNode.Parent;
    }

    public void Clear()
    {
        clear();
    }

    public void clear()
    {
        root = null;
        count = 0;
    }

    public bool IsEmpty()
    {
        return isEmpty();
    }

    public bool isEmpty()
    {
        return count == 0;
    }

    public int Size()
    {
        return size();
    }

    public int size()
    {
        return count;
    }

    public bool ContainsKey(object key)
    {
        return containsKey(key);
    }

    public bool containsKey(object key)
    {
        if (key == null || !(key is K))
            return false;

        return FindNode((K)key) != null;
    }

    public bool ContainsValue(object value)
    {
        return containsValue(value);
    }

    public bool containsValue(object value)
    {
        List<KeyValuePair<K, V>> entries = EntryList();

        for (int i = 0; i < entries.Count; i++)
        {
            if (Equals(entries[i].Value, value))
                return true;
        }

        return false;
    }

    public V Get(object key)
    {
        return get(key);
    }

    public V get(object key)
    {
        if (key == null || !(key is K))
            return default(V);

        Node node = FindNode((K)key);
        return node != null ? node.Value : default(V);
    }

    public V Put(K key, V value)
    {
        return put(key, value);
    }

    public V put(K key, V value)
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
                V oldValue = current.Value;
                current.Value = value;
                return oldValue;
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
        count++;
        return default(V);
    }

    public void PutAll(MyMap<K, V> m)
    {
        putAll(m);
    }

    public void putAll(MyMap<K, V> m)
    {
        if (m == null)
            throw new ArgumentNullException("m");

        object[] entries = m.entrySet().toArray();
        for (int i = 0; i < entries.Length; i++)
        {
            KeyValuePair<K, V> pair = (KeyValuePair<K, V>)entries[i];
            put(pair.Key, pair.Value);
        }
    }

    public V remove(object key)
    {
        if (key == null || !(key is K))
            return default(V);

        Node node = FindNode((K)key);
        if (node == null)
            return default(V);

        V removedValue = node.Value;

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

        count--;
        return removedValue;
    }

    public bool Remove(object key)
    {
        return !Equals(remove(key), default(V));
    }

    public List<K> KeySet()
    {
        List<K> keys = new List<K>();
        FillKeys(root, keys);
        return keys;
    }

    private List<KeyValuePair<K, V>> EntryList()
    {
        List<KeyValuePair<K, V>> entries = new List<KeyValuePair<K, V>>();
        FillEntries(root, entries);
        return entries;
    }

    public MySet<K> keySet()
    {
        MyTreeSet<K> set = new MyTreeSet<K>();
        List<K> keys = KeySet();

        for (int i = 0; i < keys.Count; i++)
            set.add(keys[i]);

        return set;
    }

    public MySet<KeyValuePair<K, V>> entrySet()
    {
        MyHashSet<KeyValuePair<K, V>> set = new MyHashSet<KeyValuePair<K, V>>();
        List<KeyValuePair<K, V>> entries = EntryList();

        for (int i = 0; i < entries.Count; i++)
            set.add(entries[i]);

        return set;
    }

    public MyCollection<V> values()
    {
        MyArrayList<V> list = new MyArrayList<V>();
        List<KeyValuePair<K, V>> entries = EntryList();

        for (int i = 0; i < entries.Count; i++)
            list.add(entries[i].Value);

        return list;
    }

    public K FirstKey()
    {
        return firstKey();
    }

    public K firstKey()
    {
        Node node = Min(root);
        return node != null ? node.Key : default(K);
    }

    public K LastKey()
    {
        return lastKey();
    }

    public K lastKey()
    {
        Node node = Max(root);
        return node != null ? node.Key : default(K);
    }

    public K HigherKey(K key)
    {
        return higherKey(key);
    }

    public K higherKey(K key)
    {
        KeyValuePair<K, V>? entry = higherEntry(key);
        return entry.HasValue ? entry.Value.Key : default(K);
    }

    public K LowerKey(K key)
    {
        return lowerKey(key);
    }

    public K lowerKey(K key)
    {
        KeyValuePair<K, V>? entry = lowerEntry(key);
        return entry.HasValue ? entry.Value.Key : default(K);
    }

    public K CeilingKey(K key)
    {
        return ceilingKey(key);
    }

    public K ceilingKey(K key)
    {
        KeyValuePair<K, V>? entry = ceilingEntry(key);
        return entry.HasValue ? entry.Value.Key : default(K);
    }

    public K FloorKey(K key)
    {
        return floorKey(key);
    }

    public K floorKey(K key)
    {
        KeyValuePair<K, V>? entry = floorEntry(key);
        return entry.HasValue ? entry.Value.Key : default(K);
    }

    public KeyValuePair<K, V>? firstEntry()
    {
        Node node = Min(root);
        if (node == null)
            return null;

        return new KeyValuePair<K, V>(node.Key, node.Value);
    }

    public KeyValuePair<K, V>? lastEntry()
    {
        Node node = Max(root);
        if (node == null)
            return null;

        return new KeyValuePair<K, V>(node.Key, node.Value);
    }

    public KeyValuePair<K, V>? lowerEntry(K key)
    {
        KeyValuePair<K, V>? result = null;
        List<KeyValuePair<K, V>> entries = EntryList();

        for (int i = 0; i < entries.Count; i++)
        {
            if (Compare(entries[i].Key, key) < 0)
                result = entries[i];
            else
                break;
        }

        return result;
    }

    public KeyValuePair<K, V>? floorEntry(K key)
    {
        KeyValuePair<K, V>? result = null;
        List<KeyValuePair<K, V>> entries = EntryList();

        for (int i = 0; i < entries.Count; i++)
        {
            if (Compare(entries[i].Key, key) <= 0)
                result = entries[i];
            else
                break;
        }

        return result;
    }

    public KeyValuePair<K, V>? higherEntry(K key)
    {
        List<KeyValuePair<K, V>> entries = EntryList();

        for (int i = 0; i < entries.Count; i++)
        {
            if (Compare(entries[i].Key, key) > 0)
                return entries[i];
        }

        return null;
    }

    public KeyValuePair<K, V>? ceilingEntry(K key)
    {
        List<KeyValuePair<K, V>> entries = EntryList();

        for (int i = 0; i < entries.Count; i++)
        {
            if (Compare(entries[i].Key, key) >= 0)
                return entries[i];
        }

        return null;
    }

    public KeyValuePair<K, V>? PollFirstEntry()
    {
        return pollFirstEntry();
    }

    public KeyValuePair<K, V>? pollFirstEntry()
    {
        KeyValuePair<K, V>? entry = firstEntry();
        if (entry.HasValue)
            remove(entry.Value.Key);

        return entry;
    }

    public KeyValuePair<K, V>? PollLastEntry()
    {
        return pollLastEntry();
    }

    public KeyValuePair<K, V>? pollLastEntry()
    {
        KeyValuePair<K, V>? entry = lastEntry();
        if (entry.HasValue)
            remove(entry.Value.Key);

        return entry;
    }

    public MySortedMap<K, V> headMap(K end)
    {
        MyTreeMap<K, V> map = new MyTreeMap<K, V>(comparator);
        List<KeyValuePair<K, V>> entries = EntryList();

        for (int i = 0; i < entries.Count; i++)
        {
            if (Compare(entries[i].Key, end) < 0)
                map.put(entries[i].Key, entries[i].Value);
        }

        return map;
    }

    public MySortedMap<K, V> subMap(K start, K end)
    {
        MyTreeMap<K, V> map = new MyTreeMap<K, V>(comparator);
        List<KeyValuePair<K, V>> entries = EntryList();

        for (int i = 0; i < entries.Count; i++)
        {
            if (Compare(entries[i].Key, start) >= 0 && Compare(entries[i].Key, end) < 0)
                map.put(entries[i].Key, entries[i].Value);
        }

        return map;
    }

    public MySortedMap<K, V> tailMap(K start)
    {
        MyTreeMap<K, V> map = new MyTreeMap<K, V>(comparator);
        List<KeyValuePair<K, V>> entries = EntryList();

        for (int i = 0; i < entries.Count; i++)
        {
            if (Compare(entries[i].Key, start) >= 0)
                map.put(entries[i].Key, entries[i].Value);
        }

        return map;
    }
}
