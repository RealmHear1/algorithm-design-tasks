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
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            int[] sizes = { 100, 1000, 10000 };
            var results = new List<(string Alg, int Size, double Time)>();

            progressBar.Value = 0;
            int total = algorithms.Count * sizes.Length;
            int done = 0;

            int dataTypeIndex = comboDataType.SelectedIndex;

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

            DrawGraph(results);
            txtOutput.Text = string.Join(Environment.NewLine,
                results.Select(r => $"{r.Alg,-10} | {r.Size,6} | {r.Time,8:F2} мс"));
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
    }
}
