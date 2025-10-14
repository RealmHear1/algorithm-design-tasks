using System;
using System.Collections.Generic;

namespace Task3_SortComparison
{
    public static class SortAlgorithms
    {
        public static void BubbleSort(int[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
                for (int j = 0; j < a.Length - i - 1; j++)
                    if (a[j] > a[j + 1])
                        (a[j], a[j + 1]) = (a[j + 1], a[j]);
        }

        public static void ShakerSort(int[] a)
        {
            int left = 0, right = a.Length - 1;
            while (left < right)
            {
                for (int i = left; i < right; i++)
                    if (a[i] > a[i + 1]) (a[i], a[i + 1]) = (a[i + 1], a[i]);
                right--;
                for (int i = right; i > left; i--)
                    if (a[i - 1] > a[i]) (a[i - 1], a[i]) = (a[i], a[i - 1]);
                left++;
            }
        }

        public static void CombSort(int[] a)
        {
            int gap = a.Length;
            bool swapped = true;
            while (gap > 1 || swapped)
            {
                gap = Math.Max(1, (gap * 10) / 13);
                swapped = false;
                for (int i = 0; i + gap < a.Length; i++)
                {
                    if (a[i] > a[i + gap])
                    {
                        (a[i], a[i + gap]) = (a[i + gap], a[i]);
                        swapped = true;
                    }
                }
            }
        }

        public static void InsertionSort(int[] a)
        {
            for (int i = 1; i < a.Length; i++)
            {
                int key = a[i];
                int j = i - 1;
                while (j >= 0 && a[j] > key)
                {
                    a[j + 1] = a[j];
                    j--;
                }
                a[j + 1] = key;
            }
        }

        public static void ShellSort(int[] a)
        {
            for (int gap = a.Length / 2; gap > 0; gap /= 2)
                for (int i = gap; i < a.Length; i++)
                {
                    int temp = a[i];
                    int j = i;
                    while (j >= gap && a[j - gap] > temp)
                    {
                        a[j] = a[j - gap];
                        j -= gap;
                    }
                    a[j] = temp;
                }
        }

        public static void TreeSort(int[] a)
        {
            var tree = new SortedSet<int>(a);
            int i = 0;
            foreach (var v in tree)
                a[i++] = v;
        }

        public static void GnomeSort(int[] a)
        {
            int i = 1;
            while (i < a.Length)
            {
                if (i == 0 || a[i - 1] <= a[i]) i++;
                else { (a[i], a[i - 1]) = (a[i - 1], a[i]); i--; }
            }
        }

        public static void SelectionSort(int[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < a.Length; j++)
                    if (a[j] < a[min]) min = j;
                (a[i], a[min]) = (a[min], a[i]);
            }
        }

        public static void HeapSort(int[] a)
        {
            int n = a.Length;
            for (int i = n / 2 - 1; i >= 0; i--) Heapify(a, n, i);
            for (int i = n - 1; i >= 0; i--)
            {
                (a[0], a[i]) = (a[i], a[0]);
                Heapify(a, i, 0);
            }
        }
        private static void Heapify(int[] a, int n, int i)
        {
            int largest = i;
            int l = 2 * i + 1, r = 2 * i + 2;
            if (l < n && a[l] > a[largest]) largest = l;
            if (r < n && a[r] > a[largest]) largest = r;
            if (largest != i)
            {
                (a[i], a[largest]) = (a[largest], a[i]);
                Heapify(a, n, largest);
            }
        }

        public static void QuickSort(int[] a)
        {
            void Sort(int l, int r)
            {
                int i = l, j = r, pivot = a[(l + r) / 2];
                while (i <= j)
                {
                    while (a[i] < pivot) i++;
                    while (a[j] > pivot) j--;
                    if (i <= j)
                    {
                        (a[i], a[j]) = (a[j], a[i]);
                        i++; j--;
                    }
                }
                if (l < j) Sort(l, j);
                if (i < r) Sort(i, r);
            }
            Sort(0, a.Length - 1);
        }

        public static void MergeSort(int[] a)
        {
            if (a.Length <= 1) return;
            int mid = a.Length / 2;
            int[] left = new int[mid];
            int[] right = new int[a.Length - mid];
            Array.Copy(a, left, mid);
            Array.Copy(a, mid, right, 0, a.Length - mid);
            MergeSort(left);
            MergeSort(right);
            Merge(a, left, right);
        }

        private static void Merge(int[] a, int[] left, int[] right)
        {
            int i = 0, j = 0, k = 0;
            while (i < left.Length && j < right.Length)
                a[k++] = left[i] < right[j] ? left[i++] : right[j++];
            while (i < left.Length) a[k++] = left[i++];
            while (j < right.Length) a[k++] = right[j++];
        }

        public static void RadixSort(int[] a)
        {
            int max = int.MinValue;
            foreach (var x in a) if (x > max) max = x;
            for (int exp = 1; max / exp > 0; exp *= 10)
                CountingSort(a, exp);
        }
        private static void CountingSort(int[] a, int exp)
        {
            int n = a.Length;
            int[] output = new int[n];
            int[] count = new int[10];
            for (int i = 0; i < n; i++) count[(a[i] / exp) % 10]++;
            for (int i = 1; i < 10; i++) count[i] += count[i - 1];
            for (int i = n - 1; i >= 0; i--)
            {
                int idx = (a[i] / exp) % 10;
                output[count[idx] - 1] = a[i];
                count[idx]--;
            }
            Array.Copy(output, a, n);
        }

        public static void BitonicSort(int[] a)
        {
            BitonicSortRecursive(a, 0, a.Length, true);
        }
        private static void BitonicSortRecursive(int[] a, int low, int cnt, bool dir)
        {
            if (cnt > 1)
            {
                int k = cnt / 2;
                BitonicSortRecursive(a, low, k, true);
                BitonicSortRecursive(a, low + k, k, false);
                BitonicMerge(a, low, cnt, dir);
            }
        }
        private static void BitonicMerge(int[] a, int low, int cnt, bool dir)
        {
            if (cnt > 1)
            {
                int k = cnt / 2;
                for (int i = low; i < low + k; i++)
                    if (dir == (a[i] > a[i + k]))
                        (a[i], a[i + k]) = (a[i + k], a[i]);
                BitonicMerge(a, low, k, dir);
                BitonicMerge(a, low + k, k, dir);
            }
        }
    }
}

