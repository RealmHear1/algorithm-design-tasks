using System;
using System.Diagnostics;
using System.Windows.Forms;
using ZedGraph;
using System.Collections.Generic;

namespace Task24
{
    public partial class Form1 : Form
    {
        int[] sizes = { 1000, 10000, 100000 };
        int runs = 5;
        public Form1()
        {
            InitializeComponent();
        }
        

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            lblStatus.Text = "Running benchmark...";
            Application.DoEvents();

            var putHash = BenchmarkPutHash();
            var putTree = BenchmarkPutTree();

            var getHash = BenchmarkGetHash();
            var getTree = BenchmarkGetTree();

            var removeHash = BenchmarkRemoveHash();
            var removeTree = BenchmarkRemoveTree();

            DrawGraph(zedPut, "PUT", putHash, putTree);
            DrawGraph(zedGet, "GET", getHash, getTree);
            DrawGraph(zedRemove, "REMOVE", removeHash, removeTree);

            lblStatus.Text = "Done!";
        }

        // PUT

        double[] BenchmarkPutHash()
        {
            return BenchmarkPut((n) => new MyHashMap<int, int>(), true);
        }

        double[] BenchmarkPutTree()
        {
            return BenchmarkPut((n) => new MyTreeMap<int, int>(), false);
        }

        double[] BenchmarkPut(Func<int, object> creator, bool isHash)
        {
            double[] result = new double[sizes.Length];
            Random rnd = new Random();

            for (int i = 0; i < sizes.Length; i++)
            {
                double total = 0;

                for (int r = 0; r < runs; r++)
                {
                    var sw = Stopwatch.StartNew();

                    if (isHash)
                    {
                        var map = new MyHashMap<int, int>();
                        for (int j = 0; j < sizes[i]; j++)
                            map.Put(rnd.Next(), j);
                    }
                    else
                    {
                        var map = new MyTreeMap<int, int>();
                        for (int j = 0; j < sizes[i]; j++)
                            map.Put(rnd.Next(), j);
                    }

                    sw.Stop();
                    total += sw.Elapsed.TotalMilliseconds;
                }

                result[i] = total / runs;
            }

            return result;
        }

        // GET

        double[] BenchmarkGetHash()
        {
            return BenchmarkGet(true);
        }

        double[] BenchmarkGetTree()
        {
            return BenchmarkGet(false);
        }

        double[] BenchmarkGet(bool isHash)
        {
            double[] result = new double[sizes.Length];
            Random rnd = new Random();

            for (int i = 0; i < sizes.Length; i++)
            {
                double total = 0;

                for (int r = 0; r < runs; r++)
                {
                    if (isHash)
                    {
                        var map = new MyHashMap<int, int>();
                        List<int> keys = new List<int>();

                        for (int j = 0; j < sizes[i]; j++)
                        {
                            int key = rnd.Next();
                            keys.Add(key);
                            map.Put(key, j);
                        }

                        var sw = Stopwatch.StartNew();
                        foreach (var key in keys)
                            map.Get(key);
                        sw.Stop();

                        total += sw.Elapsed.TotalMilliseconds;
                    }
                    else
                    {
                        var map = new MyTreeMap<int, int>();
                        List<int> keys = new List<int>();

                        for (int j = 0; j < sizes[i]; j++)
                        {
                            int key = rnd.Next();
                            keys.Add(key);
                            map.Put(key, j);
                        }

                        var sw = Stopwatch.StartNew();
                        foreach (var key in keys)
                            map.Get(key);
                        sw.Stop();

                        total += sw.Elapsed.TotalMilliseconds;
                    }
                }

                result[i] = total / runs;
            }

            return result;
        }

        // REMOVE

        double[] BenchmarkRemoveHash()
        {
            return BenchmarkRemove(true);
        }

        double[] BenchmarkRemoveTree()
        {
            return BenchmarkRemove(false);
        }

        double[] BenchmarkRemove(bool isHash)
        {
            double[] result = new double[sizes.Length];
            Random rnd = new Random();

            for (int i = 0; i < sizes.Length; i++)
            {
                double total = 0;

                for (int r = 0; r < runs; r++)
                {
                    List<int> keys = new List<int>();

                    if (isHash)
                    {
                        var map = new MyHashMap<int, int>();
                        for (int j = 0; j < sizes[i]; j++)
                        {
                            int key = rnd.Next();
                            keys.Add(key);
                            map.Put(key, j);
                        }

                        var sw = Stopwatch.StartNew();
                        foreach (var key in keys)
                            map.Remove(key);
                        sw.Stop();

                        total += sw.Elapsed.TotalMilliseconds;
                    }
                    else
                    {
                        var map = new MyTreeMap<int, int>();
                        for (int j = 0; j < sizes[i]; j++)
                        {
                            int key = rnd.Next();
                            keys.Add(key);
                            map.Put(key, j);
                        }

                        var sw = Stopwatch.StartNew();
                        foreach (var key in keys)
                            map.Remove(key);
                        sw.Stop();

                        total += sw.Elapsed.TotalMilliseconds;
                    }
                }

                result[i] = total / runs;
            }

            return result;
        }

        // GRAPH

        void DrawGraph(ZedGraphControl control, string title, double[] hashData, double[] treeData)
        {
            GraphPane pane = control.GraphPane;

            pane.Title.Text = title + " Comparison";
            pane.XAxis.Title.Text = "Size";
            pane.YAxis.Title.Text = "Time (ms)";

            pane.CurveList.Clear();

            PointPairList hashList = new PointPairList();
            PointPairList treeList = new PointPairList();

            for (int i = 0; i < sizes.Length; i++)
            {
                hashList.Add(sizes[i], hashData[i]);
                treeList.Add(sizes[i], treeData[i]);
            }

            pane.AddCurve("HashMap", hashList, System.Drawing.Color.Blue);
            pane.AddCurve("TreeMap", treeList, System.Drawing.Color.Red);

            control.AxisChange();
            control.Invalidate();
        }
        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
