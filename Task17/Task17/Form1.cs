using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
namespace Task17
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private List<(string Structure, string Operation, int Size, double TimeMs)> RunTests(Action<int> progressUpdate)
        {
            int[] sizes = { 1_000, 5_000, 10_000 };
            int runs = 3;
            var results = new List<(string, string, int, double)>();
            int totalSteps = sizes.Length * 5;
            int done = 0;
            foreach (int n in sizes)
            {
                results.Add(("MyArrayList", "add", n, Benchmark.MeasureAddArray(n, runs)));
                results.Add(("MyLinkedList", "add", n, Benchmark.MeasureAddList(n, runs)));
                progressUpdate(++done * 100 / totalSteps);
                results.Add(("MyArrayList", "get", n, Benchmark.MeasureGetArray(n, runs)));
                results.Add(("MyLinkedList", "get", n, Benchmark.MeasureGetList(n, runs)));
                progressUpdate(++done * 100 / totalSteps);
                results.Add(("MyArrayList", "set", n, Benchmark.MeasureSetArray(n, runs)));
                results.Add(("MyLinkedList", "set", n, Benchmark.MeasureSetList(n, runs)));
                progressUpdate(++done * 100 / totalSteps);
                results.Add(("MyArrayList", "insert", n, Benchmark.MeasureInsertArray(n, runs)));
                results.Add(("MyLinkedList", "insert", n, Benchmark.MeasureInsertList(n, runs)));
                progressUpdate(++done * 100 / totalSteps);
                results.Add(("MyArrayList", "remove", n, Benchmark.MeasureRemoveArray(n, runs)));
                results.Add(("MyLinkedList", "remove", n, Benchmark.MeasureRemoveList(n, runs)));
                progressUpdate(++done * 100 / totalSteps);
            }
            return results;
        }
        private void DrawGraph(List<(string Structure, string Operation, int Size, double TimeMs)> results)
        {
            GraphPane pane = zedGraph.GraphPane;
            pane.CurveList.Clear();
            pane.Title.Text = "Сравнение MyArrayList и MyLinkedList";
            pane.XAxis.Title.Text = "Размер структуры";
            pane.YAxis.Title.Text = "Время (мс)";
            pane.Legend.IsVisible = true;
            var operations = new[] { "add", "get", "set", "insert", "remove" };
            Color[] colors = { Color.Blue, Color.Green, Color.Orange, Color.Red, Color.Purple };
            for (int i = 0; i < operations.Length; i++)
            {
                string op = operations[i];
                var arrPoints = new PointPairList();
                var listPoints = new PointPairList();
                foreach (var r in results)
                {
                    if (r.Operation == op && r.Structure == "MyArrayList") arrPoints.Add(r.Size, r.TimeMs);
                    if (r.Operation == op && r.Structure == "MyLinkedList") listPoints.Add(r.Size, r.TimeMs);
                }
                pane.AddCurve($"ArrayList {op}", arrPoints, colors[i], SymbolType.Circle);
                pane.AddCurve($"LinkedList {op}", listPoints, colors[i], SymbolType.Diamond);
            }
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }
        private void ShowSummary(List<(string Structure, string Operation, int Size, double TimeMs)> results)
        {
            var summary = new StringBuilder();
            summary.AppendLine("Итоги анализа:\n");
            var ops = results.Select(r => r.Operation).Distinct();
            foreach (var op in ops)
            {
                var arrAvg = results.Where(r => r.Operation == op && r.Structure == "MyArrayList").Average(r => r.TimeMs);
                var listAvg = results.Where(r => r.Operation == op && r.Structure == "MyLinkedList").Average(r => r.TimeMs);
                string faster = arrAvg < listAvg ? "ArrayList быстрее" : "LinkedList быстрее";
                summary.AppendLine($"{op.ToUpper()}: {faster} (ArrayList={arrAvg:F2} мс, LinkedList={listAvg:F2} мс)");
            }
            summary.AppendLine("\nВывод:");
            summary.AppendLine("MyArrayList быстрее для операций get и set (прямой доступ).");
            summary.AppendLine("MyLinkedList выигрывает при insert и remove (вставка/удаление).");
            summary.AppendLine("Разница растёт с увеличением размера структуры.");
            txtOutput.Text = summary.ToString();
            }
        private async void btnRun_Click_1(object sender, EventArgs e)
        {
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Step = 1;
            btnRun.Enabled = false;
            btnRun.Text = "Выполняется...";
            txtOutput.Clear();
            progressBar.Value = 0;
            var results = await Task.Run(() => RunTests(progress =>
            {
                Invoke((Action)(() => progressBar.Value = progress));
            }));
            DrawGraph(results);
            ShowSummary(results);
            btnRun.Text = "Запустить тест";
            btnRun.Enabled = true;
        }
        private void zedGraph_Load(object sender, EventArgs e)
        {

        }

        private void progressBar_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
