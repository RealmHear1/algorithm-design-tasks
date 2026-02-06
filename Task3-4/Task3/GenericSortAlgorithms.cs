using System;
using System.Collections.Generic;

namespace Task3_SortComparison
{
    public static class GenericSortAlgorithms
    {
        public static void BubbleSort<T>(T[] array, Comparison<T> cmp)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (cmp(array[j], array[j + 1]) > 0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }

        public static void InsertionSort<T>(T[] array, Comparison<T> cmp)
        {
            for (int i = 1; i < array.Length; i++)
            {
                T key = array[i];
                int j = i - 1;

                while (j >= 0 && cmp(array[j], key) > 0)
                {
                    array[j + 1] = array[j];
                    j--;
                }
                array[j + 1] = key;
            }
        }

        public static void SelectionSort<T>(T[] array, Comparison<T> cmp)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (cmp(array[j], array[min]) < 0)
                        min = j;
                }

                T temp = array[i];
                array[i] = array[min];
                array[min] = temp;
            }
        }

        public static void QuickSort<T>(T[] array, Comparison<T> cmp)
        {
            QuickSort(array, 0, array.Length - 1, cmp);
        }

        private static void QuickSort<T>(T[] array, int left, int right, Comparison<T> cmp)
        {
            if (left >= right)
                return;

            T pivot = array[(left + right) / 2];
            int i = left;
            int j = right;

            while (i <= j)
            {
                while (cmp(array[i], pivot) < 0)
                    i++;
                while (cmp(array[j], pivot) > 0)
                    j--;

                if (i <= j)
                {
                    T temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                    i++;
                    j--;
                }
            }

            if (left < j)
                QuickSort(array, left, j, cmp);
            if (i < right)
                QuickSort(array, i, right, cmp);
        }

        public static void MergeSort<T>(T[] array, Comparison<T> cmp)
        {
            if (array.Length <= 1)
                return;

            int mid = array.Length / 2;
            T[] left = new T[mid];
            T[] right = new T[array.Length - mid];

            Array.Copy(array, 0, left, 0, mid);
            Array.Copy(array, mid, right, 0, array.Length - mid);

            MergeSort(left, cmp);
            MergeSort(right, cmp);
            Merge(array, left, right, cmp);
        }

        private static void Merge<T>(T[] array, T[] left, T[] right, Comparison<T> cmp)
        {
            int i = 0, j = 0, k = 0;

            while (i < left.Length && j < right.Length)
            {
                if (cmp(left[i], right[j]) <= 0)
                    array[k++] = left[i++];
                else
                    array[k++] = right[j++];
            }

            while (i < left.Length)
                array[k++] = left[i++];

            while (j < right.Length)
                array[k++] = right[j++];
        }

        public static void ShellSort<T>(T[] array, Comparison<T> cmp)
        {
            int n = array.Length;
            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < n; i++)
                {
                    T temp = array[i];
                    int j = i;
                    while (j >= gap && cmp(array[j - gap], temp) > 0)
                    {
                        array[j] = array[j - gap];
                        j -= gap;
                    }
                    array[j] = temp;
                }
            }
        }

        public static void HeapSort<T>(T[] array, Comparison<T> cmp)
        {
            int n = array.Length;

            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(array, n, i, cmp);

            for (int i = n - 1; i > 0; i--)
            {
                T temp = array[0];
                array[0] = array[i];
                array[i] = temp;
                Heapify(array, i, 0, cmp);
            }
        }

        private static void Heapify<T>(T[] array, int n, int i, Comparison<T> cmp)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && cmp(array[left], array[largest]) > 0)
                largest = left;
            if (right < n && cmp(array[right], array[largest]) > 0)
                largest = right;

            if (largest != i)
            {
                T temp = array[i];
                array[i] = array[largest];
                array[largest] = temp;

                Heapify(array, n, largest, cmp);
            }
        }
    }
}
