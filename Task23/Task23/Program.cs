using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public enum VarType
{
    Int,
    Float,
    Double
}

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
        int hash = Hash(key);
        int index = Index(hash);

        var bucket = table[index];
        if (bucket != null)
        {
            foreach (var entry in bucket)
            {
                if (entry.Hash == hash && entry.Key.Equals(key))
                    return true;
            }
        }
        return false;
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
                    Put(entry.Key, entry.Value);
            }
        }
    }
}

class VariableInfo
{
    public VarType Type;
    public string Value;

    public VariableInfo(VarType type, string value)
    {
        Type = type;
        Value = value;
    }
}

class Program
{
    static void Main()
    {
        string inputPath = "input.txt";
        string outputPath = "output.txt";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("input.txt not found.");
            return;
        }

        string content = File.ReadAllText(inputPath);

        MyHashMap<string, VariableInfo> map =
            new MyHashMap<string, VariableInfo>();

        Regex regex = new Regex(
            @"([A-Za-z_][A-Za-z0-9_]*)\s+([A-Za-z_][A-Za-z0-9_]*)\s*=\s*(\d+)\s*;",
            RegexOptions.Multiline);

        MatchCollection matches = regex.Matches(content);

        foreach (Match match in matches)
        {
            string typeStr = match.Groups[1].Value;
            string varName = match.Groups[2].Value;
            string value = match.Groups[3].Value;

            VarType varType;

            if (typeStr == "int")
                varType = VarType.Int;
            else if (typeStr == "float")
                varType = VarType.Float;
            else if (typeStr == "double")
                varType = VarType.Double;
            else
            {
                Console.WriteLine("Неправильный тип: " + match.Value);
                continue;
            }

            if (map.ContainsKey(varName))
            {
                Console.WriteLine("Переопределение замечено: " + varName);
                continue;
            }

            map.Put(varName, new VariableInfo(varType, value));
        }

        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            foreach (var entry in map.EntrySet())
            {
                writer.WriteLine(
                    entry.Item2.Type.ToString().ToLower() +
                    " => " +
                    entry.Item1 +
                    "(" +
                    entry.Item2.Value +
                    ")");
            }
        }

        Console.WriteLine("Процесс завершён. Записано в output.txt");
    }
}
