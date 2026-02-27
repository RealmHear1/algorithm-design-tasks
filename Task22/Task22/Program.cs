using System;
using System.IO;
using System.Text.RegularExpressions;
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

    public MyHashMap() : this(16, 0.75f) { }

    public MyHashMap(int capacity, float loadFactor)
    {
        table = new LinkedList<Entry>[capacity];
        this.loadFactor = loadFactor;
        threshold = (int)(capacity * loadFactor);
    }

    private int Hash(K key)
    {
        return key.GetHashCode() & 0x7fffffff;
    }

    private int Index(int hash)
    {
        return hash % table.Length;
    }

    public void Put(K key, V value)
    {
        int hash = Hash(key);
        int index = Index(hash);

        if (table[index] == null)
            table[index] = new LinkedList<Entry>();

        foreach (var entry in table[index])
        {
            if (entry.Hash == hash && entry.Key.Equals(key))
            {
                entry.Value = value;
                return;
            }
        }

        table[index].AddLast(new Entry(key, value, hash));
        size++;

        if (size >= threshold)
            Resize();
    }

    public V Get(K key)
    {
        int hash = Hash(key);
        int index = Index(hash);

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

    public bool ContainsKey(K key)
    {
        return !EqualityComparer<V>.Default.Equals(Get(key), default(V));
    }

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
        string filePath = "input.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл input.txt не найден.");
            return;
        }

        MyHashMap<string, int> map = new MyHashMap<string, int>();

        Regex regex = new Regex(@"<\/?[A-Za-z][A-Za-z0-9]*>");

        foreach (string line in File.ReadLines(filePath))
        {
            MatchCollection matches = regex.Matches(line);

            foreach (Match match in matches)
            {
                string tag = match.Value;

                // убираем < > /
                tag = tag.Replace("<", "")
                         .Replace(">", "")
                         .Replace("/", "")
                         .ToLower();

                int count = map.Get(tag);

                if (count == 0)
                    map.Put(tag, 1);
                else
                    map.Put(tag, count + 1);
            }
        }

        Console.WriteLine("Статистика тегов:");

        foreach (var entry in map.EntrySet())
        {
            Console.WriteLine(entry.Item1 + " : " + entry.Item2);
        }
    }
}
