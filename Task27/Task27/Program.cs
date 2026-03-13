using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        MyHashSet<string> set = new MyHashSet<string>();

        if (!File.Exists("input.txt"))
        {
            Console.WriteLine("File input.txt not found");
            return;
        }

        string text = File.ReadAllText("input.txt");

        Regex regex = new Regex("[A-Za-z]+");

        MatchCollection matches = regex.Matches(text);

        for (int i = 0; i < matches.Count; i++)
        {
            string word = matches[i].Value.ToLower();

            set.add(word);
        }

        Console.WriteLine("Unique words:");

        string[] words = set.toArray(new string[0]);

        for (int i = 0; i < words.Length; i++)
        {
            Console.WriteLine(words[i]);
        }

        Console.ReadLine();
    }
}