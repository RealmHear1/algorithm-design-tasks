using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        MyHashSet<LineData> set = new MyHashSet<LineData>();

        if (!File.Exists("input.txt"))
        {
            Console.WriteLine("File input.txt not found");
            return;
        }

        string[] lines = File.ReadAllLines("input.txt");

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            if (line == null || line.Trim() == "")
                continue;

            LineData data = new LineData(line);
            set.add(data);
        }

        Console.WriteLine("Result set:");

        LineData[] arr = set.toArray(new LineData[0]);

        for (int i = 0; i < arr.Length; i++)
        {
            Console.WriteLine(arr[i].OriginalLine);
        }

        Console.ReadLine();
    }
}

class LineData : IComparable<LineData>
{
    public string OriginalLine;
    private List<int> wordLengths;

    public LineData(string line)
    {
        OriginalLine = line;
        wordLengths = new List<int>();

        string[] words = line.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            string w = words[i];

            if (w != "")
            {
                wordLengths.Add(w.Length);
            }
        }

        wordLengths.Sort();
    }

    public int CompareTo(LineData other)
    {
        int min = wordLengths.Count;

        if (other.wordLengths.Count < min)
            min = other.wordLengths.Count;

        for (int i = 0; i < min; i++)
        {
            if (wordLengths[i] < other.wordLengths[i])
                return -1;

            if (wordLengths[i] > other.wordLengths[i])
                return 1;
        }

        if (wordLengths.Count < other.wordLengths.Count)
            return -1;

        if (wordLengths.Count > other.wordLengths.Count)
            return 1;

        return 0;
    }

    public override bool Equals(object obj)
    {
        LineData other = obj as LineData;

        if (other == null)
            return false;

        return CompareTo(other) == 0;
    }

    public override int GetHashCode()
    {
        int hash = 17;

        for (int i = 0; i < wordLengths.Count; i++)
        {
            hash = hash * 31 + wordLengths[i];
        }

        return hash;
    }
}