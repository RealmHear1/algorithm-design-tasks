using System;
using System.Collections.Generic;

public class MyVector<T> : MyList<T>
{
    private T[] elementData;
    private int elementCount;
    private int capacityIncrement;

    public MyVector(int initialCapacity, int capacityIncrement)
    {
        if (initialCapacity < 0) throw new ArgumentOutOfRangeException(nameof(initialCapacity));
        elementData = new T[initialCapacity];
        elementCount = 0;
        this.capacityIncrement = capacityIncrement;
    }

    public MyVector(int initialCapacity) : this(initialCapacity, 0) { }
    public MyVector() : this(10, 0) { }

    public MyVector(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        elementData = new T[a.Length];
        Array.Copy(a, elementData, a.Length);
        elementCount = a.Length;
        capacityIncrement = 0;
    }

    public MyVector(MyCollection<T> c) : this()
    {
        addAll(c);
    }

    private void EnsureCapacity()
    {
        if (elementCount >= elementData.Length)
        {
            int newCapacity = capacityIncrement > 0
                ? elementData.Length + capacityIncrement
                : elementData.Length * 2;
            if (newCapacity == 0) newCapacity = 1;
            T[] newArray = new T[newCapacity];
            Array.Copy(elementData, newArray, elementCount);
            elementData = newArray;
        }
    }

    public void Add(T e)
    {
        EnsureCapacity();
        elementData[elementCount++] = e;
    }

    public void AddAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        for (int i = 0; i < a.Length; i++)
            Add(a[i]);
    }

    public void Clear()
    {
        elementCount = 0;
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
        return elementCount == 0;
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
        for (int i = 0; i < elementCount; i++)
        {
            if (set.Contains(elementData[i]))
                elementData[newSize++] = elementData[i];
        }
        elementCount = newSize;
    }

    public int Size()
    {
        return elementCount;
    }

    public object[] ToArray()
    {
        object[] result = new object[elementCount];
        Array.Copy(elementData, result, elementCount);
        return result;
    }

    public T[] ToArray(T[] a)
    {
        if (a == null || a.Length < elementCount)
        {
            T[] result = new T[elementCount];
            Array.Copy(elementData, result, elementCount);
            return result;
        }

        Array.Copy(elementData, a, elementCount);
        if (a.Length > elementCount)
            a[elementCount] = default(T);
        return a;
    }

    public void Add(int index, T e)
    {
        if (index < 0 || index > elementCount) throw new ArgumentOutOfRangeException(nameof(index));
        EnsureCapacity();
        Array.Copy(elementData, index, elementData, index + 1, elementCount - index);
        elementData[index] = e;
        elementCount++;
    }

    public void AddAll(int index, T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        if (index < 0 || index > elementCount) throw new ArgumentOutOfRangeException(nameof(index));
        for (int i = 0; i < a.Length; i++)
            Add(index + i, a[i]);
    }

    public T Get(int index)
    {
        if (index < 0 || index >= elementCount) throw new ArgumentOutOfRangeException(nameof(index));
        return elementData[index];
    }

    public int IndexOf(object o)
    {
        if (o == null)
        {
            for (int i = 0; i < elementCount; i++)
                if (elementData[i] == null) return i;
        }
        else
        {
            for (int i = 0; i < elementCount; i++)
                if (o.Equals(elementData[i])) return i;
        }
        return -1;
    }

    public int LastIndexOf(object o)
    {
        if (o == null)
        {
            for (int i = elementCount - 1; i >= 0; i--)
                if (elementData[i] == null) return i;
        }
        else
        {
            for (int i = elementCount - 1; i >= 0; i--)
                if (o.Equals(elementData[i])) return i;
        }
        return -1;
    }

    public T Remove(int index)
    {
        if (index < 0 || index >= elementCount) throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        Array.Copy(elementData, index + 1, elementData, index, elementCount - index - 1);
        elementCount--;
        return oldValue;
    }

    public T Set(int index, T e)
    {
        if (index < 0 || index >= elementCount) throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        elementData[index] = e;
        return oldValue;
    }

    public MyVector<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > elementCount || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();
        int length = toIndex - fromIndex;
        T[] subArray = new T[length];
        Array.Copy(elementData, fromIndex, subArray, 0, length);
        return new MyVector<T>(subArray);
    }

    public T FirstElement()
    {
        if (IsEmpty()) throw new InvalidOperationException("Вектор пуст");
        return elementData[0];
    }

    public T LastElement()
    {
        if (IsEmpty()) throw new InvalidOperationException("Вектор пуст");
        return elementData[elementCount - 1];
    }

    public void RemoveElementAt(int pos)
    {
        Remove(pos);
    }

    public void RemoveRange(int begin, int end)
    {
        if (begin < 0 || end > elementCount || begin > end)
            throw new ArgumentOutOfRangeException();
        int length = end - begin;
        Array.Copy(elementData, end, elementData, begin, elementCount - end);
        elementCount -= length;
    }

    public MyListIterator<T> listIterator()
    {
        return new MyItr(this, 0);
    }

    public MyListIterator<T> listIterator(int index)
    {
        if (index < 0 || index > elementCount)
            throw new ArgumentOutOfRangeException("index");
        return new MyItr(this, index);
    }

    private class MyItr : MyListIterator<T>
    {
        private readonly MyVector<T> list;
        private int cursor;
        private int lastRet;

        public MyItr(MyVector<T> list, int index)
        {
            this.list = list;
            cursor = index;
            lastRet = -1;
        }

        public bool hasNext() { return cursor < list.elementCount; }

        public T next()
        {
            if (!hasNext()) throw new NoSuchElementException();
            lastRet = cursor;
            cursor++;
            return list.elementData[lastRet];
        }

        public bool hasPrevious() { return cursor > 0; }

        public T previous()
        {
            if (!hasPrevious()) throw new NoSuchElementException();
            cursor--;
            lastRet = cursor;
            return list.elementData[lastRet];
        }

        public int nextIndex() { return cursor; }
        public int previousIndex() { return cursor - 1; }

        public void remove()
        {
            if (lastRet < 0) throw new IllegalStateException();
            list.Remove(lastRet);
            if (lastRet < cursor) cursor--;
            lastRet = -1;
        }

        public void set(T element)
        {
            if (lastRet < 0) throw new IllegalStateException();
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
        for (int i = 0; i < arr.Length; i++) Add((T)arr[i]);
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
        for (int i = 0; i < arr.Length; i++) set.Add((T)arr[i]);

        int oldSize = elementCount;
        int newSize = 0;
        for (int i = 0; i < elementCount; i++)
        {
            if (set.Contains(elementData[i]))
                elementData[newSize++] = elementData[i];
        }
        elementCount = newSize;
        return oldSize != elementCount;
    }

    public int size() { return Size(); }
    public object[] toArray() { return ToArray(); }
    public T[] toArray(T[] a) { return ToArray(a); }
    public void add(int index, T e) { Add(index, e); }

    public bool addAll(int index, MyCollection<T> c)
    {
        if (c == null) throw new ArgumentNullException(nameof(c));
        object[] arr = c.toArray();
        for (int i = 0; i < arr.Length; i++) Add(index + i, (T)arr[i]);
        return arr.Length > 0;
    }

    public T get(int index) { return Get(index); }
    public int indexOf(object o) { return IndexOf(o); }
    public int lastIndexOf(object o) { return LastIndexOf(o); }
    public T remove(int index) { return Remove(index); }
    public T set(int index, T e) { return Set(index, e); }
    public MyList<T> subList(int fromIndex, int toIndex) { return SubList(fromIndex, toIndex); }
}
