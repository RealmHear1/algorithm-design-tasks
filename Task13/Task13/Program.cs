using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Использование: Task13.exe \"выражение\" [a=5 b=10 ...]");
            return;
        }
        string expression = args[0];
        Dictionary<string, double> variables = new Dictionary<string, double>();
        for (int i = 1; i < args.Length; i++)
        {
            string[] parts = args[i].Split('=');
            if (parts.Length == 2 && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
            {
                variables[parts[0]] = val;
            }
            else
            {
                Console.WriteLine($"Ошибка: неверный параметр {args[i]}");
                return;
            }
        }
        try
        {
            var tokens = Tokenize(expression);
            var rpn = ToRPN(tokens);
            double result = EvaluateRPN(rpn, variables);
            Console.WriteLine(result.ToString(CultureInfo.InvariantCulture));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }
    static List<string> Tokenize(string expr)
    {
        List<string> tokens = new List<string>();
        string number = "";
        for (int i = 0; i < expr.Length; i++)
        {
            char c = expr[i];
            if (char.IsWhiteSpace(c))
                continue;
            if (char.IsDigit(c) || c == '.')
            {
                number += c;
            }
            else
            {
                if (number.Length > 0)
                {
                    tokens.Add(number);
                    number = "";
                }
                if ("()+-*/^".Contains(c))
                {
                    tokens.Add(c.ToString());
                }
                else if (char.IsLetter(c))
                {
                    string func = c.ToString();
                    while (i + 1 < expr.Length && char.IsLetter(expr[i + 1]))
                    {
                        func += expr[++i];
                    }
                    tokens.Add(func);
                }
                else
                {
                    throw new Exception($"Неизвестный символ: {c}");
                }
            }
        }
        if (number.Length > 0)
        {
            tokens.Add(number);
        }
        return tokens;
    }
    // Приоритеты операций
    static int Priority(string op)
    {
        switch (op)
        {
            case "+":
            case "-":
                return 1;
            case "*":
            case "/":
            case "%":
                return 2;
            case "^":
                return 3;
            default:
                return 0;
        }
    }
    // Преобразование в ОПН (алгоритм сортировочной станции)
    static List<string> ToRPN(List<string> tokens)
    {
        List<string> output = new List<string>();
        MyStack<string> ops = new MyStack<string>();
        foreach (var token in tokens)
        {
            if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out _) || char.IsLetter(token[0]))
            {
                output.Add(token);
            }
            else if (token == "(")
            {
                ops.Push(token);
            }
            else if (token == ")")
            {
                while (!ops.Empty() && ops.Peek() != "(")
                {
                    output.Add(ops.Pop());
                }
                if (ops.Empty())
                    throw new Exception("Несогласованные скобки");
                ops.Pop();
            }
            else 
            {
                while (!ops.Empty() && Priority(ops.Peek()) >= Priority(token))
                {
                    output.Add(ops.Pop());
                }
                ops.Push(token);
            }
        }
        while (!ops.Empty())
        {
            if (ops.Peek() == "(" || ops.Peek() == ")")
                throw new Exception("Несогласованные скобки");
            output.Add(ops.Pop());
        }
        return output;
    }
    // Вычисление ОПН
    static double EvaluateRPN(List<string> rpn, Dictionary<string, double> vars)
    {
        MyStack<double> stack = new MyStack<double>();
        foreach (var token in rpn)
        {
            if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
            {
                stack.Push(num);
            }
            else if (vars.ContainsKey(token))
            {
                stack.Push(vars[token]);
            }
            else
            {
                if (token == "+")
                {
                    double b = stack.Pop(), a = stack.Pop();
                    stack.Push(a + b);
                }
                else if (token == "-")
                {
                    double b = stack.Pop(), a = stack.Pop();
                    stack.Push(a - b);
                }
                else if (token == "*")
                {
                    double b = stack.Pop(), a = stack.Pop();
                    stack.Push(a * b);
                }
                else if (token == "/")
                {
                    double b = stack.Pop(), a = stack.Pop();
                    if (b == 0) throw new DivideByZeroException();
                    stack.Push(a / b);
                }
                else if (token == "^")
                {
                    double b = stack.Pop(), a = stack.Pop();
                    stack.Push(Math.Pow(a, b));
                }
                else
                {
                    throw new Exception($"Неизвестная операция: {token}");
                }
            }
        }
        if (stack.Size() != 1)
            throw new Exception("Некорректное выражение");
        return stack.Pop();
    }
}

// Пример входных данных .\Task13.exe "3 + 4 * 2 / (1 - 5) ^ 2" Вывод: 3.5
// Или .\Task13.exe "a * b + c" "a=5" "b=10" "c=3" Вывод: 53
