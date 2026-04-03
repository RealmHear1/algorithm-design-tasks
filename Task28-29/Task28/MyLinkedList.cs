using System;
using System.Collections.Generic;

public class MyLinkedList<T> : MyList<T>
{
    private class Node
    {
        public T Data;
        public Node Prev;
        public Node Next;

        public Node(T data)
        {
            Data = data;
        }
    }

    private Node first;
    private Node last;
    private int sizeValue;

    public MyLinkedList()
    {
        first = null;
        last = null;
        sizeValue = 0;
    }

    public MyLinkedList(T[] a) : this()
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        for (int i = 0; i < a.Length; i++)
            Add(a[i]);
    }

    public MyLinkedList(MyCollection<T> c) : this()
    {
        addAll(c);
    }

    public void Add(T e)
    {
        Node node = new Node(e);
        if (last == null)
        {
            first = last = node;
        }
        else
        {
            last.Next = node;
            node.Prev = last;
            last = node;
        }
        sizeValue++;
    }

    public void AddAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        for (int i = 0; i < a.Length; i++)
            Add(a[i]);
    }

    public void Clear()
    {
        first = null;
        last = null;
        sizeValue = 0;
    }

    public bool Contains(object o)
    {
        for (Node cur = first; cur != null; cur = cur.Next)
        {
            if (Equals(cur.Data, o))
                return true;
        }
        return false;
    }

    public bool ContainsAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        for (int i = 0; i < a.Length; i++)
            if (!Contains(a[i])) return false;
        return true;
    }

    public bool IsEmpty() { return sizeValue == 0; }
    public int Size() { return sizeValue; }

    public bool Remove(object o)
    {
        for (Node cur = first; cur != null; cur = cur.Next)
        {
            if (Equals(cur.Data, o))
            {
                RemoveNode(cur);
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
        for (Node cur = first; cur != null;)
        {
            Node next = cur.Next;
            if (Array.IndexOf(a, cur.Data) == -1)
                RemoveNode(cur);
            cur = next;
        }
    }

    private void RemoveNode(Node node)
    {
        if (node.Prev != null) node.Prev.Next = node.Next;
        else first = node.Next;

        if (node.Next != null) node.Next.Prev = node.Prev;
        else last = node.Prev;

        sizeValue--;
    }

    public T[] ToArray()
    {
        T[] arr = new T[sizeValue];
        int i = 0;
        for (Node cur = first; cur != null; cur = cur.Next)
            arr[i++] = cur.Data;
        return arr;
    }

    public T[] ToArray(T[] a)
    {
        if (a == null || a.Length < sizeValue)
            a = new T[sizeValue];
        int i = 0;
        for (Node cur = first; cur != null; cur = cur.Next)
            a[i++] = cur.Data;
        if (a.Length > sizeValue)
            a[sizeValue] = default(T);
        return a;
    }

    private Node GetNode(int index)
    {
        if (index < 0 || index >= sizeValue) throw new ArgumentOutOfRangeException();
        Node cur = first;
        for (int i = 0; i < index; i++) cur = cur.Next;
        return cur;
    }

    public T Get(int index) { return GetNode(index).Data; }

    public void Add(int index, T e)
    {
        if (index < 0 || index > sizeValue) throw new ArgumentOutOfRangeException();
        if (index == sizeValue) { Add(e); return; }
        if (index == 0) { AddFirst(e); return; }
        Node nextNode = GetNode(index);
        Node newNode = new Node(e);
        Node prev = nextNode.Prev;
        prev.Next = newNode;
        newNode.Prev = prev;
        newNode.Next = nextNode;
        nextNode.Prev = newNode;
        sizeValue++;
    }

    public void AddAll(int index, T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        for (int i = 0; i < a.Length; i++)
            Add(index + i, a[i]);
    }

    public int IndexOf(object o)
    {
        int i = 0;
        for (Node cur = first; cur != null; cur = cur.Next)
        {
            if (Equals(cur.Data, o)) return i;
            i++;
        }
        return -1;
    }

    public int LastIndexOf(object o)
    {
        int i = sizeValue - 1;
        for (Node cur = last; cur != null; cur = cur.Prev)
        {
            if (Equals(cur.Data, o)) return i;
            i--;
        }
        return -1;
    }

    public T Remove(int index)
    {
        Node node = GetNode(index);
        T value = node.Data;
        RemoveNode(node);
        return value;
    }

    public T Set(int index, T e)
    {
        Node node = GetNode(index);
        T old = node.Data;
        node.Data = e;
        return old;
    }

    public MyLinkedList<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > sizeValue || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();
        MyLinkedList<T> list = new MyLinkedList<T>();
        Node cur = GetNode(fromIndex);
        for (int i = fromIndex; i < toIndex; i++)
        {
            list.Add(cur.Data);
            cur = cur.Next;
        }
        return list;
    }

    public T Element() { return GetFirst(); }
    public bool Offer(T obj) { AddLast(obj); return true; }
    public T Peek() { return IsEmpty() ? default(T) : GetFirst(); }

    public T Poll()
    {
        if (IsEmpty()) return default(T);
        T val = GetFirst();
        RemoveFirst();
        return val;
    }

    public void AddFirst(T obj)
    {
        Node node = new Node(obj);
        if (first == null)
        {
            first = last = node;
        }
        else
        {
            node.Next = first;
            first.Prev = node;
            first = node;
        }
        sizeValue++;
    }

    public void AddLast(T obj) { Add(obj); }

    public T GetFirst()
    {
        if (IsEmpty()) throw new InvalidOperationException("Список пуст");
        return first.Data;
    }

    public T GetLast()
    {
        if (IsEmpty()) throw new InvalidOperationException("Список пуст");
        return last.Data;
    }

    public bool OfferFirst(T obj) { AddFirst(obj); return true; }
    public bool OfferLast(T obj) { AddLast(obj); return true; }
    public T Pop() { return PollFirst(); }
    public void Push(T obj) { AddFirst(obj); }
    public T PeekFirst() { return Peek(); }
    public T PeekLast() { return IsEmpty() ? default(T) : GetLast(); }
    public T PollFirst() { return Poll(); }

    public T PollLast()
    {
        if (IsEmpty()) return default(T);
        T val = GetLast();
        RemoveLast();
        return val;
    }

    public T RemoveFirst() { return Remove(0); }
    public T RemoveLast() { return Remove(sizeValue - 1); }

    public bool RemoveFirstOccurrence(object obj)
    {
        for (Node cur = first; cur != null; cur = cur.Next)
        {
            if (Equals(cur.Data, obj))
            {
                RemoveNode(cur);
                return true;
            }
        }
        return false;
    }

    public bool RemoveLastOccurrence(object obj)
    {
        for (Node cur = last; cur != null; cur = cur.Prev)
        {
            if (Equals(cur.Data, obj))
            {
                RemoveNode(cur);
                return true;
            }
        }
        return false;
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
        private readonly MyLinkedList<T> list;
        private int cursor;
        private int lastRet;

        public MyItr(MyLinkedList<T> list, int index)
        {
            this.list = list;
            cursor = index;
            lastRet = -1;
        }

        public bool hasNext() { return cursor < list.sizeValue; }

        public T next()
        {
            if (!hasNext()) throw new NoSuchElementException();
            lastRet = cursor;
            cursor++;
            return list.Get(lastRet);
        }

        public bool hasPrevious() { return cursor > 0; }

        public T previous()
        {
            if (!hasPrevious()) throw new NoSuchElementException();
            cursor--;
            lastRet = cursor;
            return list.Get(lastRet);
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

        bool changed = false;
        for (Node cur = first; cur != null;)
        {
            Node next = cur.Next;
            if (!keep.Contains(cur.Data))
            {
                RemoveNode(cur);
                changed = true;
            }
            cur = next;
        }
        return changed;
    }

    public int size() { return Size(); }

    public object[] toArray()
    {
        T[] arr = ToArray();
        object[] result = new object[arr.Length];
        for (int i = 0; i < arr.Length; i++) result[i] = arr[i];
        return result;
    }

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
