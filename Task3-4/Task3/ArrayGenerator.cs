using System;

namespace Task3_SortComparison
{
    public static class ArrayGenerator
    {
        private static readonly Random rand = new Random();
        public static int[] RandomArray(int size, int mod = 1000)
        {
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
                arr[i] = rand.Next(mod);
            return arr;
        }

        public static int[] SemiSortedArray(int size)
        {
            int[] arr = RandomArray(size);
            Array.Sort(arr);
            int swaps = Math.Max(1, size / 100);
            for (int i = 0; i < swaps; i++)
            {
                int a = rand.Next(size);
                int b = rand.Next(size);
                (arr[a], arr[b]) = (arr[b], arr[a]);
            }
            return arr;
        }

        public static int[] SortedArray(int size, bool reverse = false)
        {
            int[] arr = RandomArray(size);
            Array.Sort(arr);
            if (reverse) Array.Reverse(arr);
            return arr;
        }

        public static int[] RepeatedArray(int size, double repetitionPercent)
        {
            int uniqueCount = Math.Max(1, (int)(size * (1 - repetitionPercent)));
            int[] baseVals = RandomArray(uniqueCount);
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
                arr[i] = baseVals[rand.Next(uniqueCount)];
            return arr;
        }

        public static int[] SubarraysArray(int size, int maxSubSize = 100)
        {
            int[] arr = new int[size];
            int pos = 0;
            while (pos < size)
            {
                int subSize = rand.Next(1, Math.Min(maxSubSize, size - pos + 1));
                int[] sub = RandomArray(subSize);
                Array.Sort(sub);
                Array.Copy(sub, 0, arr, pos, subSize);
                pos += subSize;
            }
            return arr;
        }
    }
}


