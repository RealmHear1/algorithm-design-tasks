using System;
using System.Collections.Generic;
using System.Linq;

namespace Task3_SortComparison
{
    public static class SortAlgorithms
    {
        public static void BubbleSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
                for (int j = 0; j < n - i - 1; j++)
                    if (arr[j] > arr[j + 1])
                    {
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
        }

        public static void ShakerSort(int[] arr)
        {
            int left = 0, right = arr.Length - 1;
            while (left < right)
            {
                for (int i = left; i < right; i++)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        int temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                    }
                }
                right--;
                for (int i = right; i > left; i--)
                {
                    if (arr[i - 1] > arr[i])
                    {
                        int temp = arr[i];
                        arr[i] = arr[i - 1];
                        arr[i - 1] = temp;
                    }
                }
                left++;
            }
        }

        public static void CombSort(int[] arr)
        {
            int gap = arr.Length;
            bool swapped = true;
            while (gap > 1 || swapped)
            {
                gap = Math.Max(1, (int)(gap / 1.3));
                swapped = false;
                for (int i = 0; i + gap < arr.Length; i++)
                {
                    if (arr[i] > arr[i + gap])
                    {
                        int temp = arr[i];
                        arr[i] = arr[i + gap];
                        arr[i + gap] = temp;
                        swapped = true;
                    }
                }
            }
        }

        public static void InsertionSort(int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                int key = arr[i];
                int j = i - 1;
                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = key;
            }
        }

        public static void ShellSort(int[] arr)
        {
            int n = arr.Length;
            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < n; i++)
                {
                    int temp = arr[i];
                    int j = i;
                    while (j >= gap && arr[j - gap] > temp)
                    {
                        arr[j] = arr[j - gap];
                        j -= gap;
                    }
                    arr[j] = temp;
                }
            }
        }

        public static void TreeSort(int[] arr)
        {
            SortedSet<int> bst = new SortedSet<int>(arr);
            int i = 0;
            foreach (int x in bst)
            {
                arr[i++] = x;
            }
        }

        public static void GnomeSort(int[] arr)
        {
            int i = 1;
            while (i < arr.Length)
            {
                if (i == 0 || arr[i - 1] <= arr[i])
                    i++;
                else
                {
                    int temp = arr[i];
                    arr[i] = arr[i - 1];
                    arr[i - 1] = temp;
                    i--;
                }
            }
        }

        public static void SelectionSort(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < arr.Length; j++)
                    if (arr[j] < arr[min])
                        min = j;

                int temp = arr[i];
                arr[i] = arr[min];
                arr[min] = temp;
            }
        }

        public static void HeapSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(arr, n, i);

            for (int i = n - 1; i > 0; i--)
            {
                int temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;
                Heapify(arr, i, 0);
            }
        }

        private static void Heapify(int[] arr, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && arr[left] > arr[largest]) largest = left;
            if (right < n && arr[right] > arr[largest]) largest = right;

            if (largest != i)
            {
                int temp = arr[i];
                arr[i] = arr[largest];
                arr[largest] = temp;
                Heapify(arr, n, largest);
            }
        }

        public static void QuickSort(int[] arr)
        {
            QuickSort(arr, 0, arr.Length - 1);
        }

        private static void QuickSort(int[] arr, int left, int right)
        {
            if (left >= right) return;
            int pivot = arr[(left + right) / 2];
            int i = left, j = right;

            while (i <= j)
            {
                while (arr[i] < pivot) i++;
                while (arr[j] > pivot) j--;
                if (i <= j)
                {
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    i++;
                    j--;
                }
            }

            if (left < j) QuickSort(arr, left, j);
            if (i < right) QuickSort(arr, i, right);
        }

        public static void MergeSort(int[] a)
        {
            if (a.Length <= 1) return;
            int mid = a.Length / 2;

            int[] left = new int[mid];
            int[] right = new int[a.Length - mid];

            Array.Copy(a, 0, left, 0, mid);
            Array.Copy(a, mid, right, 0, a.Length - mid);

            MergeSort(left);
            MergeSort(right);
            Merge(a, left, right);
        }

        private static void Merge(int[] arr, int[] left, int[] right)
        {
            int i = 0, j = 0, k = 0;
            while (i < left.Length && j < right.Length)
            {
                if (left[i] <= right[j])
                    arr[k++] = left[i++];
                else
                    arr[k++] = right[j++];
            }
            while (i < left.Length) arr[k++] = left[i++];
            while (j < right.Length) arr[k++] = right[j++];
        }

        public static void RadixSort(int[] arr)
        {
            int max = arr.Max();
            for (int exp = 1; max / exp > 0; exp *= 10)
                CountSort(arr, exp);
        }

        private static void CountSort(int[] arr, int exp)
        {
            int n = arr.Length;
            int[] output = new int[n];
            int[] count = new int[10];

            for (int i = 0; i < n; i++) count[(arr[i] / exp) % 10]++;
            for (int i = 1; i < 10; i++) count[i] += count[i - 1];
            for (int i = n - 1; i >= 0; i--)
            {
                output[count[(arr[i] / exp) % 10] - 1] = arr[i];
                count[(arr[i] / exp) % 10]--;
            }
            for (int i = 0; i < n; i++) arr[i] = output[i];
        }

        public static void BitonicSort(int[] arr)
        {
            BitonicSort(arr, 0, arr.Length, true);
        }

        private static void BitonicSort(int[] arr, int low, int cnt, bool dir)
        {
            if (cnt <= 1) return;
            int k = cnt / 2;
            BitonicSort(arr, low, k, true);
            BitonicSort(arr, low + k, k, false);
            BitonicMerge(arr, low, cnt, dir);
        }

        private static void BitonicMerge(int[] arr, int low, int cnt, bool dir)
        {
            if (cnt <= 1) return;
            int k = GreatestPowerOfTwoLessThan(cnt);
            for (int i = low; i < low + cnt - k; i++)
            {
                if (dir == (arr[i] > arr[i + k]))
                {
                    int temp = arr[i];
                    arr[i] = arr[i + k];
                    arr[i + k] = temp;
                }
            }
            BitonicMerge(arr, low, k, dir);
            BitonicMerge(arr, low + k, cnt - k, dir);
        }

        private static int GreatestPowerOfTwoLessThan(int n)
        {
            int k = 1;
            while (k > 0 && k < n)
                k <<= 1;
            return k >> 1;
        }
    }
}
