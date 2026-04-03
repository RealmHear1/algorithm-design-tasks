using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    const int SIZE = 20;
    static char?[,] grid = new char?[SIZE, SIZE];
    static List<string> words = new List<string>();
    static int solutionCount = 0;

    static void Main()
    {
        Console.WriteLine("Введите слова (через пробел):");

        words = Console.ReadLine()
    .ToUpper()
    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
    .Distinct()
    .OrderByDescending(w => w.Length)
    .ToList();

        if (words.Count == 0)
        {
            Console.WriteLine("Нет слов");
            return;
        }

        // Первое слово в центр
        string first = words[0];
        int row = SIZE / 2;
        int col = (SIZE - first.Length) / 2;

        PlaceWord(first, row, col, true);
        words.RemoveAt(0);

        Backtrack(0);

        if (solutionCount == 0)
        {
            Console.WriteLine("Решение не существует");
        }
        else if (solutionCount > 1)
        {
            Console.WriteLine("\nНайдено несколько решений (неоднозначность)");
        }
    }

    static void Backtrack(int index)
    {
        if (index == words.Count)
        {
            solutionCount++;

            if (solutionCount == 1)
            {
                Console.WriteLine("Найдено решение:\n");
                PrintGrid();
            }

            return;
        }

        string word = words[index];

        var positions = GetBestPositions(word);

        foreach (var pos in positions)
        {
            PlaceWord(word, pos.Row, pos.Col, pos.Horizontal);
            Backtrack(index + 1);
            RemoveWord(word, pos.Row, pos.Col, pos.Horizontal);
        }
    }

    static List<Position> GetBestPositions(string word)
    {
        List<Position> result = new List<Position>();

        for (int r = 0; r < SIZE; r++)
        {
            for (int c = 0; c < SIZE; c++)
            {
                foreach (bool horizontal in new[] { true, false })
                {
                    int score = CanPlace(word, r, c, horizontal);
                    if (score >= 0)
                    {
                        result.Add(new Position(r, c, horizontal, score));
                    }
                }
            }
        }

        return result.OrderByDescending(p => p.Score).ToList();
    }

    static int CanPlace(string word, int r, int c, bool horizontal)
    {
        int score = 0;

        for (int i = 0; i < word.Length; i++)
        {
            int rr = r + (horizontal ? 0 : i);
            int cc = c + (horizontal ? i : 0);

            if (rr >= SIZE || cc >= SIZE)
                return -1;

            if (grid[rr, cc] != null)
            {
                if (grid[rr, cc] != word[i])
                    return -1;

                score++; // пересечение
            }
        }

        // Требуем хотя бы одно пересечение (кроме первого слова)
        if (score == 0 && HasAnyLetter())
            return -1;

        return score;
    }

    static bool HasAnyLetter()
    {
        foreach (var c in grid)
            if (c != null) return true;
        return false;
    }

    static void PlaceWord(string word, int r, int c, bool horizontal)
    {
        for (int i = 0; i < word.Length; i++)
        {
            int rr = r + (horizontal ? 0 : i);
            int cc = c + (horizontal ? i : 0);
            grid[rr, cc] = word[i];
        }
    }

    static void RemoveWord(string word, int r, int c, bool horizontal)
    {
        for (int i = 0; i < word.Length; i++)
        {
            int rr = r + (horizontal ? 0 : i);
            int cc = c + (horizontal ? i : 0);

            // Проверка: не пересечение ли это
            bool isIntersection = false;

            foreach (var dir in new[] { (1, 0), (-1, 0), (0, 1), (0, -1) })
            {
                int nr = rr + dir.Item1;
                int nc = cc + dir.Item2;

                if (nr >= 0 && nr < SIZE && nc >= 0 && nc < SIZE)
                {
                    if (grid[nr, nc] != null && grid[nr, nc] != word[i])
                    {
                        isIntersection = true;
                    }
                }
            }

            if (!isIntersection)
                grid[rr, cc] = null;
        }
    }

    static void PrintGrid()
    {
        for (int r = 0; r < SIZE; r++)
        {
            for (int c = 0; c < SIZE; c++)
            {
                Console.Write(grid[r, c] ?? '.');
                Console.Write(' ');
            }
            Console.WriteLine();
        }
    }

    class Position
    {
        public int Row;
        public int Col;
        public bool Horizontal;
        public int Score;

        public Position(int r, int c, bool h, int s)
        {
            Row = r;
            Col = c;
            Horizontal = h;
            Score = s;
        }
    }
}
// SUN MOON STAR PLANET COMET ASTEROID GALAXY ORBIT ROCKET SPACE
// APPLE PEAR GRAPE DOG CAT
// CAT DOG RAT BAT