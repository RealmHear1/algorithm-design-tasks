using System;
using System.Collections.Generic;

public class MyHashMap<K, V>
{
    private LinkedList<Entry>[] table;
    private int size;
    private float loadFactor;
    private int threshold;

    private class Entry
    {
        public K Key;
        public V Value;
        public int Hash;

        public Entry(K key, V value, int hash)
        {
            Key = key;
            Value = value;
            Hash = hash;
        }
    }

    // 1) Конструктор по умолчанию
    public MyHashMap() : this(16, 0.75f) { }

    // 2) Конструктор с capacity
    public MyHashMap(int initialCapacity) : this(initialCapacity, 0.75f) { }

    // 3) Конструктор с capacity и loadFactor
    public MyHashMap(int initialCapacity, float loadFactor)
    {
        if (initialCapacity <= 0)
            throw new ArgumentException("Capacity must be > 0");
        if (loadFactor <= 0)
            throw new ArgumentException("LoadFactor must be > 0");

        table = new LinkedList<Entry>[initialCapacity];
        this.loadFactor = loadFactor;
        threshold = (int)(initialCapacity * loadFactor);
        size = 0;
    }

    // hash функция
    private int Hash(K key)
    {
        if (key == null)
            throw new ArgumentNullException("Key cannot be null");

        return key.GetHashCode() & 0x7fffffff;
    }

    private int IndexFor(int hash)
    {
        return hash % table.Length;
    }

    // 11) put
    public V Put(K key, V value)
    {
        int hash = Hash(key);
        int index = IndexFor(hash);

        if (table[index] == null)
            table[index] = new LinkedList<Entry>();

        foreach (var entry in table[index])
        {
            if (entry.Hash == hash && entry.Key.Equals(key))
            {
                V oldValue = entry.Value;
                entry.Value = value;
                return oldValue;
            }
        }

        table[index].AddLast(new Entry(key, value, hash));
        size++;

        if (size >= threshold)
            Resize();

        return default(V);
    }

    // 8) get
    public V Get(K key)
    {
        int hash = Hash(key);
        int index = IndexFor(hash);

        var bucket = table[index];
        if (bucket != null)
        {
            foreach (var entry in bucket)
            {
                if (entry.Hash == hash && entry.Key.Equals(key))
                    return entry.Value;
            }
        }

        return default(V);
    }

    // 12) remove
    public V Remove(K key)
    {
        int hash = Hash(key);
        int index = IndexFor(hash);

        var bucket = table[index];
        if (bucket != null)
        {
            var node = bucket.First;
            while (node != null)
            {
                if (node.Value.Hash == hash && node.Value.Key.Equals(key))
                {
                    V oldValue = node.Value.Value;
                    bucket.Remove(node);
                    size--;
                    return oldValue;
                }
                node = node.Next;
            }
        }
        return default(V);
    }

    // 5) containsKey
    public bool ContainsKey(K key)
    {
        return !EqualityComparer<V>.Default.Equals(Get(key), default(V));
    }

    // 6) containsValue
    public bool ContainsValue(V value)
    {
        foreach (var bucket in table)
        {
            if (bucket != null)
            {
                foreach (var entry in bucket)
                {
                    if (EqualityComparer<V>.Default.Equals(entry.Value, value))
                        return true;
                }
            }
        }
        return false;
    }

    // 4) clear
    public void Clear()
    {
        table = new LinkedList<Entry>[table.Length];
        size = 0;
    }

    // 9) isEmpty
    public bool IsEmpty()
    {
        return size == 0;
    }

    // 13) size
    public int Size()
    {
        return size;
    }

    // 10) keySet
    public HashSet<K> KeySet()
    {
        HashSet<K> keys = new HashSet<K>();
        foreach (var bucket in table)
        {
            if (bucket != null)
            {
                foreach (var entry in bucket)
                    keys.Add(entry.Key);
            }
        }
        return keys;
    }

    // 7) entrySet
    public HashSet<(K, V)> EntrySet()
    {
        HashSet<(K, V)> set = new HashSet<(K, V)>();
        foreach (var bucket in table)
        {
            if (bucket != null)
            {
                foreach (var entry in bucket)
                    set.Add((entry.Key, entry.Value));
            }
        }
        return set;
    }

    // Resize
    private void Resize()
    {
        int newCapacity = table.Length * 2;
        var oldTable = table;

        table = new LinkedList<Entry>[newCapacity];
        threshold = (int)(newCapacity * loadFactor);
        size = 0;

        foreach (var bucket in oldTable)
        {
            if (bucket != null)
            {
                foreach (var entry in bucket)
                {
                    Put(entry.Key, entry.Value);
                }
            }
        }
    }
}

class Program
{
    static void Main()
    {
        MyHashMap<string, int> map = new MyHashMap<string, int>();

        map.Put("A", 10);
        map.Put("B", 20);
        map.Put("C", 30);

        Console.WriteLine("Размер: " + map.Size());
        Console.WriteLine("Получить A: " + map.Get("A"));

        map.Remove("B");

        Console.WriteLine("Содержит C: " + map.ContainsKey("C"));
        Console.WriteLine("Все ключи:");

        foreach (var key in map.KeySet())
            Console.WriteLine(key);
    }
}
