using System;
using System.Collections.Generic;

public class MyHashSet<T> : MySet<T>
{
    private static readonly object PRESENT = new object();
    private readonly MyHashMap<T, object> map;

    public MyHashSet()
    {
        map = new MyHashMap<T, object>(16, 0.75f);
    }

    public MyHashSet(T[] a)
        : this()
    {
        if (a != null)
            addAll(new MyArrayList<T>(a));
    }

    public MyHashSet(MyCollection<T> c)
        : this()
    {
        addAll(c);
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

    private List<T> SortedSnapshot()
    {
        object[] arr = toArray();
        List<T> items = new List<T>(arr.Length);
        for (int i = 0; i < arr.Length; i++)
            items.Add((T)arr[i]);
        items.Sort();
        return items;
    }

    public bool add(T e)
    {
        if (map.containsKey(e))
            return false;

        map.put(e, PRESENT);
        return true;
    }

    public void addAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        for (int i = 0; i < a.Length; i++)
            add(a[i]);
    }

    public bool addAll(MyCollection<T> c)
    {
        if (c == null)
            throw new ArgumentNullException(nameof(c));

        bool changed = false;
        object[] arr = c.toArray();
        for (int i = 0; i < arr.Length; i++)
        {
            if (add((T)arr[i]))
                changed = true;
        }

        return changed;
    }

    public void clear()
    {
        map.clear();
    }

    public bool contains(object o)
    {
        return map.containsKey(o);
    }

    public bool containsAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        for (int i = 0; i < a.Length; i++)
        {
            if (!map.containsKey(a[i]))
                return false;
        }

        return true;
    }

    public bool containsAll(MyCollection<T> c)
    {
        if (c == null)
            throw new ArgumentNullException(nameof(c));

        object[] arr = c.toArray();
        for (int i = 0; i < arr.Length; i++)
        {
            if (!contains(arr[i]))
                return false;
        }

        return true;
    }

    public bool isEmpty()
    {
        return map.size() == 0;
    }

    public bool remove(object o)
    {
        return !Equals(map.remove(o), default(object));
    }

    public void removeAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        for (int i = 0; i < a.Length; i++)
            map.remove(a[i]);
    }

    public bool removeAll(MyCollection<T> c)
    {
        if (c == null)
            throw new ArgumentNullException(nameof(c));

        bool changed = false;
        object[] arr = c.toArray();
        for (int i = 0; i < arr.Length; i++)
        {
            if (remove(arr[i]))
                changed = true;
        }
        return changed;
    }

    public void retainAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        HashSet<T> keep = new HashSet<T>(a);
        List<T> toRemove = new List<T>();
        List<T> keys = map.KeySet();

        for (int i = 0; i < keys.Count; i++)
        {
            if (!keep.Contains(keys[i]))
                toRemove.Add(keys[i]);
        }

        for (int i = 0; i < toRemove.Count; i++)
            map.remove(toRemove[i]);
    }

    public bool retainAll(MyCollection<T> c)
    {
        if (c == null)
            throw new ArgumentNullException(nameof(c));

        HashSet<T> keep = new HashSet<T>();
        object[] arr = c.toArray();
        for (int i = 0; i < arr.Length; i++)
            keep.Add((T)arr[i]);

        List<T> toRemove = new List<T>();
        List<T> keys = map.KeySet();
        for (int i = 0; i < keys.Count; i++)
        {
            if (!keep.Contains(keys[i]))
                toRemove.Add(keys[i]);
        }

        for (int i = 0; i < toRemove.Count; i++)
            map.remove(toRemove[i]);

        return toRemove.Count > 0;
    }

    public int size()
    {
        return map.size();
    }

    public object[] toArray()
    {
        List<T> keys = map.KeySet();
        object[] arr = new object[keys.Count];
        for (int i = 0; i < keys.Count; i++)
            arr[i] = keys[i];
        return arr;
    }

    public T[] toArray(T[] a)
    {
        List<T> keys = map.KeySet();

        if (a == null || a.Length < keys.Count)
            a = new T[keys.Count];

        for (int i = 0; i < keys.Count; i++)
            a[i] = keys[i];
        if (a.Length > keys.Count)
            a[keys.Count] = default(T);

        return a;
    }

    public T first()
    {
        List<T> items = SortedSnapshot();
        return items.Count > 0 ? items[0] : default(T);
    }

    public T last()
    {
        List<T> items = SortedSnapshot();
        return items.Count > 0 ? items[items.Count - 1] : default(T);
    }

    public MySet<T> subSet(T fromElement, T toElement)
    {
        MyHashSet<T> set = new MyHashSet<T>();
        List<T> items = SortedSnapshot();
        for (int i = 0; i < items.Count; i++)
        {
            if (Comparer<T>.Default.Compare(items[i], fromElement) >= 0 &&
                Comparer<T>.Default.Compare(items[i], toElement) < 0)
            {
                set.add(items[i]);
            }
        }
        return set;
    }

    public MySet<T> headSet(T toElement)
    {
        MyHashSet<T> set = new MyHashSet<T>();
        List<T> items = SortedSnapshot();
        for (int i = 0; i < items.Count; i++)
        {
            if (Comparer<T>.Default.Compare(items[i], toElement) < 0)
                set.add(items[i]);
        }
        return set;
    }

    public MySet<T> tailSet(T fromElement)
    {
        MyHashSet<T> set = new MyHashSet<T>();
        List<T> items = SortedSnapshot();
        for (int i = 0; i < items.Count; i++)
        {
            if (Comparer<T>.Default.Compare(items[i], fromElement) >= 0)
                set.add(items[i]);
        }
        return set;
    }

    public MyIterator<T> iterator()
    {
        return new MyItr(this);
    }

    private class MyItr : MyIterator<T>
    {
        private readonly List<T> data;
        private readonly MyHashSet<T> set;
        private int cursor;
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
