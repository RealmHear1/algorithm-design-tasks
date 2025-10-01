using System;
public class MyStack<T> : MyVector<T>
{
    // Конструкторы наследуются от MyVector
    public MyStack() : base() { }
    public MyStack(int initialCapacity) : base(initialCapacity) { }
    public MyStack(int initialCapacity, int capacityIncrement) : base(initialCapacity, capacityIncrement) { }
    public MyStack(T[] array) : base(array) { }
    // 1) Помещение элемента в стек
    public void Push(T item)
    {
        this.Add(item);
    }
    // 2) Извлечение элемента из стека
    public T Pop()
    {
        if (this.IsEmpty())
            throw new InvalidOperationException("Стек пуст");
        T item = this.LastElement();
        this.Remove(this.Size() - 1);
        return item;
    }
    // 3) Просмотр верхнего элемента
    public T Peek()
    {
        if (this.IsEmpty())
            throw new InvalidOperationException("Стек пуст");
        return this.LastElement();
    }
    // 4) Проверка пустоты
    public bool Empty()
    {
        return this.IsEmpty();
    }
    // 5) Поиск элемента
    public int Search(T item)
    {
        for (int i = this.Size() - 1, depth = 1; i >= 0; i--, depth++)
        {
            if (this.Get(i).Equals(item))
            {
                return depth;
            }
        }
        return -1;
    }
}
class Program
{
    static void Main()
    {
        MyStack<int> stack = new MyStack<int>();
        stack.Push(10);
        stack.Push(20);
        stack.Push(30);
        Console.WriteLine("Верхний элемент (peek): " + stack.Peek()); // 30
        Console.WriteLine("Pop: " + stack.Pop()); // 30
        Console.WriteLine("Pop: " + stack.Pop()); // 20
        Console.WriteLine("Stack empty? " + stack.Empty()); // false
        stack.Push(40);
        stack.Push(50);
        Console.WriteLine("Поиск 40: " + stack.Search(40)); // 2
        Console.WriteLine("Поиск 100: " + stack.Search(100)); // -1
    }
}
