using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task6
{
    public class MyPriorityQueue<T>
    {
        private T[] queue;
        private int size;
        private IComparer<T> comparator;
        private const int DEFAULT_CAPACITY = 11;
        // Конструкторы
        public MyPriorityQueue()
            : this(DEFAULT_CAPACITY, Comparer<T>.Default) { }
        public MyPriorityQueue(int initialCapacity)
            : this(initialCapacity, Comparer<T>.Default) { }
        public MyPriorityQueue(int initialCapacity, IComparer<T> comp)
        {
            if (initialCapacity <= 0) throw new ArgumentException("Capacity must be > 0");
            queue = new T[initialCapacity];
            comparator = comp ?? Comparer<T>.Default;
            size = 0;
        }
        public MyPriorityQueue(T[] a)
            : this(Math.Max(DEFAULT_CAPACITY, a.Length), Comparer<T>.Default)
        {
            AddAll(a);
        }
        public MyPriorityQueue(MyPriorityQueue<T> other)
            : this(other.size, other.comparator)
        {
            AddAll(other.ToArray());
        }
        // Методы
        public void Add(T e)
        {
            EnsureCapacity(size + 1);
            queue[size] = e;
            SiftUp(size);
            size++;
        }
        public bool Offer(T e)
        {
            try
            {
                Add(e);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void AddAll(T[] a)
        {
            foreach (var e in a)
                Add(e);
        }
        public void Clear()
        {
            queue = new T[DEFAULT_CAPACITY];
            size = 0;
        }
        public bool Contains(T o)
        {
            for (int i = 0; i < size; i++)
                if (EqualityComparer<T>.Default.Equals(queue[i], o))
                    return true;
            return false;
        }
        public bool ContainsAll(T[] a)
        {
            foreach (var e in a)
                if (!Contains(e)) return false;
            return true;
        }
        public bool IsEmpty() => size == 0;
        public bool Remove(T o)
        {
            for (int i = 0; i < size; i++)
            {
                if (EqualityComparer<T>.Default.Equals(queue[i], o))
                {
                    RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public void RemoveAll(T[] a)
        {
            foreach (var e in a)
                Remove(e);
        }
        public void RetainAll(T[] a)
        {
            HashSet<T> keep = new HashSet<T>(a);
            for (int i = 0; i < size;)
            {
                if (!keep.Contains(queue[i]))
                    RemoveAt(i);
                else
                    i++;
            }
        }
        public int Size() => size;
        public T[] ToArray()
        {
            T[] result = new T[size];
            Array.Copy(queue, result, size);
            return result;
        }
        public T[] ToArray(T[] a)
        {
            if (a == null || a.Length < size)
                return ToArray();
            Array.Copy(queue, a, size);
            return a;
        }
        public T Element()
        {
            if (IsEmpty()) throw new InvalidOperationException("Queue is empty");
            return queue[0];
        }
        public T Peek()
        {
            return IsEmpty() ? default(T) : queue[0];
        }
        public T Poll()
        {
            if (IsEmpty()) return default(T);
            return RemoveAt(0);
        }
        // Вспомогательные методы для кучи
        private T RemoveAt(int index)
        {
            if (index < 0 || index >= size) throw new ArgumentOutOfRangeException();
            T removed = queue[index];
            size--;
            if (index != size)
            {
                queue[index] = queue[size];
                queue[size] = default(T);
                SiftDown(index);
            }
            return removed;
        }
        private void EnsureCapacity(int minCapacity)
        {
            if (minCapacity > queue.Length)
            {
                int oldCap = queue.Length;
                int newCap = (oldCap < 64) ? (oldCap + 2) : (oldCap + oldCap / 2);
                if (newCap < minCapacity) newCap = minCapacity;
                Array.Resize(ref queue, newCap);
            }
        }
        private void SiftUp(int i)
        {
            T e = queue[i];
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (comparator.Compare(e, queue[parent]) >= 0) break;
                queue[i] = queue[parent];
                i = parent;
            }
            queue[i] = e;
        }
        private void SiftDown(int i)
        {
            T e = queue[i];
            int half = size / 2;
            while (i < half)
            {
                int child = i * 2 + 1;
                int right = child + 1;
                if (right < size && comparator.Compare(queue[right], queue[child]) < 0)
                    child = right;
                if (comparator.Compare(queue[child], e) >= 0)
                    break;
                queue[i] = queue[child];
                i = child;
            }
            queue[i] = e;
        }
    }
    class Program
    {
        static void Main()
        {
            MyPriorityQueue<int> pq = new MyPriorityQueue<int>();
            pq.Add(5);
            pq.Add(2);
            pq.Add(8);
            pq.Add(1);
            Console.WriteLine("Размер: " + pq.Size());  // 4
            Console.WriteLine("Минимум (peek): " + pq.Peek()); // 1
            Console.WriteLine("Извлекаем элементы по приоритету:");
            while (!pq.IsEmpty())
                Console.WriteLine(pq.Poll()); // 1, 2, 5, 8
        }
    }
}
