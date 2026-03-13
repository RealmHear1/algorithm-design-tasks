using System;
using System.Collections.Generic;

public class MyHashSet<T>
{
    private static readonly object PRESENT = new object();
    private MyHashMap<T, object> map;

    public MyHashSet()
    {
        map = new MyHashMap<T, object>(16, 0.75f);
    }

    public MyHashSet(T[] a)
    {
        map = new MyHashMap<T, object>(16, 0.75f);
        if (a != null)
            addAll(a);
    }

    public MyHashSet(int initialCapacity, float loadFactor)
    {
        if (initialCapacity <= 0)
            throw new ArgumentException("Capacity must be positive");
        if (loadFactor <= 0)
            throw new ArgumentException("Load factor must be positive");

        map = new MyHashMap<T, object>(initialCapacity, loadFactor);
    }

    public MyHashSet(int initialCapacity)
    {
        if (initialCapacity <= 0)
            throw new ArgumentException("Capacity must be positive");

        map = new MyHashMap<T, object>(initialCapacity, 0.75f);
    }

    public bool add(T e)
    {
        if (map.ContainsKey(e))
            return false;

        map.Put(e, PRESENT);
        return true;
    }

    public void addAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        foreach (var item in a)
            add(item);
    }

    public void clear()
    {
        map.Clear();
    }

    public bool contains(object o)
    {
        if (o == null || !(o is T))
            return false;

        return map.ContainsKey((T)o);
    }

    public bool containsAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        foreach (var item in a)
        {
            if (!map.ContainsKey(item))
                return false;
        }

        return true;
    }

    public bool isEmpty()
    {
        return map.Size() == 0;
    }

    public bool remove(object o)
    {
        if (o == null || !(o is T))
            return false;

        return map.Remove((T)o) != null;
    }

    public void removeAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        foreach (var item in a)
            map.Remove(item);
    }

    public void retainAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        HashSet<T> keep = new HashSet<T>(a);
        List<T> toRemove = new List<T>();

        foreach (var key in map.KeySet())
        {
            if (!keep.Contains(key))
                toRemove.Add(key);
        }

        foreach (var key in toRemove)
            map.Remove(key);
    }

    public int size()
    {
        return map.Size();
    }

    public object[] toArray()
    {
        List<object> list = new List<object>();

        foreach (var key in map.KeySet())
            list.Add(key);

        return list.ToArray();
    }

    public T[] toArray(T[] a)
    {
        var keys = new List<T>(map.KeySet());

        if (a == null || a.Length < keys.Count)
            return keys.ToArray();

        for (int i = 0; i < keys.Count; i++)
            a[i] = keys[i];

        return a;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("MyHashSet");

        MyHashSet<int> set = new MyHashSet<int>();

        set.add(10);
        set.add(20);
        set.add(30);
        set.add(20);

        Console.WriteLine("Размер множества: " + set.size());

        Console.WriteLine("Содержит 20: " + set.contains(20));
        Console.WriteLine("Содержит 50: " + set.contains(50));

        Console.WriteLine("\nДобавляем массив {40,50,60}");
        set.addAll(new int[] { 40, 50, 60 });

        Console.WriteLine("Размер после addAll: " + set.size());

        Console.WriteLine("\nЭлементы множества:");
        foreach (var x in set.toArray())
            Console.WriteLine(x);

        Console.WriteLine("\nУдаляем 20");
        set.remove(20);

        Console.WriteLine("Размер после удаления: " + set.size());

        Console.WriteLine("\nremoveAll {10,40}");
        set.removeAll(new int[] { 10, 40 });

        Console.WriteLine("Элементы после removeAll:");
        foreach (var x in set.toArray())
            Console.WriteLine(x);

        Console.WriteLine("\nretainAll {30,60}");
        set.retainAll(new int[] { 30, 60 });

        Console.WriteLine("Элементы после retainAll:");
        foreach (var x in set.toArray())
            Console.WriteLine(x);

        Console.WriteLine("\nОчистка множества");
        set.clear();

        Console.WriteLine("Пустое ли множество: " + set.isEmpty());
    }
}
