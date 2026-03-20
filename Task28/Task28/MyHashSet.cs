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
    public MyIterator<T> iterator()
    {
        return new MyItr(this);
    }

    private class MyItr : MyIterator<T>
    {
        private List<T> data;
        private MyHashSet<T> set;
        private int cursor = 0;
        private int lastRet = -1;

        public MyItr(MyHashSet<T> s)
        {
            set = s;

            data = new List<T>();

            object[] arr = s.toArray();

            for (int i = 0; i < arr.Length; i++)
                data.Add((T)arr[i]);
        }

        public bool hasNext()
        {
            return cursor < data.Count;
        }

        public T next()
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

            set.remove(data[lastRet]);
            data.RemoveAt(lastRet);
            cursor = lastRet;
            lastRet = -1;
        }
    }
}

