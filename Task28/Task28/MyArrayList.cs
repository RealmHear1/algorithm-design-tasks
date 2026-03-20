using System;
using System.Collections.Generic;
public class MyArrayList<T>
{
    private T[] elementData; 
    private int size;
    // 1) Конструктор по умолчанию
    public MyArrayList()
    {
        elementData = new T[10];
        size = 0;
    }
    // 2) Конструктор из массива
    public MyArrayList(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        elementData = new T[a.Length];
        Array.Copy(a, elementData, a.Length);
        size = a.Length;
    }
    // 3) Конструктор с capacity
    public MyArrayList(int capacity)
    {
        if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        elementData = new T[capacity];
        size = 0;
    }
    private void EnsureCapacity()
    {
        if (size >= elementData.Length)
        {
            int newCapacity = elementData.Length * 3 / 2 + 1;
            if (newCapacity == 0) newCapacity = 1;
            T[] newArray = new T[newCapacity];
            Array.Copy(elementData, newArray, size);
            elementData = newArray;
        }
    }
    // 4) Добавление элемента
    public void Add(T e)
    {
        EnsureCapacity();
        elementData[size++] = e;
    }
    // 5) Добавление массива
    public void AddAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        foreach (T item in a)
            Add(item);
    }
    // 6) Очистка
    public void Clear()
    {
        size = 0;
        elementData = new T[10];
    }
    // 7) Содержит ли объект
    public bool Contains(object o)
    {
        return IndexOf(o) != -1;
    }
    // 8) Содержит ли все элементы массива
    public bool ContainsAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        foreach (T item in a)
            if (!Contains(item)) return false;
        return true;
    }
    // 9) Пустой ли
    public bool IsEmpty()
    {
        return size == 0;
    }
    // 10) Удаление по объекту
    public bool Remove(object o)
    {
        int index = IndexOf(o);
        if (index == -1) return false;
        Remove(index);
        return true;
    }
    // 11) Удаление всех элементов из массива
    public void RemoveAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        foreach (T item in a)
            Remove(item);
    }
    // 12) Оставить только указанные
    public void RetainAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        HashSet<T> set = new HashSet<T>(a);
        int newSize = 0;
        for (int i = 0; i < size; i++)
        {
            if (set.Contains(elementData[i]))
                elementData[newSize++] = elementData[i];
        }
        size = newSize;
    }
    // 13) Размер
    public int Size()
    {
        return size;
    }
    // 14) В массив объектов
    public object[] ToArray()
    {
        object[] result = new object[size];
        Array.Copy(elementData, result, size);
        return result;
    }
    // 15) В массив T[]
    public T[] ToArray(T[] a)
    {
        if (a == null || a.Length < size)
        {
            T[] result = new T[size];
            Array.Copy(elementData, result, size);
            return result;
        }
        else
        {
            Array.Copy(elementData, a, size);
            if (a.Length > size)
                a[size] = default;
            return a;
        }
    }
    // 16) Добавить в позицию
    public void Add(int index, T e)
    {
        if (index < 0 || index > size) throw new ArgumentOutOfRangeException(nameof(index));
        EnsureCapacity();
        Array.Copy(elementData, index, elementData, index + 1, size - index);
        elementData[index] = e;
        size++;
    }
    // 17) Добавить массив в позицию
    public void AddAll(int index, T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        if (index < 0 || index > size) throw new ArgumentOutOfRangeException(nameof(index));
        foreach (T item in a)
        {
            Add(index++, item);
        }
    }
    // 18) Получить элемент
    public T Get(int index)
    {
        if (index < 0 || index >= size) throw new ArgumentOutOfRangeException(nameof(index));
        return elementData[index];
    }
    // 19) Индекс объекта
    public int IndexOf(object o)
    {
        if (o == null)
        {
            for (int i = 0; i < size; i++)
                if (elementData[i] == null) return i;
        }
        else
        {
            for (int i = 0; i < size; i++)
                if (o.Equals(elementData[i])) return i;
        }
        return -1;
    }
    // 20) Последний индекс объекта
    public int LastIndexOf(object o)
    {
        if (o == null)
        {
            for (int i = size - 1; i >= 0; i--)
                if (elementData[i] == null) return i;
        }
        else
        {
            for (int i = size - 1; i >= 0; i--)
                if (o.Equals(elementData[i])) return i;
        }
        return -1;
    }
    // 21) Удалить по индексу
    public T Remove(int index)
    {
        if (index < 0 || index >= size) throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        Array.Copy(elementData, index + 1, elementData, index, size - index - 1);
        size--;
        return oldValue;
    }
    // 22) Заменить элемент
    public T Set(int index, T e)
    {
        if (index < 0 || index >= size) throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        elementData[index] = e;
        return oldValue;
    }
    // 23) Подсписок
    public MyArrayList<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > size || fromIndex > toIndex)
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
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException("index");
        return new MyItr(this, index);
    }

    private class MyItr : MyListIterator<T>
    {
        private MyArrayList<T> list;
        private int cursor;
        private int lastRet;

        public MyItr(MyArrayList<T> list, int index)
        {
            this.list = list;
            this.cursor = index;
            this.lastRet = -1;
        }

        public bool hasNext()
        {
            return cursor < list.size;
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
}


