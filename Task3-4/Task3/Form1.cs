using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task3_SortComparison;
using ZedGraph;

namespace Task3
{
    public partial class Form1 : Form
    {
        private readonly Dictionary<string, Action<int[]>> algorithms;
        private Dictionary<string, Action<int[], Comparison<int>>> genericAlgorithms;
        public Form1()
        {
            InitializeComponent();
            algorithms = new Dictionary<string, Action<int[]>>()
            {
                { "Bubble", SortAlgorithms.BubbleSort },
                { "Shaker", SortAlgorithms.ShakerSort },
                { "Comb", SortAlgorithms.CombSort },
                { "Insertion", SortAlgorithms.InsertionSort },
                { "Shell", SortAlgorithms.ShellSort },
                { "Tree", SortAlgorithms.TreeSort },
                { "Gnome", SortAlgorithms.GnomeSort },
                { "Selection", SortAlgorithms.SelectionSort },
                { "Heap", SortAlgorithms.HeapSort },
                { "Quick", SortAlgorithms.QuickSort },
                { "Merge", SortAlgorithms.MergeSort },
                { "Radix", SortAlgorithms.RadixSort },
                { "Bitonic", SortAlgorithms.BitonicSort }
            };
            genericAlgorithms = new Dictionary<string, Action<int[], Comparison<int>>>
            {
                {"Bubble (Gen)", GenericSortAlgorithms.BubbleSort},
                {"Insertion (Gen)", GenericSortAlgorithms.InsertionSort},
                {"Selection (Gen)", GenericSortAlgorithms.SelectionSort},
                {"Quick (Gen)", GenericSortAlgorithms.QuickSort},
                {"Merge (Gen)", GenericSortAlgorithms.MergeSort}
            };
            comboDataType.Items.AddRange(new[]
        {
                "Случайные числа",
                "Отсортированные подмассивы",
                "Почти отсортированные",
                "Отсортированные",
                "Обратные",
                "Повторения (50%)"
            });
            comboDataType.SelectedIndex = 0;
            comboType.Items.AddRange(new[] { "int", "double", "string" });
            comboType.SelectedIndex = 0;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private async void btnRun_Click(object sender, EventArgs e)
        {
            int[] sizes = { 100, 1000, 10000 };
            var results = new List<(string Alg, int Size, double Time)>();

            progressBar.Value = 0;
            int total = (algorithms.Count + genericAlgorithms.Count) * sizes.Length;
            int done = 0;

            int dataTypeIndex = comboDataType.SelectedIndex;
            string type = comboType.SelectedItem.ToString();

            await Task.Run(() =>
            {
                foreach (var alg in algorithms)
                {
                    foreach (int size in sizes)
                    {
                        int[] arr = GenerateArray(size, dataTypeIndex);
                        double time = Benchmark.Measure(alg.Value, arr, 5);
                        results.Add((alg.Key, size, time));

                        Invoke((Action)(() =>
                        {
                            progressBar.Value = ++done * 100 / total;
                        }));
                    }
                }
            });

            Comparison<int> cmpInt = Comparer<int>.Default.Compare;
            Comparison<double> cmpDouble = Comparer<double>.Default.Compare;
            Comparison<string> cmpString = Comparer<string>.Default.Compare;

            await Task.Run(() =>
            {
                foreach (var alg in genericAlgorithms)
                {
                    foreach (int size in sizes)
                    {
                        switch (type)
                        {
                            case "int":
                                {
                                    int[] arr = (int[])GenerateGenericArray(size, "int");
                                    double time = BenchmarkGeneric.Measure(alg.Value, arr, cmpInt, 5);
                                    results.Add((alg.Key + " (int)", size, time));
                                    break;
                                }
                            case "double":
                                {
                                    double[] arr = (double[])GenerateGenericArray(size, "double");
                                    double time = BenchmarkGeneric.Measure(GenericSortAlgorithms.BubbleSort, arr, cmpDouble, 5);
                                    results.Add((alg.Key + " (double)", size, time));
                                    break;
                                }

                            case "string":
                                {
                                    string[] arr = (string[])GenerateGenericArray(size, "string");
                                    double time = BenchmarkGeneric.Measure(GenericSortAlgorithms.BubbleSort, arr, cmpString, 5);
                                    results.Add((alg.Key + " (string)", size, time));
                                    break;
                                }
                        }

                        Invoke((Action)(() => progressBar.Value = ++done * 100 / total));
                    }
                }
            });

            DrawGraph(results);
            txtOutput.Text = string.Join(Environment.NewLine,
                results.Select(r => $"{r.Alg,-15} | {r.Size,6} | {r.Time,8:F2} мс"));
        }


        private int[] GenerateArray(int size, int dataTypeIndex)
        {
            switch (dataTypeIndex)
            {
                case 0:
                    return ArrayGenerator.RandomArray(size);
                case 1:
                    return ArrayGenerator.SubarraysArray(size);
                case 2:
                    return ArrayGenerator.SemiSortedArray(size);
                case 3:
                    return ArrayGenerator.SortedArray(size);
                case 4:
                    return ArrayGenerator.SortedArray(size, true);
                case 5:
                    return ArrayGenerator.RepeatedArray(size, 0.5);
                default:
                    return ArrayGenerator.RandomArray(size);
            }
        }
        private static readonly Random rand = new Random();

        private Array GenerateGenericArray(int size, string type)
        {
            if (type == "int")
            {
                int[] arr = new int[size];
                for (int i = 0; i < size; i++) arr[i] = rand.Next(0, 1000);
                return arr;
            }
            else if (type == "double")
            {
                double[] arr = new double[size];
                for (int i = 0; i < size; i++) arr[i] = rand.NextDouble() * 1000.0;
                return arr;
            }
            else if (type == "string")
            {
                string[] arr = new string[size];
                for (int i = 0; i < size; i++)
                {
                    int len = 1 + rand.Next(1, 8);
                    char[] buf = new char[len];
                    for (int j = 0; j < len; j++) buf[j] = (char)rand.Next('A', 'Z' + 1);
                    arr[i] = new string(buf);
                }
                return arr;
            }
            else
            {
                int[] arr = new int[size];
                for (int i = 0; i < size; i++) arr[i] = rand.Next(0, 1000);
                return arr;
            }
        }



        private void DrawGraph(List<(string Alg, int Size, double Time)> data)
        {
            GraphPane pane = zedGraph.GraphPane;
            pane.CurveList.Clear();
            pane.Title.Text = "Сравнение алгоритмов сортировки";
            pane.XAxis.Title.Text = "Размер массива";
            pane.YAxis.Title.Text = "Время (мс)";
            pane.Legend.IsVisible = true;

            Color[] colors = { Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Purple,
                               Color.Brown, Color.Cyan, Color.Magenta, Color.Gray, Color.Gold, Color.Black };
            int i = 0;

            foreach (var alg in data.Select(d => d.Alg).Distinct())
            {
                var list = new PointPairList();
                foreach (var r in data.Where(x => x.Alg == alg))
                    list.Add(r.Size, r.Time);
                pane.AddCurve(alg, list, colors[i++ % colors.Length], SymbolType.Circle);
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text files|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
                System.IO.File.WriteAllText(sfd.FileName, txtOutput.Text);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    public static class BenchmarkGeneric
    {
        public static double Measure<T>(Action<T[], Comparison<T>> sort, T[] array, Comparison<T> cmp, int runs = 10)
        {
            double total = 0;
            for (int i = 0; i < runs; i++)
            {
                T[] copy = (T[])array.Clone();
                var sw = System.Diagnostics.Stopwatch.StartNew();
                sort(copy, cmp);
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / runs;
        }
    }
}
