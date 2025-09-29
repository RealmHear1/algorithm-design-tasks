using System;
using System.Collections.Generic;
public class MyVector<T>
{
    private T[] elementData;   
    private int elementCount;    
    private int capacityIncrement;
    // 1) Конструктор с capacity и capacityIncrement
    public MyVector(int initialCapacity, int capacityIncrement)
    {
        if (initialCapacity < 0) throw new ArgumentOutOfRangeException(nameof(initialCapacity));
        this.elementData = new T[initialCapacity];
        this.elementCount = 0;
        this.capacityIncrement = capacityIncrement;
    }
    // 2) Конструктор с capacity
    public MyVector(int initialCapacity) : this(initialCapacity, 0) { }
    // 3) Конструктор без параметров
    public MyVector() : this(10, 0) { }
    // 4) Конструктор из массива
    public MyVector(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        elementData = new T[a.Length];
        Array.Copy(a, elementData, a.Length);
        elementCount = a.Length;
        capacityIncrement = 0;
    }
    // Увеличение ёмкости
    private void EnsureCapacity()
    {
        if (elementCount >= elementData.Length)
        {
            int newCapacity = (capacityIncrement > 0)
                ? elementData.Length + capacityIncrement
                : elementData.Length * 2;
            if (newCapacity == 0) newCapacity = 1;
            T[] newArray = new T[newCapacity];
            Array.Copy(elementData, newArray, elementCount);
            elementData = newArray;
        }
    }
    // 5) Добавление элемента
    public void Add(T e)
    {
        EnsureCapacity();
        elementData[elementCount++] = e;
    }
    // 6) Добавить массив
    public void AddAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        foreach (T item in a)
            Add(item);
    }
    // 7) Очистка
    public void Clear()
    {
        elementCount = 0;
        elementData = new T[10];
    }
    // 8) Содержит ли объект
    public bool Contains(object o)
    {
        return IndexOf(o) != -1;
    }
    // 9) Содержит ли все
    public bool ContainsAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        foreach (T item in a)
            if (!Contains(item)) return false;
        return true;
    }
    // 10) Пустой ли
    public bool IsEmpty()
    {
        return elementCount == 0;
    }
    // 11) Удалить объект
    public bool Remove(object o)
    {
        int index = IndexOf(o);
        if (index == -1) return false;
        Remove(index);
        return true;
    }
    // 12) Удалить все из массива
    public void RemoveAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        foreach (T item in a)
            Remove(item);
    }
    // 13) Оставить только указанные
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
    // 14) Размер
    public int Size()
    {
        return elementCount;
    }
    // 15) ToArray()
    public object[] ToArray()
    {
        object[] result = new object[elementCount];
        Array.Copy(elementData, result, elementCount);
        return result;
    }
    // 16) ToArray(T[])
    public T[] ToArray(T[] a)
    {
        if (a == null || a.Length < elementCount)
        {
            T[] result = new T[elementCount];
            Array.Copy(elementData, result, elementCount);
            return result;
        }
        else
        {
            Array.Copy(elementData, a, elementCount);
            if (a.Length > elementCount)
                a[elementCount] = default;
            return a;
        }
    }
    // 17) Добавить по индексу
    public void Add(int index, T e)
    {
        if (index < 0 || index > elementCount) throw new ArgumentOutOfRangeException(nameof(index));
        EnsureCapacity();
        Array.Copy(elementData, index, elementData, index + 1, elementCount - index);
        elementData[index] = e;
        elementCount++;
    }
    // 18) Добавить массив в позицию
    public void AddAll(int index, T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        if (index < 0 || index > elementCount) throw new ArgumentOutOfRangeException(nameof(index));
        foreach (T item in a)
        {
            Add(index++, item);
        }
    }
    // 19) Получить элемент
    public T Get(int index)
    {
        if (index < 0 || index >= elementCount) throw new ArgumentOutOfRangeException(nameof(index));
        return elementData[index];
    }
    // 20) IndexOf
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
    // 21) LastIndexOf
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
    // 22) Remove по индексу
    public T Remove(int index)
    {
        if (index < 0 || index >= elementCount) throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        Array.Copy(elementData, index + 1, elementData, index, elementCount - index - 1);
        elementCount--;
        return oldValue;
    }
    // 23) Set
    public T Set(int index, T e)
    {
        if (index < 0 || index >= elementCount) throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        elementData[index] = e;
        return oldValue;
    }
    // 24) SubList
    public MyVector<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > elementCount || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();
        int length = toIndex - fromIndex;
        T[] subArray = new T[length];
        Array.Copy(elementData, fromIndex, subArray, 0, length);
        return new MyVector<T>(subArray);
    }
    // 25) Первый элемент
    public T FirstElement()
    {
        if (IsEmpty()) throw new InvalidOperationException("Вектор пуст");
        return elementData[0];
    }
    // 26) Последний элемент
    public T LastElement()
    {
        if (IsEmpty()) throw new InvalidOperationException("Вектор пуст");
        return elementData[elementCount - 1];
    }
    // 27) RemoveElementAt
    public void RemoveElementAt(int pos)
    {
        Remove(pos);
    }
    // 28) RemoveRange
    public void RemoveRange(int begin, int end)
    {
        if (begin < 0 || end > elementCount || begin > end)
            throw new ArgumentOutOfRangeException();
        int length = end - begin;
        Array.Copy(elementData, end, elementData, begin, elementCount - end);
        elementCount -= length;
    }
}
class Program
{
    static void Main()
    {
        MyVector<int> vec = new MyVector<int>(2, 3); // начальная ёмкость = 2, приращение = 3
        vec.Add(10);
        vec.Add(20);
        vec.Add(30); 
        Console.WriteLine("Размер: " + vec.Size()); // 3
        Console.WriteLine("Первый элемент: " + vec.FirstElement()); // 10
        Console.WriteLine("Последний элемент: " + vec.LastElement()); // 30
        vec.RemoveElementAt(1); // удаляем 20
        Console.WriteLine("После удаления: " + string.Join(", ", vec.ToArray()));
    }
}

