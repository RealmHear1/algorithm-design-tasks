using System;
using System.Collections.Generic;
using System.IO;
using Task6;

namespace Task7
{
    class Request
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public int Step { get; set; }
        public override string ToString()
        {
            return $"Заявка {Id}, Приоритет {Priority}, Шаг добавления {Step}";
        }
    }
    class RequestComparer : IComparer<Request>
    {
        public int Compare(Request x, Request y)
        {
            return y.Priority.CompareTo(x.Priority);
        }
    }
    class Program
    {
        static void Main()
        {
            Console.Write("Введите количество шагов N: ");
            int N = int.Parse(Console.ReadLine());
            MyPriorityQueue<Request> queue = new MyPriorityQueue<Request>(11, new RequestComparer());
            Random rnd = new Random();
            int requestCounter = 0;
            int maxWait = -1;
            Request maxWaitRequest = null;
            using (StreamWriter log = new StreamWriter("result.txt", false))
            {
                for (int step = 1; step <= N; step++)
                {
                    int k = rnd.Next(1, 11);
                    for (int i = 0; i < k; i++)
                    {
                        requestCounter++;
                        Request req = new Request
                        {
                            Id = requestCounter,
                            Priority = rnd.Next(1, 6),
                            Step = step
                        };
                        queue.Add(req);
                        log.WriteLine($"ADD {req.Id} {req.Priority} {req.Step}");
                    }
                    if (!queue.IsEmpty())
                    {
                        Request removed = queue.Poll();
                        log.WriteLine($"REMOVE {removed.Id} {removed.Priority} {step}");
                        int wait = step - removed.Step;
                        if (wait > maxWait)
                        {
                            maxWait = wait;
                            maxWaitRequest = removed;
                        }
                    }
                }
                int stepRemove = N;
                while (!queue.IsEmpty())
                {
                    stepRemove++;
                    Request removed = queue.Poll();
                    log.WriteLine($"REMOVE {removed.Id} {removed.Priority} {stepRemove}");
                    int wait = stepRemove - removed.Step;
                    if (wait > maxWait)
                    {
                        maxWait = wait;
                        maxWaitRequest = removed;
                    }
                }
            }
            Console.WriteLine("\nЗаявка с максимальным временем ожидания:");
            Console.WriteLine($"{maxWaitRequest}, Время ожидания: {maxWait}");
            Console.WriteLine("Все действия записаны в result.txt");
            Console.WriteLine("Содержимое result.txt");
            Console.WriteLine(File.ReadAllText("result.txt"));
        }
    }
}

