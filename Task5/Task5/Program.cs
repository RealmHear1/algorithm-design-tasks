using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task5
{
    public class MinHeap
    {
        private List<int> heap;
        public MinHeap(int[] array)
        {
            heap = new List<int>(array);
            BuildHeap();
        }
        // Построение кучи за O(n)
        private void BuildHeap()
        {
            for (int i = heap.Count / 2 - 1; i >= 0; i--)
            {
                SiftDown(i);
            }
        }
        public int GetMin()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("Куча пуста");
            return heap[0];
        }
        public int RemoveMin()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("Куча пуста");
            int min = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            if (heap.Count > 0)
                SiftDown(0);
            return min;
        }
        public void DecreaseKey(int index, int newValue)
        {
            if (index < 0 || index >= heap.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (newValue > heap[index])
                throw new ArgumentException("Новое значение больше текущего");

            heap[index] = newValue;
            SiftUp(index);
        }
        public void Insert(int value)
        {
            heap.Add(value);
            SiftUp(heap.Count - 1);
        }
        public MinHeap Merge(MinHeap other)
        {
            List<int> combined = new List<int>(this.heap);
            combined.AddRange(other.heap);
            return new MinHeap(combined.ToArray());
        }
        private void SiftUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (heap[index] < heap[parent])
                {
                    Swap(index, parent);
                    index = parent;
                }
                else
                {
                    break;
                }
            }
        }
        private void SiftDown(int index)
        {
            int n = heap.Count;
            while (index * 2 + 1 < n)
            {
                int left = index * 2 + 1;
                int right = index * 2 + 2;
                int smallest = left;

                if (right < n && heap[right] < heap[left])
                    smallest = right;

                if (heap[index] <= heap[smallest])
                    break;

                Swap(index, smallest);
                index = smallest;
            }
        }
        private void Swap(int i, int j)
        {
            int temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }
        public void PrintHeap()
        {
            Console.WriteLine("Куча: " + string.Join(", ", heap));
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var heap1 = new MinHeap(new int[] { 2, 5, 7, 8, 6, 10, 42, 11, 15, 28, 9, 13 });
            Console.WriteLine("Минимум: " + heap1.GetMin());
            heap1.Insert(1);
            Console.WriteLine("После вставки 1: " + heap1.GetMin());
            heap1.DecreaseKey(3, 0);
            Console.WriteLine("После уменьшения ключа: " + heap1.GetMin());
            Console.WriteLine("Удаление минимума: " + heap1.RemoveMin());
            Console.WriteLine("Новый минимум: " + heap1.GetMin());
            heap1.PrintHeap();
            var heap2 = new MinHeap(new int[] { 4, 50, 200 });
            var merged = heap1.Merge(heap2);
            Console.WriteLine("Минимум объединённой кучи: " + merged.GetMin());
            merged.PrintHeap();
        }
    }
}
