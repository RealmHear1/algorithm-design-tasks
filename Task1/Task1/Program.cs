using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Чтение всех строк из файла
            // Пришлось указывать полный путь, почему то visual studio отказывается искать .txt файл
            string[] lines = File.ReadAllLines("input.txt");
            // Первая строка — размерность пространства
            int n = Convert.ToInt32(lines[0]);
            // Массив для матрицы G
            double[,] G = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                string[] row = lines[i + 1].Split(' ');
                for (int j = 0; j < n; j++)
                {
                    G[i, j] = double.Parse(row[j]);
                }
            }
            // Чтение вектора x
            double[] x = new double[n];
            string[] vectorLine = lines[n + 1].Split(' ');
            for (int i = 0; i < n; i++)
                x[i] = Convert.ToDouble(vectorLine[i]);
            if (!IsSymmetric(G, n))
            {
                Console.WriteLine("Ошибка: матрица G не симметрична.");
                return;
            }
            // Вычисление длины вектора
            double length = ComputeLength(x, G, n);
            Console.WriteLine($"Длина вектора: {length:F6}");
        }
        static bool IsSymmetric(double[,] G, int n)
        {
            for (int i = 0; i < n; i++)
                for (int j = i + 1; j < n; j++)
                    if (G[i, j] != G[j, i])
                        return false;
            return true;
        }
        static double ComputeLength(double[] x, double[,] G, int n)
        {
            double sum = 0.0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    sum += x[i] * G[i, j] * x[j];
            return Math.Sqrt(sum);
        }
    }
}
