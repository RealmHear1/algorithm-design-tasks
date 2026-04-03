using System;
using System.Collections.Generic;

public class MyTreeSet<E> : MyNavigableSet<E>
{
    private readonly MyTreeMap<E, object> m;
    private static readonly object PRESENT = new object();

    public MyTreeSet()
    {
        m = new MyTreeMap<E, object>();
    }

    public MyTreeSet(MyCollection<E> c)
        : this()
    {
        addAll(c);
    }

    public bool Add(E e)
    {
        return add(e);
    }

    public bool add(E e)
    {
        if (m.containsKey(e))
            return false;

        m.put(e, PRESENT);
        return true;
    }

    public bool addAll(MyCollection<E> c)
    {
        if (c == null)
            throw new ArgumentNullException("c");

        bool changed = false;
        object[] arr = c.toArray();

        for (int i = 0; i < arr.Length; i++)
        {
            if (add((E)arr[i]))
                changed = true;
        }

        return changed;
    }

    public void clear()
    {
        m.clear();
    }

    public bool contains(object o)
    {
        return m.containsKey(o);
    }

    public bool containsAll(MyCollection<E> c)
    {
        if (c == null)
            throw new ArgumentNullException("c");

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
        return m.isEmpty();
    }

    public bool remove(object o)
    {
        return !Equals(m.remove(o), default(object));
    }

    public bool removeAll(MyCollection<E> c)
    {
        if (c == null)
            throw new ArgumentNullException("c");

        bool changed = false;
        object[] arr = c.toArray();

        for (int i = 0; i < arr.Length; i++)
        {
            if (remove(arr[i]))
                changed = true;
        }

        return changed;
    }

    public bool retainAll(MyCollection<E> c)
    {
        if (c == null)
            throw new ArgumentNullException("c");

        MyHashSet<E> keep = new MyHashSet<E>(c);
        object[] arr = toArray();
        bool changed = false;

        for (int i = 0; i < arr.Length; i++)
        {
            if (!keep.contains(arr[i]))
            {
                remove(arr[i]);
                changed = true;
            }
        }

        return changed;
    }

    public int size()
    {
        return m.size();
    }

    public object[] toArray()
    {
        List<E> keys = m.KeySet();
        object[] arr = new object[keys.Count];

        for (int i = 0; i < keys.Count; i++)
            arr[i] = keys[i];

        return arr;
    }

    public E[] toArray(E[] a)
    {
        List<E> keys = m.KeySet();

        if (a == null || a.Length < keys.Count)
            a = new E[keys.Count];

        for (int i = 0; i < keys.Count; i++)
            a[i] = keys[i];

        return a;
    }

    public int Size()
    {
        return size();
    }

    public E First()
    {
        return first();
    }

    public E first()
    {
        return m.firstKey();
    }

    public E Last()
    {
        return last();
    }

    public E last()
    {
        return m.lastKey();
    }

    public E Higher(E obj)
    {
        return higherKey(obj);
    }

    public E higherKey(E key)
    {
        return m.higherKey(key);
    }

    public E Lower(E obj)
    {
        return lowerKey(obj);
    }

    public E lowerKey(E key)
    {
        return m.lowerKey(key);
    }

    public E Ceiling(E obj)
    {
        return ceilingKey(obj);
    }

    public E ceilingKey(E key)
    {
        return m.ceilingKey(key);
    }

    public E Floor(E obj)
    {
        return floorKey(obj);
    }

    public E floorKey(E key)
    {
        return m.floorKey(key);
    }

    public bool Remove(object o)
    {
        return remove(o);
    }

    public E PollFirst()
    {
        KeyValuePair<E, object>? entry = pollFirstEntry();
        return entry.HasValue ? entry.Value.Key : default(E);
    }

    public E PollLast()
    {
        KeyValuePair<E, object>? entry = pollLastEntry();
        return entry.HasValue ? entry.Value.Key : default(E);
    }

    public KeyValuePair<E, object>? lowerEntry(E key)
    {
        return m.lowerEntry(key);
    }

    public KeyValuePair<E, object>? floorEntry(E key)
    {
        return m.floorEntry(key);
    }

    public KeyValuePair<E, object>? higherEntry(E key)
    {
        return m.higherEntry(key);
    }

    public KeyValuePair<E, object>? ceilingEntry(E key)
    {
        return m.ceilingEntry(key);
    }

    public KeyValuePair<E, object>? pollFirstEntry()
    {
        return m.pollFirstEntry();
    }

    public KeyValuePair<E, object>? pollLastEntry()
    {
        return m.pollLastEntry();
    }

    public KeyValuePair<E, object>? firstEntry()
    {
        return m.firstEntry();
    }

    public KeyValuePair<E, object>? lastEntry()
    {
        return m.lastEntry();
    }

    public MySet<E> subSet(E fromElement, E toElement)
    {
        MyTreeSet<E> set = new MyTreeSet<E>();
        object[] arr = toArray();

        for (int i = 0; i < arr.Length; i++)
        {
            E item = (E)arr[i];
            if (Comparer<E>.Default.Compare(item, fromElement) >= 0 &&
                Comparer<E>.Default.Compare(item, toElement) < 0)
            {
                set.add(item);
            }
        }

        return set;
    }

    public MySet<E> headSet(E toElement)
    {
        MyTreeSet<E> set = new MyTreeSet<E>();
        object[] arr = toArray();

        for (int i = 0; i < arr.Length; i++)
        {
            E item = (E)arr[i];
            if (Comparer<E>.Default.Compare(item, toElement) < 0)
                set.add(item);
        }

        return set;
    }

    public MySet<E> tailSet(E fromElement)
    {
        MyTreeSet<E> set = new MyTreeSet<E>();
        object[] arr = toArray();

        for (int i = 0; i < arr.Length; i++)
        {
            E item = (E)arr[i];
            if (Comparer<E>.Default.Compare(item, fromElement) >= 0)
                set.add(item);
        }

        return set;
    }

    public MyIterator<E> iterator()
    {
        return new MyItr(this);
    }

    private class MyItr : MyIterator<E>
    {
        private readonly List<E> data;
        private readonly MyTreeSet<E> set;
        private int cursor;
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

            set.remove(data[lastRet]);
            data.RemoveAt(lastRet);
            cursor = lastRet;
            lastRet = -1;
        }
    }
}
