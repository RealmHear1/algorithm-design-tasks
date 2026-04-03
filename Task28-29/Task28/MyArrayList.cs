using System;
using System.Collections.Generic;

public class MyArrayList<T> : MyList<T>
{
    private T[] elementData;
    private int sizeValue;

    public MyArrayList()
    {
        elementData = new T[10];
        sizeValue = 0;
    }

    public MyArrayList(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        elementData = new T[a.Length];
        Array.Copy(a, elementData, a.Length);
        sizeValue = a.Length;
    }

    public MyArrayList(MyCollection<T> c)
    {
        if (c == null) throw new ArgumentNullException(nameof(c));
        object[] arr = c.toArray();
        elementData = new T[Math.Max(10, arr.Length)];
        for (int i = 0; i < arr.Length; i++)
            elementData[i] = (T)arr[i];
        sizeValue = arr.Length;
    }

    public MyArrayList(int capacity)
    {
        if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        elementData = new T[capacity];
        sizeValue = 0;
    }

    private void EnsureCapacity()
    {
        if (sizeValue >= elementData.Length)
        {
            int newCapacity = elementData.Length * 3 / 2 + 1;
            if (newCapacity == 0) newCapacity = 1;
            T[] newArray = new T[newCapacity];
            Array.Copy(elementData, newArray, sizeValue);
            elementData = newArray;
        }
    }

    public void Add(T e)
    {
        EnsureCapacity();
        elementData[sizeValue++] = e;
    }

    public void AddAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        for (int i = 0; i < a.Length; i++)
            Add(a[i]);
    }

    public void Clear()
    {
        sizeValue = 0;
        elementData = new T[10];
    }

    public bool Contains(object o)
    {
        return IndexOf(o) != -1;
    }

    public bool ContainsAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        for (int i = 0; i < a.Length; i++)
            if (!Contains(a[i])) return false;
        return true;
    }

    public bool IsEmpty()
    {
        return sizeValue == 0;
    }

    public bool Remove(object o)
    {
        int index = IndexOf(o);
        if (index == -1) return false;
        Remove(index);
        return true;
    }

    public void RemoveAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        for (int i = 0; i < a.Length; i++)
            Remove(a[i]);
    }

    public void RetainAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        HashSet<T> set = new HashSet<T>(a);
        int newSize = 0;
        for (int i = 0; i < sizeValue; i++)
        {
            if (set.Contains(elementData[i]))
                elementData[newSize++] = elementData[i];
        }
        sizeValue = newSize;
    }

    public int Size()
    {
        return sizeValue;
    }

    public object[] ToArray()
    {
        object[] result = new object[sizeValue];
        Array.Copy(elementData, result, sizeValue);
        return result;
    }

    public T[] ToArray(T[] a)
    {
        if (a == null || a.Length < sizeValue)
        {
            T[] result = new T[sizeValue];
            Array.Copy(elementData, result, sizeValue);
            return result;
        }

        Array.Copy(elementData, a, sizeValue);
        if (a.Length > sizeValue)
            a[sizeValue] = default(T);
        return a;
    }

    public void Add(int index, T e)
    {
        if (index < 0 || index > sizeValue) throw new ArgumentOutOfRangeException(nameof(index));
        EnsureCapacity();
        Array.Copy(elementData, index, elementData, index + 1, sizeValue - index);
        elementData[index] = e;
        sizeValue++;
    }

    public void AddAll(int index, T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        if (index < 0 || index > sizeValue) throw new ArgumentOutOfRangeException(nameof(index));
        for (int i = 0; i < a.Length; i++)
            Add(index + i, a[i]);
    }

    public T Get(int index)
    {
        if (index < 0 || index >= sizeValue) throw new ArgumentOutOfRangeException(nameof(index));
        return elementData[index];
    }

    public int IndexOf(object o)
    {
        if (o == null)
        {
            for (int i = 0; i < sizeValue; i++)
                if (elementData[i] == null) return i;
        }
        else
        {
            for (int i = 0; i < sizeValue; i++)
                if (o.Equals(elementData[i])) return i;
        }
        return -1;
    }

    public int LastIndexOf(object o)
    {
        if (o == null)
        {
            for (int i = sizeValue - 1; i >= 0; i--)
                if (elementData[i] == null) return i;
        }
        else
        {
            for (int i = sizeValue - 1; i >= 0; i--)
                if (o.Equals(elementData[i])) return i;
        }
        return -1;
    }

    public T Remove(int index)
    {
        if (index < 0 || index >= sizeValue) throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        Array.Copy(elementData, index + 1, elementData, index, sizeValue - index - 1);
        sizeValue--;
        return oldValue;
    }

    public T Set(int index, T e)
    {
        if (index < 0 || index >= sizeValue) throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        elementData[index] = e;
        return oldValue;
    }

    public MyArrayList<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > sizeValue || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();
        int length = toIndex - fromIndex;
        T[] subArray = new T[length];
        Array.Copy(elementData, fromIndex, subArray, 0, length);
        return new MyArrayList<T>(subArray);
    }

    public MyListIterator<T> listIterator()
    {
        return new MyItr(this, 0);
    }

    public MyListIterator<T> listIterator(int index)
    {
        if (index < 0 || index > sizeValue)
            throw new ArgumentOutOfRangeException("index");
        return new MyItr(this, index);
    }

    private class MyItr : MyListIterator<T>
    {
        private readonly MyArrayList<T> list;
        private int cursor;
        private int lastRet;

        public MyItr(MyArrayList<T> list, int index)
        {
            this.list = list;
            cursor = index;
            lastRet = -1;
        }

        public bool hasNext()
        {
            return cursor < list.sizeValue;
        }

        public T next()
        {
            if (!hasNext())
                throw new NoSuchElementException();

            lastRet = cursor;
            cursor++;
            return list.elementData[lastRet];
        }

        public bool hasPrevious()
        {
            return cursor > 0;
        }

        public T previous()
        {
            if (!hasPrevious())
                throw new NoSuchElementException();

            cursor--;
            lastRet = cursor;
            return list.elementData[lastRet];
        }

        public int nextIndex()
        {
            return cursor;
        }

        public int previousIndex()
        {
            return cursor - 1;
        }

        public void remove()
        {
            if (lastRet < 0)
                throw new IllegalStateException();

            list.Remove(lastRet);
            if (lastRet < cursor)
                cursor--;
            lastRet = -1;
        }

        public void set(T element)
        {
            if (lastRet < 0)
                throw new IllegalStateException();

            list.Set(lastRet, element);
        }

        public void add(T element)
        {
            list.Add(cursor, element);
            cursor++;
            lastRet = -1;
        }
    }

    public bool add(T e) { Add(e); return true; }

    public bool addAll(MyCollection<T> c)
    {
        if (c == null) throw new ArgumentNullException(nameof(c));
        object[] arr = c.toArray();
        for (int i = 0; i < arr.Length; i++)
            Add((T)arr[i]);
        return arr.Length > 0;
    }

    public void clear() { Clear(); }
    public bool contains(object o) { return Contains(o); }

    public bool containsAll(MyCollection<T> c)
    {
        if (c == null) throw new ArgumentNullException(nameof(c));
        object[] arr = c.toArray();
        for (int i = 0; i < arr.Length; i++)
            if (!Contains(arr[i])) return false;
        return true;
    }

    public bool isEmpty() { return IsEmpty(); }
    public bool remove(object o) { return Remove(o); }

    public bool removeAll(MyCollection<T> c)
    {
        if (c == null) throw new ArgumentNullException(nameof(c));
        bool changed = false;
        object[] arr = c.toArray();
        for (int i = 0; i < arr.Length; i++)
            if (Remove(arr[i])) changed = true;
        return changed;
    }

    public bool retainAll(MyCollection<T> c)
    {
        if (c == null) throw new ArgumentNullException(nameof(c));
        HashSet<T> set = new HashSet<T>();
        object[] arr = c.toArray();
        for (int i = 0; i < arr.Length; i++)
            set.Add((T)arr[i]);

        int oldSize = sizeValue;
        int newSize = 0;
        for (int i = 0; i < sizeValue; i++)
        {
            if (set.Contains(elementData[i]))
                elementData[newSize++] = elementData[i];
        }
        sizeValue = newSize;
        return oldSize != sizeValue;
    }

    public int size() { return Size(); }
    public object[] toArray() { return ToArray(); }
    public T[] toArray(T[] a) { return ToArray(a); }
    public void add(int index, T e) { Add(index, e); }

    public bool addAll(int index, MyCollection<T> c)
    {
        if (c == null) throw new ArgumentNullException(nameof(c));
        object[] arr = c.toArray();
        for (int i = 0; i < arr.Length; i++)
            Add(index + i, (T)arr[i]);
        return arr.Length > 0;
    }

    public T get(int index) { return Get(index); }
    public int indexOf(object o) { return IndexOf(o); }
    public int lastIndexOf(object o) { return LastIndexOf(o); }
    public T remove(int index) { return Remove(index); }
    public T set(int index, T e) { return Set(index, e); }
    public MyList<T> subList(int fromIndex, int toIndex) { return SubList(fromIndex, toIndex); }
}
