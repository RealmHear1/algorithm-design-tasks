using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    class ComplexNumber
    {
        public double Real { get; set; }
        public double Imag { get; set; }
        public ComplexNumber(double real, double imag)
        {
            Real = real;
            Imag = imag;
        }
        // Сложение
        public void Add(ComplexNumber other)
        {
            Real += other.Real;
            Imag += other.Imag;
        }
        // Вычитание
        public void Subtract(ComplexNumber other)
        {
            Real -= other.Real;
            Imag -= other.Imag;
        }
        // Умножение
        public void Multiply(ComplexNumber other)
        {
            double r = Real * other.Real - Imag * other.Imag;
            double i = Real * other.Imag + Imag * other.Real;
            Real = r;
            Imag = i;
        }
        // Деление
        public void Divide(ComplexNumber other)
        {
            double module = other.Real * other.Real + other.Imag * other.Imag;
            if (module == 0)
            {
                Console.WriteLine("Ошибка: деление на ноль!");
                return;
            }
            double r = (Real * other.Real + Imag * other.Imag) / module;
            double i = (Imag * other.Real - Real * other.Imag) / module;
            Real = r;
            Imag = i;
        }
        // Модуль
        public double Modulus()
        {
            return Math.Sqrt(Real * Real + Imag * Imag);
        }
        // Аргумент
        public double Argument()
        {
            return Math.Atan2(Imag, Real);
        }
        // Вывод
        public override string ToString()
        {
            if (Imag >= 0)
                return $"{Real} + {Imag}i";
            else
                return $"{Real} - {-Imag}i";
        }
    }
    class Program
    {
        static void Main()
        {
            ComplexNumber current = new ComplexNumber(0, 0);
            while (true)
            {
                Console.WriteLine("\nТекущее число: " + current);
                Console.WriteLine("Меню:");
                Console.WriteLine("1 - Ввести число");
                Console.WriteLine("2 - Сложение");
                Console.WriteLine("3 - Вычитание");
                Console.WriteLine("4 - Умножение");
                Console.WriteLine("5 - Деление");
                Console.WriteLine("6 - Модуль");
                Console.WriteLine("7 - Аргумент");
                Console.WriteLine("8 - Показать действительную часть");
                Console.WriteLine("9 - Показать мнимую часть");
                Console.WriteLine("Q или q - Выход");
                Console.Write("Введите команду: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Введите действительную часть: ");
                        double real = double.Parse(Console.ReadLine());
                        Console.Write("Введите мнимую часть: ");
                        double imag = double.Parse(Console.ReadLine());
                        current = new ComplexNumber(real, imag);
                        break;
                    case "2":
                        Console.WriteLine("Введите второе число:");
                        ComplexNumber addNum = InputComplex();
                        current.Add(addNum);
                        break;
                    case "3":
                        Console.WriteLine("Введите второе число:");
                        ComplexNumber subNum = InputComplex();
                        current.Subtract(subNum);
                        break;
                    case "4":
                        Console.WriteLine("Введите второе число:");
                        ComplexNumber mulNum = InputComplex();
                        current.Multiply(mulNum);
                        break;
                    case "5":
                        Console.WriteLine("Введите второе число:");
                        ComplexNumber divNum = InputComplex();
                        current.Divide(divNum);
                        break;
                    case "6":
                        Console.WriteLine("Модуль: " + current.Modulus());
                        break;
                    case "7":
                        Console.WriteLine("Аргумент (в радианах): " + current.Argument());
                        break;
                    case "8":
                        Console.WriteLine("Действительная часть: " + current.Real);
                        break;
                    case "9":
                        Console.WriteLine("Мнимая часть: " + current.Imag);
                        break;
                    case "Q":
                    case "q":
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда");
                        break;
                }
            }
        }
        static ComplexNumber InputComplex()
        {
            Console.Write("Введите действительную часть: ");
            double real = double.Parse(Console.ReadLine());
            Console.Write("Введите мнимую часть: ");
            double imag = double.Parse(Console.ReadLine());
            return new ComplexNumber(real, imag);
        }
    }
}
