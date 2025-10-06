using System;
using System.Collections.Generic;
public class MyArrayDeque<T>
{
    private T[] elements;
    private int head;
    private int tail;
    private int count;
    private const int DEFAULT_CAPACITY = 16;
    // 1 Конструктор по умолчанию
    public MyArrayDeque()
    {
        elements = new T[DEFAULT_CAPACITY];
        head = 0;
        tail = 0;
        count = 0;
    }
    // 2 Конструктор из массива
    public MyArrayDeque(T[] a)
    {
        elements = new T[Math.Max(DEFAULT_CAPACITY, a.Length)];
        Array.Copy(a, elements, a.Length);
        head = 0;
        tail = a.Length % elements.Length;
        count = a.Length;
    }
    // 3 Конструктор с заданной ёмкостью
    public MyArrayDeque(int numElements)
    {
        if (numElements <= 0)
            throw new ArgumentException("Размер должен быть положительным");
        elements = new T[numElements];
        head = 0;
        tail = 0;
        count = 0;
    }
    // Увеличение ёмкости (в 2 раза)
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
    // 4 Добавление элемента в конец
    public void Add(T e)
    {
        EnsureCapacity();
        elements[tail] = e;
        tail = (tail + 1) % elements.Length;
        count++;
    }
    // 5 Добавление всех элементов из массива
    public void AddAll(T[] a)
    {
        foreach (var item in a)
            Add(item);
    }
    // 6 Очистка очереди
    public void Clear()
    {
        elements = new T[DEFAULT_CAPACITY];
        head = 0;
        tail = 0;
        count = 0;
    }
    // 7 Проверка наличия элемента
    public bool Contains(object o)
    {
        for (int i = 0; i < count; i++)
        {
            if (Equals(elements[(head + i) % elements.Length], o))
                return true;
        }
        return false;
    }
    // 8 Проверка наличия всех элементов
    public bool ContainsAll(T[] a)
    {
        foreach (var item in a)
            if (!Contains(item)) return false;
        return true;
    }
    // 9 Проверка, пуста ли очередь
    public bool IsEmpty() => count == 0;
    // 10 Удаление объекта
    public bool Remove(object o)
    {
        for (int i = 0; i < count; i++)
        {
            int index = (head + i) % elements.Length;
            if (Equals(elements[index], o))
            {
                RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    // Вспомогательный метод удаления по индексу
    private void RemoveAt(int index)
    {
        for (int i = index; i < count - 1; i++)
            elements[(head + i) % elements.Length] = elements[(head + i + 1) % elements.Length];
        tail = (tail - 1 + elements.Length) % elements.Length;
        elements[tail] = default;
        count--;
    }
    // 11 Удалить все элементы из списка
    public void RemoveAll(T[] a)
    {
        foreach (var item in a)
            while (Remove(item)) { }
    }
    // 12 Оставить только указанные элементы
    public void RetainAll(T[] a)
    {
        var keep = new HashSet<T>(a);
        int newCount = 0;
        for (int i = 0; i < count; i++)
        {
            var el = elements[(head + i) % elements.Length];
            if (keep.Contains(el))
            {
                elements[(head + newCount) % elements.Length] = el;
                newCount++;
            }
        }
        count = newCount;
        tail = (head + count) % elements.Length;
    }
    // 13 Размер
    public int Size() => count;
    // 14 toArray
    public object[] ToArray()
    {
        object[] arr = new object[count];
        for (int i = 0; i < count; i++)
            arr[i] = elements[(head + i) % elements.Length];
        return arr;
    }
    // 15 toArray(T[] a)
    public T[] ToArray(T[] a)
    {
        T[] arr = a ?? new T[count];
        for (int i = 0; i < count; i++)
            arr[i] = elements[(head + i) % elements.Length];
        return arr;
    }
    // 16 element() — первый элемент без удаления
    public T Element()
    {
        if (IsEmpty()) throw new InvalidOperationException("Очередь пуста");
        return elements[head];
    }
    // 17 offer()
    public bool Offer(T obj)
    {
        Add(obj);
        return true;
    }
    // 18 peek()
    public T Peek() => IsEmpty() ? default : elements[head];
    // 19 poll()
    public T Poll()
    {
        if (IsEmpty()) return default;
        T value = elements[head];
        elements[head] = default;
        head = (head + 1) % elements.Length;
        count--;
        return value;
    }
    // 20–21 addFirst и addLast
    public void AddFirst(T obj)
    {
        EnsureCapacity();
        head = (head - 1 + elements.Length) % elements.Length;
        elements[head] = obj;
        count++;
    }
    public void AddLast(T obj) => Add(obj);
    // 22–23 getFirst и getLast
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
    // 24–25 offerFirst и offerLast
    public bool OfferFirst(T obj)
    {
        AddFirst(obj);
        return true;
    }
    public bool OfferLast(T obj)
    {
        AddLast(obj);
        return true;
    }
    // 26–27 Pop и Push
    public T Pop() => RemoveFirst();
    public void Push(T obj) => AddFirst(obj);
    // 28–29 PeekFirst  PeekLast
    public T PeekFirst() => Peek();
    public T PeekLast() => IsEmpty() ? default : GetLast();
    // 30–31 PollFirst и PollLast
    public T PollFirst() => Poll();
    public T PollLast()
    {
        if (IsEmpty()) return default;
        tail = (tail - 1 + elements.Length) % elements.Length;
        T value = elements[tail];
        elements[tail] = default;
        count--;
        return value;
    }
    // 32–33 RemoveLast и RemoveFirst
    public T RemoveLast()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Очередь пуста");
        tail = (tail - 1 + elements.Length) % elements.Length;
        T value = elements[tail];
        elements[tail] = default;
        count--;
        return value;
    }
    public T RemoveFirst()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Очередь пуста");
        T value = elements[head];
        elements[head] = default;
        head = (head + 1) % elements.Length;
        count--;
        return value;
    }
    // 34–35 RemoveLastOccurrence и RemoveFirstOccurrence
    public bool RemoveLastOccurrence(object obj)
    {
        for (int i = count - 1; i >= 0; i--)
        {
            int index = (head + i) % elements.Length;
            if (Equals(elements[index], obj))
            {
                RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    public bool RemoveFirstOccurrence(object obj)
    {
        return Remove(obj);
    }
}
public class Program
{
    public static void Main()
    {
        var deque = new MyArrayDeque<int>();
        deque.AddLast(1);
        deque.AddLast(2);
        deque.AddFirst(0);
        deque.Push(-1);
        Console.WriteLine("Первый элемент: " + deque.GetFirst());
        Console.WriteLine("Последний элемент: " + deque.GetLast());
        Console.WriteLine("Удалён первый: " + deque.RemoveFirst());
        Console.WriteLine("Удалён последний: " + deque.RemoveLast());
        Console.WriteLine("Текущий размер: " + deque.Size());
    }
}
