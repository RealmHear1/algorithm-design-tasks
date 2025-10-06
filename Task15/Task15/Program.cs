using System;
using System.IO;
class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.txt";
            string outputPath = "sorted.txt";
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("Файл input.txt не найден!");
                return;
            }
            string[] lines = File.ReadAllLines(inputPath);
            if (lines.Length == 0)
            {
                Console.WriteLine("Файл пуст!");
                return;
            }
            MyArrayDeque<string> deque = new MyArrayDeque<string>();
            // Добавляем первую строку
            deque.AddLast(lines[0]);
            int firstDigits = CountDigits(lines[0]);
            // Остальные строки вставляем по правилу
            for (int i = 1; i < lines.Length; i++)
            {
                int digits = CountDigits(lines[i]);
                if (digits > firstDigits)
                    deque.AddLast(lines[i]);
                else
                    deque.AddFirst(lines[i]);
            }
            // Записываем в файл sorted.txt
            using (StreamWriter sw = new StreamWriter(outputPath))
            {
                foreach (var line in deque.ToArray())
                    sw.WriteLine(line);
            }
            Console.WriteLine($"Результат записан в файл {outputPath}.");
            // Вводим n и удаляем строки с более чем n пробелами
            Console.Write("Введите n (максимум пробелов в строке): ");
            if (!int.TryParse(Console.ReadLine(), out int n))
            {
                Console.WriteLine("Ошибка: введите целое число.");
                return;
            }
            int initialSize = deque.Size();
            for (int i = 0; i < initialSize; i++)
            {
                string line = deque.RemoveFirst();
                int spaces = CountSpaces(line);

                if (spaces <= n)
                    deque.AddLast(line);
            }
            Console.WriteLine("\nОставшиеся строки:");
            foreach (var line in deque.ToArray())
                Console.WriteLine(line);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
    static int CountDigits(string s)
    {
        int count = 0;
        foreach (char c in s)
            if (char.IsDigit(c))
                count++;
        return count;
    }
    static int CountSpaces(string s)
    {
        int count = 0;
        foreach (char c in s)
            if (c == ' ')
                count++;
        return count;
    }
}