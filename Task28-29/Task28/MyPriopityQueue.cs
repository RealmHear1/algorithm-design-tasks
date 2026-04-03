using System;
using System.Collections.Generic;

namespace Task6
{
    public class MyPriorityQueue<T> : MyQueue<T>
    {
        private T[] queue;
        private int sizeValue;
        private IComparer<T> comparator;
        private const int DEFAULT_CAPACITY = 11;

        public MyPriorityQueue()
            : this(DEFAULT_CAPACITY, Comparer<T>.Default) { }

        public MyPriorityQueue(int initialCapacity)
            : this(initialCapacity, Comparer<T>.Default) { }

        public MyPriorityQueue(int initialCapacity, IComparer<T> comp)
        {
            if (initialCapacity <= 0) throw new ArgumentException("Вместимость должна быть > 0");
            queue = new T[initialCapacity];
            comparator = comp ?? Comparer<T>.Default;
            sizeValue = 0;
        }

        public MyPriorityQueue(T[] a)
            : this(Math.Max(DEFAULT_CAPACITY, a.Length), Comparer<T>.Default)
        {
            AddAll(a);
        }

        public MyPriorityQueue(MyCollection<T> c)
            : this()
        {
            addAll(c);
        }

        public MyPriorityQueue(MyPriorityQueue<T> other)
            : this(other.sizeValue, other.comparator)
        {
            AddAll(other.ToArrayRaw());
        }

        public void Add(T e)
        {
            EnsureCapacity(sizeValue + 1);
            queue[sizeValue] = e;
            SiftUp(sizeValue);
            sizeValue++;
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
            if (a == null) throw new ArgumentNullException(nameof(a));
            for (int i = 0; i < a.Length; i++)
                Add(a[i]);
        }

        public void Clear()
        {
            queue = new T[DEFAULT_CAPACITY];
            sizeValue = 0;
        }

        public bool Contains(T o)
        {
            for (int i = 0; i < sizeValue; i++)
                if (EqualityComparer<T>.Default.Equals(queue[i], o))
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

        public bool IsEmpty() { return sizeValue == 0; }

        public bool Remove(T o)
        {
            for (int i = 0; i < sizeValue; i++)
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
            if (a == null) throw new ArgumentNullException(nameof(a));
            for (int i = 0; i < a.Length; i++)
                Remove(a[i]);
        }

        public void RetainAll(T[] a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            HashSet<T> keep = new HashSet<T>(a);
            for (int i = 0; i < sizeValue;)
            {
                if (!keep.Contains(queue[i]))
                    RemoveAt(i);
                else
                    i++;
            }
        }

        public int Size() { return sizeValue; }

        public T[] ToArrayRaw()
        {
            T[] result = new T[sizeValue];
            Array.Copy(queue, result, sizeValue);
            return result;
        }

        public object[] ToArray()
        {
            object[] result = new object[sizeValue];
            Array.Copy(queue, result, sizeValue);
            return result;
        }

        public T[] ToArray(T[] a)
        {
            if (a == null || a.Length < sizeValue)
                return ToArrayRaw();
            Array.Copy(queue, a, sizeValue);
            if (a.Length > sizeValue)
                a[sizeValue] = default(T);
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

        private T RemoveAt(int index)
        {
            if (index < 0 || index >= sizeValue) throw new ArgumentOutOfRangeException();
            T removed = queue[index];
            sizeValue--;
            if (index != sizeValue)
            {
                queue[index] = queue[sizeValue];
                queue[sizeValue] = default(T);
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
            int half = sizeValue / 2;
            while (i < half)
            {
                int child = i * 2 + 1;
                int right = child + 1;
                if (right < sizeValue && comparator.Compare(queue[right], queue[child]) < 0)
                    child = right;
                if (comparator.Compare(queue[child], e) >= 0)
                    break;
                queue[i] = queue[child];
                i = child;
            }
            queue[i] = e;
        }

        public MyIterator<T> iterator()
        {
            return new MyItr(this);
        }

        private class MyItr : MyIterator<T>
        {
            private readonly MyPriorityQueue<T> queueRef;
            private int cursor;
            private int lastRet = -1;

            public MyItr(MyPriorityQueue<T> q)
            {
                queueRef = q;
            }

            public bool hasNext()
            {
                return cursor < queueRef.sizeValue;
            }

            public T next()
            {
                if (!hasNext())
                    throw new NoSuchElementException();

                lastRet = cursor;
                return queueRef.queue[cursor++];
            }

            public void remove()
            {
                if (lastRet < 0)
                    throw new IllegalStateException();

                queueRef.RemoveAt(lastRet);
                cursor = lastRet;
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
        public bool contains(object o) { return o is T && Contains((T)o); }

        public bool containsAll(MyCollection<T> c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            object[] arr = c.toArray();
            for (int i = 0; i < arr.Length; i++)
                if (!contains(arr[i])) return false;
            return true;
        }

        public bool isEmpty() { return IsEmpty(); }
        public bool remove(object o) { return o is T && Remove((T)o); }

        public bool removeAll(MyCollection<T> c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            bool changed = false;
            object[] arr = c.toArray();
            for (int i = 0; i < arr.Length; i++)
                if (remove(arr[i])) changed = true;
            return changed;
        }

        public bool retainAll(MyCollection<T> c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            HashSet<T> keep = new HashSet<T>();
            object[] arr = c.toArray();
            for (int i = 0; i < arr.Length; i++) keep.Add((T)arr[i]);

            int oldSize = sizeValue;
            for (int i = 0; i < sizeValue;)
            {
                if (!keep.Contains(queue[i]))
                    RemoveAt(i);
                else
                    i++;
            }
            return oldSize != sizeValue;
        }

        public int size() { return Size(); }
        public object[] toArray() { return ToArray(); }
        public T[] toArray(T[] a) { return ToArray(a); }
        public T element() { return Element(); }
        public bool offer(T obj) { return Offer(obj); }
        public T peek() { return Peek(); }
        public T poll() { return Poll(); }
    }
}
