using System;
using System.IO;
using System.Text.RegularExpressions;
class Program
{
    static void Main()
    {
        MyArrayList<string> tags = new MyArrayList<string>();
        string[] lines = File.ReadAllLines("input.txt");
        Regex tagPattern = new Regex(@"<\/?[A-Za-z][A-Za-z0-9]*>");
        foreach (string line in lines)
        {
            MatchCollection matches = tagPattern.Matches(line);
            foreach (Match match in matches)
            {
                tags.Add(match.Value);
            }
        }
        Console.WriteLine("Найденные теги:");
        for (int i = 0; i < tags.Size(); i++)
        {
            Console.WriteLine(tags.Get(i));
        }
        RemoveDuplicates(tags);
        Console.WriteLine("\nПосле удаления дубликатов:");
        for (int i = 0; i < tags.Size(); i++)
        {
            Console.WriteLine(tags.Get(i));
        }
    }
    static void RemoveDuplicates(MyArrayList<string> list)
    {
        for (int i = 0; i < list.Size(); i++)
        {
            string current = NormalizeTag(list.Get(i));
            for (int j = i + 1; j < list.Size(); j++)
            {
                string other = NormalizeTag(list.Get(j));
                if (current == other)
                {
                    list.Remove(j);
                    j--; 
                }
            }
        }
    }
    static string NormalizeTag(string tag)
    {
        tag = tag.ToLower();
        if (tag.StartsWith("</"))
            tag = tag.Substring(2, tag.Length - 3); // вырезаем </ и >
        else
            tag = tag.Substring(1, tag.Length - 2); // вырезаем < и >
        return tag;
    }
}

