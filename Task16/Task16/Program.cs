using System;
public class MyLinkedList<T>
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
    private int size;
    // Конструкторы
    public MyLinkedList()
    {
        first = null;
        last = null;
        size = 0;
    }
    public MyLinkedList(T[] a)
    {
        foreach (var item in a)
            Add(item);
    }
    // Базовые методы
    public void Add(T e)
    {
        var node = new Node(e);
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
        size++;
    }
    public void AddAll(T[] a)
    {
        foreach (var item in a)
            Add(item);
    }
    public void Clear()
    {
        first = last = null;
        size = 0;
    }
    public bool Contains(object o)
    {
        for (var cur = first; cur != null; cur = cur.Next)
        {
            if (Equals(cur.Data, o))
                return true;
        }
        return false;
    }
    public bool ContainsAll(T[] a)
    {
        foreach (var item in a)
        {
            if (!Contains(item)) return false;
        }
        return true;
    }
    public bool IsEmpty() => size == 0;
    public int Size() => size;
    public bool Remove(object o)
    {
        for (var cur = first; cur != null; cur = cur.Next)
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
        foreach (var item in a)
            while (Remove(item)) { }
    }
    public void RetainAll(T[] a)
    {
        for (var cur = first; cur != null;)
        {
            var next = cur.Next;
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

        size--;
    }
    public T[] ToArray()
    {
        var arr = new T[size];
        int i = 0;
        for (var cur = first; cur != null; cur = cur.Next)
            arr[i++] = cur.Data;
        return arr;
    }
    public T[] ToArray(T[] a)
    {
        if (a == null || a.Length < size)
            a = new T[size];
        int i = 0;
        for (var cur = first; cur != null; cur = cur.Next)
            a[i++] = cur.Data;
        return a;
    }
    // Методы по индексам
    private Node GetNode(int index)
    {
        if (index < 0 || index >= size) throw new ArgumentOutOfRangeException();
        var cur = first;
        for (int i = 0; i < index; i++) cur = cur.Next;
        return cur;
    }
    public T Get(int index) => GetNode(index).Data;
    public void Add(int index, T e)
    {
        if (index < 0 || index > size) throw new ArgumentOutOfRangeException();
        if (index == size) { Add(e); return; }
        if (index == 0) { AddFirst(e); return; }
        var nextNode = GetNode(index);
        var newNode = new Node(e);
        var prev = nextNode.Prev;
        prev.Next = newNode;
        newNode.Prev = prev;
        newNode.Next = nextNode;
        nextNode.Prev = newNode;
        size++;
    }
    public void AddAll(int index, T[] a)
    {
        foreach (var item in a)
        {
            Add(index++, item);
        }
    }
    public int IndexOf(object o)
    {
        int i = 0;
        for (var cur = first; cur != null; cur = cur.Next)
        {
            if (Equals(cur.Data, o)) return i;
            i++;
        }
        return -1;
    }
    public int LastIndexOf(object o)
    {
        int i = size - 1;
        for (var cur = last; cur != null; cur = cur.Prev)
        {
            if (Equals(cur.Data, o)) return i;
            i--;
        }
        return -1;
    }
    public T Remove(int index)
    {
        var node = GetNode(index);
        var value = node.Data;
        RemoveNode(node);
        return value;
    }
    public void Set(int index, T e)
    {
        GetNode(index).Data = e;
    }
    public MyLinkedList<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > size || fromIndex >= toIndex)
            throw new ArgumentOutOfRangeException();
        var list = new MyLinkedList<T>();
        var cur = GetNode(fromIndex);
        for (int i = fromIndex; i < toIndex; i++)
        {
            list.Add(cur.Data);
            cur = cur.Next;
        }
        return list;
    }
    // Методы для очереди 
    public T Element() => GetFirst();
    public bool Offer(T obj) { AddLast(obj); return true; }
    public T Peek() => IsEmpty() ? default : GetFirst();
    public T Poll()
    {
        if (IsEmpty()) return default;
        var val = GetFirst();
        RemoveFirst();
        return val;
    }
    public void AddFirst(T obj)
    {
        var node = new Node(obj);
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
        size++;
    }
    public void AddLast(T obj) => Add(obj);
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
    public T Pop() => PollFirst();
    public void Push(T obj) => AddFirst(obj);
    public T PeekFirst() => Peek();
    public T PeekLast() => IsEmpty() ? default : GetLast();
    public T PollFirst() => Poll();
    public T PollLast()
    {
        if (IsEmpty()) return default;
        var val = GetLast();
        RemoveLast();
        return val;
    }
    public T RemoveFirst() => Remove(0);
    public T RemoveLast() => Remove(size - 1);
    public bool RemoveFirstOccurrence(object obj)
    {
        var cur = first;
        while (cur != null)
        {
            if (Equals(cur.Data, obj))
            {
                RemoveNode(cur);
                return true;
            }
            cur = cur.Next;
        }
        return false;
    }
    public bool RemoveLastOccurrence(object obj)
    {
        var cur = last;
        while (cur != null)
        {
            if (Equals(cur.Data, obj))
            {
                RemoveNode(cur);
                return true;
            }
            cur = cur.Prev;
        }
        return false;
    }
}
class Program
{
    static void Main()
    {
        var list = new MyLinkedList<string>(new string[] { "a", "b", "c" });
        list.AddFirst("start");
        list.AddLast("end");
        Console.WriteLine("Все элементы:");
        foreach (var s in list.ToArray())
            Console.WriteLine(s);
        Console.WriteLine($"\nПервый: {list.GetFirst()}");
        Console.WriteLine($"Последний: {list.GetLast()}");
        list.Remove("b");
        Console.WriteLine("\nПосле удаления 'b':");
        foreach (var s in list.ToArray())
            Console.WriteLine(s);
    }
}
