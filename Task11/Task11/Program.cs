using System;
using System.IO;
class Program
{
    static bool IsValidIP(string s)
    {
        string[] parts = s.Split('.');
        if (parts.Length != 4) return false;
        foreach (string part in parts)
        {
            if (part.Length == 0) return false;
            int num;
            if (!int.TryParse(part, out num)) return false;
            if (num < 0 || num > 255) return false;
            if (part.Length > 1 && part[0] == '0') return false;
        }
        return true;
    }
    static void Main()
    {
        MyVector<string> inputVec = new MyVector<string>();
        foreach (string line in File.ReadAllLines("input.txt"))
        {
            inputVec.Add(line);
        }
        MyVector<string> ipVec = new MyVector<string>();
        // Обработка строк
        for (int i = 0; i < inputVec.Size(); i++)
        {
            string[] tokens = inputVec.Get(i).Split(' ', '\t', ',', ';', ':');

            foreach (string token in tokens)
            {
                if (IsValidIP(token))
                {
                    ipVec.Add(token);
                }
            }
        }
        // Запись IP в файл output.txt
        using (StreamWriter writer = new StreamWriter("output.txt"))
        {
            for (int i = 0; i < ipVec.Size(); i++)
            {
                writer.WriteLine(ipVec.Get(i));
            }
        }
        Console.WriteLine("IP-адреса успешно сохранены в output.txt");
        Console.WriteLine("Содержимое output.txt:");
        foreach (string line in File.ReadAllLines("output.txt"))
        {
            Console.WriteLine(line);
        }
    }
}

