using System;
using System.Collections.Generic;

public class MyTreeSet<E>
{
    private MyTreeMap<E, object> m;
    private static readonly object PRESENT = new object();

    // 1. Пустое множество (естественный порядок)
    public MyTreeSet()
    {
        m = new MyTreeMap<E, object>();
    }

    // 2. С существующим TreeMap
    public MyTreeSet(MyTreeMap<E, object> map)
    {
        if (map == null)
            throw new ArgumentException("Map не может быть null");
        m = map;
    }

    // 3. С компаратором
    public MyTreeSet(TreeMapComparator<E> comparator)
    {
        m = new MyTreeMap<E, object>(comparator);
    }

    // 4. Из массива
    public MyTreeSet(E[] a)
    {
        m = new MyTreeMap<E, object>();
        AddAll(a);
    }

    // 5. Из SortedSet
    public MyTreeSet(SortedSet<E> s)
    {
        m = new MyTreeMap<E, object>();
        foreach (E e in s)
            Add(e);
    }

    // 6. add
    public bool Add(E e)
    {
        if (m.ContainsKey(e))
            return false;
        m.Put(e, PRESENT);
        return true;
    }

    // 7. addAll
    public void AddAll(E[] a)
    {
        if (a == null) return;
        foreach (E e in a)
            Add(e);
    }

    // 8. clear
    public void Clear()
    {
        m.Clear();
    }

    // 9. contains
    public bool Contains(object o)
    {
        return m.ContainsKey(o);
    }

    // 10. containsAll
    public bool ContainsAll(E[] a)
    {
        if (a == null) return true;
        foreach (E e in a)
            if (!Contains(e)) return false;
        return true;
    }

    // 11. isEmpty
    public bool IsEmpty()
    {
        return m.IsEmpty();
    }

    // 12. remove
    public bool Remove(object o)
    {
        return m.Remove(o);
    }

    // 13. removeAll
    public void RemoveAll(E[] a)
    {
        if (a == null) return;
        foreach (E e in a)
            Remove(e);
    }

    // 14. retainAll
    public void RetainAll(E[] a)
    {
        if (a == null)
        {
            Clear();
            return;
        }

        HashSet<E> keep = new HashSet<E>(a);
        List<E> keys = m.KeySet();

        foreach (E e in keys)
            if (!keep.Contains(e))
                Remove(e);
    }

    // 15. size
    public int Size()
    {
        return m.Size();
    }

    // 16. toArray()
    public object[] ToArray()
    {
        List<E> keys = m.KeySet();
        object[] arr = new object[keys.Count];
        for (int i = 0; i < keys.Count; i++)
            arr[i] = keys[i];
        return arr;
    }

    // 17. toArray(T[] a)
    public E[] ToArray(E[] a)
    {
        List<E> keys = m.KeySet();
        if (a == null || a.Length < keys.Count)
            a = new E[keys.Count];

        for (int i = 0; i < keys.Count; i++)
            a[i] = keys[i];
        return a;
    }

    // 18. first
    public E First()
    {
        return m.FirstKey();
    }

    // 19. last
    public E Last()
    {
        return m.LastKey();
    }

    // 20. subSet [from; to)
    public MyTreeSet<E> SubSet(E fromElement, E toElement)
    {
        MyTreeMap<E, object> sub = m.SubMap(fromElement, toElement);
        return new MyTreeSet<E>(sub);
    }

    // 21. headSet (< toElement)
    public MyTreeSet<E> HeadSet(E toElement)
    {
        MyTreeMap<E, object> sub = m.HeadMap(toElement);
        return new MyTreeSet<E>(sub);
    }

    // 22. tailSet (>= fromElement)
    public MyTreeSet<E> TailSet(E fromElement)
    {
        MyTreeMap<E, object> sub = m.TailMap(fromElement);
        return new MyTreeSet<E>(sub);
    }

    // 23. ceiling (>=)
    public E Ceiling(E obj)
    {
        var e = m.CeilingKey(obj);
        return e;
    }

    // 24. floor (<=)
    public E Floor(E obj)
    {
        var e = m.FloorKey(obj);
        return e;
    }

    // 25. higher (>)
    public E Higher(E obj)
    {
        return m.HigherKey(obj);
    }

    // 26. lower (<)
    public E Lower(E obj)
    {
        return m.LowerKey(obj);
    }

    // 27. headSet(bound, incl)
    public MyTreeSet<E> HeadSet(E upperBound, bool incl)
    {
        MyTreeMap<E, object> result = new MyTreeMap<E, object>();
        foreach (E e in m.KeySet())
        {
            int cmp = ((IComparable<E>)e).CompareTo(upperBound);
            if (cmp < 0 || (incl && cmp == 0))
                result.Put(e, PRESENT);
        }
        return new MyTreeSet<E>(result);
    }

    // 28. subSet(lower, lowIncl, upper, highIncl)
    public MyTreeSet<E> SubSet(E lower, bool lowIncl, E upper, bool highIncl)
    {
        MyTreeMap<E, object> result = new MyTreeMap<E, object>();

        foreach (E e in m.KeySet())
        {
            int c1 = ((IComparable<E>)e).CompareTo(lower);
            int c2 = ((IComparable<E>)e).CompareTo(upper);

            bool okLower = c1 > 0 || (lowIncl && c1 == 0);
            bool okUpper = c2 < 0 || (highIncl && c2 == 0);

            if (okLower && okUpper)
                result.Put(e, PRESENT);
        }

        return new MyTreeSet<E>(result);
    }

    // 29. tailSet(bound, inclusive)
    public MyTreeSet<E> TailSet(E fromElement, bool inclusive)
    {
        MyTreeMap<E, object> result = new MyTreeMap<E, object>();

        foreach (E e in m.KeySet())
        {
            int cmp = ((IComparable<E>)e).CompareTo(fromElement);
            if (cmp > 0 || (inclusive && cmp == 0))
                result.Put(e, PRESENT);
        }

        return new MyTreeSet<E>(result);
    }

    // 30. pollLast
    public E PollLast()
    {
        var e = m.PollLastEntry();
        if (e.HasValue)
            return e.Value.Key;
        return default(E);
    }

    // 31. pollFirst
    public E PollFirst()
    {
        var e = m.PollFirstEntry();
        if (e.HasValue)
            return e.Value.Key;
        return default(E);
    }

    // 32. descendingIterator
    public List<E> DescendingIterator()
    {
        List<E> list = m.KeySet();
        list.Reverse();
        return list;
    }

    // 33. descendingSet
    public MyTreeSet<E> DescendingSet()
    {
        MyTreeSet<E> set = new MyTreeSet<E>();
        List<E> list = m.KeySet();
        list.Reverse();
        foreach (E e in list)
            set.Add(e);
        return set;
    }
}

public class Program
{
    public static void Main()
    {
        MyTreeSet<int> set = new MyTreeSet<int>();

        set.Add(5);
        set.Add(1);
        set.Add(8);
        set.Add(3);

        Console.WriteLine("Size: " + set.Size());
        Console.WriteLine("First: " + set.First());
        Console.WriteLine("Last: " + set.Last());

        Console.WriteLine("Higher(3): " + set.Higher(3));
        Console.WriteLine("Lower(3): " + set.Lower(3));

        Console.WriteLine("PollFirst: " + set.PollFirst());
        Console.WriteLine("New First: " + set.First());
    }
}
