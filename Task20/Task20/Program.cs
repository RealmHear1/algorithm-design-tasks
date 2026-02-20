using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        string path = "graph.txt";
        var graphData = ReadGraph(path);

        Console.WriteLine("Tarjan SCC");
        var tarjan = new TarjanSCC(graphData.AdjList);
        tarjan.FindSCC();

        Console.WriteLine("\nDinic Max Flow");
        var dinic = new Dinic(graphData.VertexCount);
        foreach (var edge in graphData.Edges)
            dinic.AddEdge(edge.Item1, edge.Item2, edge.Item3);

        int maxFlow = dinic.MaxFlow(0, graphData.VertexCount - 1);
        Console.WriteLine("Max Flow: " + maxFlow);

        Console.WriteLine("\nClique Heuristic");
        var clique = new CliqueHeuristic(graphData.AdjListUndirected);
        var result = clique.FindClique();
        Console.WriteLine("Clique size: " + result.Count);
        Console.WriteLine("Vertices: " + string.Join(", ", result));
    }

    static GraphData ReadGraph(string path)
    {
        var lines = File.ReadAllLines(path);
        var first = lines[0].Split();
        int n = int.Parse(first[0]);
        int m = int.Parse(first[1]);

        var adj = new List<int>[n];
        var adjUndirected = new List<int>[n];
        for (int i = 0; i < n; i++)
        {
            adj[i] = new List<int>();
            adjUndirected[i] = new List<int>();
        }

        var edges = new List<Tuple<int, int, int>>();

        for (int i = 1; i <= m; i++)
        {
            var parts = lines[i].Split();
            int u = int.Parse(parts[0]);
            int v = int.Parse(parts[1]);
            int w = int.Parse(parts[2]);

            adj[u].Add(v);
            adjUndirected[u].Add(v);
            adjUndirected[v].Add(u);
            edges.Add(new Tuple<int, int, int>(u, v, w));
        }

        return new GraphData
        {
            VertexCount = n,
            AdjList = adj,
            AdjListUndirected = adjUndirected,
            Edges = edges
        };
    }
}

class GraphData
{
    public int VertexCount;
    public List<int>[] AdjList;
    public List<int>[] AdjListUndirected;
    public List<Tuple<int, int, int>> Edges;
}


class TarjanSCC
{
    private List<int>[] graph;
    private int[] ids, low;
    private bool[] onStack;
    private Stack<int> stack;
    private int id = 0;

    public TarjanSCC(List<int>[] graph)
    {
        this.graph = graph;
        int n = graph.Length;
        ids = new int[n];
        low = new int[n];
        onStack = new bool[n];
        stack = new Stack<int>();

        for (int i = 0; i < n; i++)
            ids[i] = -1;
    }

    public void FindSCC()
    {
        for (int i = 0; i < graph.Length; i++)
            if (ids[i] == -1)
                DFS(i);
    }

    private void DFS(int at)
    {
        stack.Push(at);
        onStack[at] = true;
        ids[at] = low[at] = id++;

        foreach (var to in graph[at])
        {
            if (ids[to] == -1)
            {
                DFS(to);
                low[at] = Math.Min(low[at], low[to]);
            }
            else if (onStack[to])
            {
                low[at] = Math.Min(low[at], ids[to]);
            }
        }

        if (ids[at] == low[at])
        {
            Console.Write("SCC: ");
            while (true)
            {
                int node = stack.Pop();
                onStack[node] = false;
                Console.Write(node + " ");
                if (node == at) break;
            }
            Console.WriteLine();
        }
    }
}


class Dinic
{
    class Edge
    {
        public int To, Rev;
        public int Capacity;
        public Edge(int to, int rev, int cap)
        {
            To = to;
            Rev = rev;
            Capacity = cap;
        }
    }

    private List<Edge>[] graph;
    private int[] level, ptr;
    private int n;

    public Dinic(int n)
    {
        this.n = n;
        graph = new List<Edge>[n];
        for (int i = 0; i < n; i++)
            graph[i] = new List<Edge>();
    }

    public void AddEdge(int u, int v, int cap)
    {
        graph[u].Add(new Edge(v, graph[v].Count, cap));
        graph[v].Add(new Edge(u, graph[u].Count - 1, 0));
    }

    public int MaxFlow(int s, int t)
    {
        int flow = 0;
        while (BFS(s, t))
        {
            ptr = new int[n];
            int pushed;
            while ((pushed = DFS(s, t, int.MaxValue)) > 0)
                flow += pushed;
        }
        return flow;
    }

    private bool BFS(int s, int t)
    {
        level = new int[n];
        level = new int[n];
        for (int i = 0; i < n; i++)
            level[i] = -1;
        Queue<int> q = new Queue<int>();
        q.Enqueue(s);
        level[s] = 0;

        while (q.Count > 0)
        {
            int u = q.Dequeue();
            foreach (var e in graph[u])
            {
                if (e.Capacity > 0 && level[e.To] == -1)
                {
                    level[e.To] = level[u] + 1;
                    q.Enqueue(e.To);
                }
            }
        }
        return level[t] != -1;
    }

    private int DFS(int u, int t, int pushed)
    {
        if (pushed == 0) return 0;
        if (u == t) return pushed;

        for (; ptr[u] < graph[u].Count; ptr[u]++)
        {
            var e = graph[u][ptr[u]];
            if (level[e.To] == level[u] + 1 && e.Capacity > 0)
            {
                int tr = DFS(e.To, t, Math.Min(pushed, e.Capacity));
                if (tr == 0) continue;

                e.Capacity -= tr;
                graph[e.To][e.Rev].Capacity += tr;
                return tr;
            }
        }
        return 0;
    }
}


class CliqueHeuristic
{
    private List<int>[] graph;

    public CliqueHeuristic(List<int>[] graph)
    {
        this.graph = graph;
    }

    public List<int> FindClique()
    {
        int n = graph.Length;
        var clique = new List<int>();

        for (int i = 0; i < n; i++)
        {
            bool fits = true;
            foreach (var v in clique)
            {
                if (!graph[i].Contains(v))
                {
                    fits = false;
                    break;
                }
            }
            if (fits)
                clique.Add(i);
        }

        return clique;
    }
}
