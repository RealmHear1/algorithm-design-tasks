using System;
using System.Collections.Generic;

public class MyArrayDeque<T> : MyList<T>, MyDeque<T>
{
    private T[] elements;
    private int head;
    private int tail;
    private int count;
    private const int DEFAULT_CAPACITY = 16;

    public MyArrayDeque()
    {
        elements = new T[DEFAULT_CAPACITY];
        head = 0;
        tail = 0;
        count = 0;
    }

    public MyArrayDeque(T[] a)
        : this()
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        AddAll(a);
    }

    public MyArrayDeque(MyCollection<T> c)
        : this()
    {
        addAll(c);
    }

    public MyArrayDeque(int numElements)
    {
        if (numElements <= 0)
            throw new ArgumentException("Размер должен быть положительным");
        elements = new T[numElements];
        head = 0;
        tail = 0;
        count = 0;
    }

    private void EnsureCapacity()
    {
        if (count < elements.Length) return;
        int newCapacity = elements.Length * 2;
        T[] newArray = new T[newCapacity];
        for (int i = 0; i < count; i++)
            newArray[i] = elements[(head + i) % elements.Length];
        elements = newArray;
        head = 0;
        tail = count;
    }

    private void Normalize()
    {
        if (head == 0 && (count == 0 || tail == count))
            return;

        T[] newArray = new T[elements.Length];
        for (int i = 0; i < count; i++)
            newArray[i] = elements[(head + i) % elements.Length];
        elements = newArray;
        head = 0;
        tail = count;
    }

    private void RemoveAtLogical(int index)
    {
        Normalize();
        Array.Copy(elements, index + 1, elements, index, count - index - 1);
        count--;
        tail = count;
        elements[tail] = default(T);
    }

    public void Add(T e)
    {
        EnsureCapacity();
        elements[tail] = e;
        tail = (tail + 1) % elements.Length;
        count++;
    }

    public void AddAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        for (int i = 0; i < a.Length; i++)
            Add(a[i]);
    }

    public void Clear()
    {
        elements = new T[DEFAULT_CAPACITY];
        head = 0;
        tail = 0;
        count = 0;
    }

    public bool Contains(object o)
    {
        for (int i = 0; i < count; i++)
            if (Equals(elements[(head + i) % elements.Length], o))
                return true;
        return false;
    }

    public bool ContainsAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        for (int i = 0; i < a.Length; i++)
            if (!Contains(a[i])) return false;
        return true;
    }

    public bool IsEmpty() { return count == 0; }

    public bool Remove(object o)
    {
        for (int i = 0; i < count; i++)
        {
            int index = (head + i) % elements.Length;
            if (Equals(elements[index], o))
            {
                RemoveAtLogical(i);
                return true;
            }
        }
        return false;
    }

    public void RemoveAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        for (int i = 0; i < a.Length; i++)
            while (Remove(a[i])) { }
    }

    public void RetainAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        HashSet<T> keep = new HashSet<T>(a);
        Normalize();
        int newCount = 0;
        for (int i = 0; i < count; i++)
        {
            if (keep.Contains(elements[i]))
                elements[newCount++] = elements[i];
        }
        for (int i = newCount; i < count; i++)
            elements[i] = default(T);
        count = newCount;
        tail = count;
    }

    public int Size() { return count; }

    public object[] ToArray()
    {
        object[] arr = new object[count];
        for (int i = 0; i < count; i++)
            arr[i] = elements[(head + i) % elements.Length];
        return arr;
    }

    public T[] ToArray(T[] a)
    {
        if (a == null || a.Length < count)
            a = new T[count];
        for (int i = 0; i < count; i++)
            a[i] = elements[(head + i) % elements.Length];
        if (a.Length > count)
            a[count] = default(T);
        return a;
    }

    public T Element()
    {
        if (IsEmpty()) throw new InvalidOperationException("Очередь пуста");
        return elements[head];
    }

    public bool Offer(T obj)
    {
        Add(obj);
        return true;
    }

    public T Peek() { return IsEmpty() ? default(T) : elements[head]; }

    public T Poll()
    {
        if (IsEmpty()) return default(T);
        T value = elements[head];
        elements[head] = default(T);
        head = (head + 1) % elements.Length;
        count--;
        if (count == 0)
        {
            head = 0;
            tail = 0;
        }
        return value;
    }

    public void AddFirst(T obj)
    {
        EnsureCapacity();
        head = (head - 1 + elements.Length) % elements.Length;
        elements[head] = obj;
        count++;
    }

    public void AddLast(T obj) { Add(obj); }

    public T GetFirst()
    {
        if (IsEmpty()) throw new InvalidOperationException("Очередь пуста");
        return elements[head];
    }

    public T GetLast()
    {
        if (IsEmpty()) throw new InvalidOperationException("Очередь пуста");
        int lastIndex = (tail - 1 + elements.Length) % elements.Length;
        return elements[lastIndex];
    }

    public bool OfferFirst(T obj) { AddFirst(obj); return true; }
    public bool OfferLast(T obj) { AddLast(obj); return true; }
    public T Pop() { return RemoveFirst(); }
    public void Push(T obj) { AddFirst(obj); }
    public T PeekFirst() { return Peek(); }
    public T PeekLast() { return IsEmpty() ? default(T) : GetLast(); }
    public T PollFirst() { return Poll(); }

    public T PollLast()
    {
        if (IsEmpty()) return default(T);
        tail = (tail - 1 + elements.Length) % elements.Length;
        T value = elements[tail];
        elements[tail] = default(T);
        count--;
        if (count == 0)
        {
            head = 0;
            tail = 0;
        }
        return value;
    }

    public T RemoveLast()
    {
        if (IsEmpty()) throw new InvalidOperationException("Очередь пуста");
        return PollLast();
    }

    public T RemoveFirst()
    {
        if (IsEmpty()) throw new InvalidOperationException("Очередь пуста");
        return PollFirst();
    }

    public bool RemoveLastOccurrence(object obj)
    {
        for (int i = count - 1; i >= 0; i--)
        {
            if (Equals(Get(i), obj))
            {
                RemoveAtLogical(i);
                return true;
            }
        }
        return false;
    }

    public bool RemoveFirstOccurrence(object obj)
    {
        return Remove(obj);
    }

    public T Get(int index)
    {
        if (index < 0 || index >= count) throw new ArgumentOutOfRangeException(nameof(index));
        return elements[(head + index) % elements.Length];
    }

    public void Add(int index, T e)
    {
        if (index < 0 || index > count) throw new ArgumentOutOfRangeException(nameof(index));
        if (index == count)
        {
            Add(e);
            return;
        }
        if (index == 0)
        {
            AddFirst(e);
            return;
        }

        EnsureCapacity();
        Normalize();
        Array.Copy(elements, index, elements, index + 1, count - index);
        elements[index] = e;
        count++;
        tail = count;
    }

    public void AddAll(int index, T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        for (int i = 0; i < a.Length; i++)
            Add(index + i, a[i]);
    }

    public int IndexOf(object o)
    {
        for (int i = 0; i < count; i++)
            if (Equals(Get(i), o))
                return i;
        return -1;
    }

    public int LastIndexOf(object o)
    {
        for (int i = count - 1; i >= 0; i--)
            if (Equals(Get(i), o))
                return i;
        return -1;
    }

    public T Remove(int index)
    {
        if (index < 0 || index >= count) throw new ArgumentOutOfRangeException(nameof(index));
        T value = Get(index);
        if (index == 0) return RemoveFirst();
        if (index == count - 1) return RemoveLast();
        RemoveAtLogical(index);
        return value;
    }

    public T Set(int index, T e)
    {
        if (index < 0 || index >= count) throw new ArgumentOutOfRangeException(nameof(index));
        Normalize();
        T old = elements[index];
        elements[index] = e;
        return old;
    }

    public MyArrayList<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > count || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();

        MyArrayList<T> list = new MyArrayList<T>();
        for (int i = fromIndex; i < toIndex; i++)
            list.add(Get(i));
        return list;
    }

    public MyIterator<T> iterator()
    {
        return new MyItr(this);
    }

    public MyListIterator<T> listIterator()
    {
        return new MyListItr(this, 0);
    }

    public MyListIterator<T> listIterator(int index)
    {
        if (index < 0 || index > count)
            throw new ArgumentOutOfRangeException("index");
        return new MyListItr(this, index);
    }

    private class MyItr : MyIterator<T>
    {
        private readonly MyArrayDeque<T> deque;
        private int cursor;
        private int lastRet = -1;

        public MyItr(MyArrayDeque<T> d)
        {
            deque = d;
        }

        public bool hasNext() { return cursor < deque.count; }

        public T next()
        {
            if (!hasNext()) throw new NoSuchElementException();
            lastRet = cursor;
            return deque.Get(cursor++);
        }

        public void remove()
        {
            if (lastRet < 0) throw new IllegalStateException();
            deque.Remove(lastRet);
            cursor = lastRet;
            lastRet = -1;
        }
    }

    private class MyListItr : MyListIterator<T>
    {
        private readonly MyArrayDeque<T> deque;
        private int cursor;
        private int lastRet = -1;

        public MyListItr(MyArrayDeque<T> deque, int index)
        {
            this.deque = deque;
            cursor = index;
        }

        public bool hasNext() { return cursor < deque.count; }
        public bool hasPrevious() { return cursor > 0; }

        public T next()
        {
            if (!hasNext()) throw new NoSuchElementException();
            lastRet = cursor;
            return deque.Get(cursor++);
        }

        public T previous()
        {
            if (!hasPrevious()) throw new NoSuchElementException();
            cursor--;
            lastRet = cursor;
            return deque.Get(cursor);
        }

        public int nextIndex() { return cursor; }
        public int previousIndex() { return cursor - 1; }

        public void remove()
        {
            if (lastRet < 0) throw new IllegalStateException();
            deque.Remove(lastRet);
            if (lastRet < cursor) cursor--;
            lastRet = -1;
        }

        public void set(T element)
        {
            if (lastRet < 0) throw new IllegalStateException();
            deque.Set(lastRet, element);
        }

        public void add(T element)
        {
            deque.Add(cursor, element);
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
        {
            while (Remove(arr[i]))
                changed = true;
        }
        return changed;
    }

    public bool retainAll(MyCollection<T> c)
    {
        if (c == null) throw new ArgumentNullException(nameof(c));
        HashSet<T> keep = new HashSet<T>();
        object[] arr = c.toArray();
        for (int i = 0; i < arr.Length; i++) keep.Add((T)arr[i]);

        int oldCount = count;
        Normalize();
        int newCount = 0;
        for (int i = 0; i < count; i++)
        {
            if (keep.Contains(elements[i]))
                elements[newCount++] = elements[i];
        }
        for (int i = newCount; i < count; i++)
            elements[i] = default(T);
        count = newCount;
        tail = count;
        return oldCount != count;
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

    public void addFirst(T obj) { AddFirst(obj); }
    public void addLast(T obj) { AddLast(obj); }
    public T getFirst() { return GetFirst(); }
    public T getLast() { return GetLast(); }
    public bool offerFirst(T obj) { return OfferFirst(obj); }
    public bool offerLast(T obj) { return OfferLast(obj); }
    public T pop() { return Pop(); }
    public void push(T obj) { Push(obj); }
    public T peekFirst() { return PeekFirst(); }
    public T peekLast() { return PeekLast(); }
    public T pollFirst() { return PollFirst(); }
    public T pollLast() { return PollLast(); }
    public T removeLast() { return RemoveLast(); }
    public T removeFirst() { return RemoveFirst(); }
    public bool removeLastOccurrence(object obj) { return RemoveLastOccurrence(obj); }
    public bool removeFirstOccurrence(object obj) { return RemoveFirstOccurrence(obj); }
}
