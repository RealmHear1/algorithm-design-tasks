using System;
using System.Diagnostics;

namespace Task3_SortComparison
{
    public static class Benchmark
    {
        public static double Measure(Action<int[]> sort, int[] array, int runs = 20)
        {
            double total = 0;
            for (int i = 0; i < runs; i++)
            {
                int[] copy = (int[])array.Clone();
                var sw = Stopwatch.StartNew();
                sort(copy);
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / runs;
        }
    }
}

