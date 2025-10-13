using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace Task17
{
    public static class Benchmark
    {
        // Добавление
        public static double MeasureAddArray(int n, int runs)
        {
            double total = 0;
            for (int r = 0; r < runs; r++)
            {
                var list = new List<int>();
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < n; i++) list.Add(i);
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / runs;
        }
        public static double MeasureAddList(int n, int runs)
        {
            double total = 0;
            for (int r = 0; r < runs; r++)
            {
                var list = new LinkedList<int>();
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < n; i++) list.AddLast(i);
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / runs;
        }
        // Получение
        public static double MeasureGetArray(int n, int runs)
        {
            double total = 0;
            var data = Enumerable.Range(0, n).ToList();
            var rnd = new Random();
            for (int r = 0; r < runs; r++)
            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < n; i++) _ = data[rnd.Next(n)];
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / runs;
        }
        public static double MeasureGetList(int n, int runs)
        {
            double total = 0;
            var data = new LinkedList<int>(Enumerable.Range(0, n));
            var rnd = new Random();
            for (int r = 0; r < runs; r++)
            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < n; i++)
                {
                    int index = rnd.Next(data.Count());
                    var node = data.First;
                    for (int j = 0; j < index; j++) node = node.Next;
                    _ = node.Value;
                }
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / runs;
        }
        // Присваивание (set)
        public static double MeasureSetArray(int n, int runs)
        {
            double total = 0;
            var data = Enumerable.Range(0, n).ToList();
            var rnd = new Random();
            for (int r = 0; r < runs; r++)
            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < n; i++) data[rnd.Next(n)] = rnd.Next();
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / runs;
        }
        public static double MeasureSetList(int n, int runs)
        {
            double total = 0;
            var data = new LinkedList<int>(Enumerable.Range(0, n));
            var rnd = new Random();
            for (int r = 0; r < runs; r++)
            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < n; i++)
                {
                    int index = rnd.Next(data.Count());
                    var node = data.First;
                    for (int j = 0; j < index; j++) node = node.Next;
                    node.Value = rnd.Next();
                }
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / runs;
        }
        // Вставка (add(i, value))
        public static double MeasureInsertArray(int n, int runs)
        {
            double total = 0;
            var rnd = new Random();
            for (int r = 0; r < runs; r++)
            {
                var data = Enumerable.Range(0, n).ToList();
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < 1000; i++) data.Insert(rnd.Next(data.Count), rnd.Next());
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / runs;
        }
        public static double MeasureInsertList(int n, int runs)
        {
            double total = 0;
            var rnd = new Random();
            for (int r = 0; r < runs; r++)
            {
                var data = new LinkedList<int>(Enumerable.Range(0, n));
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < 1000; i++)
                {
                    int index = rnd.Next(data.Count());
                    var node = data.First;
                    for (int j = 0; j < index; j++) node = node.Next;
                    data.AddBefore(node, rnd.Next());
                }
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / runs;
        }
        // Удаление
        public static double MeasureRemoveArray(int n, int runs)
        {
            double total = 0;
            var rnd = new Random();
            for (int r = 0; r < runs; r++)
            {
                var data = Enumerable.Range(0, n).ToList();
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < 1000; i++) data.RemoveAt(rnd.Next(data.Count));
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / runs;
        }
        public static double MeasureRemoveList(int n, int runs)
        {
            double total = 0;
            var rnd = new Random();
            for (int r = 0; r < runs; r++)
            {
                var data = new LinkedList<int>(Enumerable.Range(0, n));
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < 1000; i++)
                {
                    int index = rnd.Next(data.Count());
                    var node = data.First;
                    for (int j = 0; j < index; j++) node = node.Next;
                    data.Remove(node);
                }
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / runs;
        }
    }
}
