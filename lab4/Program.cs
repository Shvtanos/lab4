using lab4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace lab4
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                // Задание 1: Ввод и переворот списка
                Console.WriteLine("Введите числа для списка, разделенные пробелом:");
                List<double> list = ParseListInput(Console.ReadLine());
                Console.WriteLine("Исходный список: " + string.Join(", ", list));
                Collection.ReverseList(list);
                Console.WriteLine("Перевернутый список: " + string.Join(", ", list));

                // Задание 2: Ввод данных для связного списка и вставка элементов
                Console.WriteLine("\nВведите числа для связного списка, разделенные пробелом:");
                LinkedList<double> linkedList = new LinkedList<double>(ParseListInput(Console.ReadLine()));

                Console.WriteLine("Введите элемент E, вокруг которого будет вставляться элемент F:");
                double e = ParseSingleDoubleInput(Console.ReadLine());

                Console.WriteLine("Введите элемент F, который будет вставлен слева и справа от каждого элемента E:");
                double f = ParseSingleDoubleInput(Console.ReadLine());

                Console.WriteLine("Исходный связный список: " + string.Join(" -> ", linkedList));
                Collection.InsertAroundElement(linkedList, e, f);
                Console.WriteLine("Модифицированный связный список: " + string.Join(" -> ", linkedList));

                // Задание 3: Ввод данных для анализа посещаемости дискотек
                Console.WriteLine("\nВведите названия дискотек через запятую:");
                HashSet<string> discos = new HashSet<string>(Console.ReadLine().Split(','));

                Dictionary<string, HashSet<string>> studentVisits = new Dictionary<string, HashSet<string>>();
                Console.WriteLine("Введите данные о посещении дискотек студентами в формате 'Имя: дискотека1, дискотека2'. Введите пустую строку для завершения:");
                while (true)
                {
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input)) break;

                    string[] parts = input.Split(':');
                    if (parts.Length != 2)
                    {
                        Console.WriteLine("Неверный формат. Попробуйте еще раз.");
                        continue;
                    }

                    string studentName = parts[0].Trim();
                    HashSet<string> visits = new HashSet<string>(parts[1].Split(',').Select(d => d.Trim()));
                    studentVisits[studentName] = visits;
                }
                Console.WriteLine("\nАнализ посещаемости дискотек:");
                Collection.DiscoAnalysis(discos, studentVisits);

                // Задание 4: Печать символов из четных слов в текстовом файле
                string textFilePath = "text.txt";
                Console.WriteLine($"\nСимволы из четных слов в алфавитном порядке (файл {textFilePath}):");
                Collection.PrintEvenWordCharacters(textFilePath);

                // Задание 5: Ввод данных абитуриентов из файла и сохранение в XML
                string inputFilePath = "applicants.txt";
                string outputFilePath = "admittedApplicants.xml";
                Console.WriteLine("\nОбработка данных абитуриентов:");
                Collection.ProcessApplicants(inputFilePath, outputFilePath);
                Console.WriteLine("Допущенные абитуриенты записаны в файл " + outputFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }
        }

        // Метод для разбора строки с числами и преобразования их в список (поддержка целых и вещественных)
        private static List<double> ParseListInput(string input)
        {
            List<double> list = new List<double>();
            foreach (string item in input.Split(' '))
            {
                if (double.TryParse(item, out double number))
                {
                    list.Add(number);
                }
                else
                {
                    Console.WriteLine($"'{item}' не является числом и будет проигнорировано.");
                }
            }
            return list;
        }

        // Метод для разбора одиночного числа (поддержка целых и вещественных)
        private static double ParseSingleDoubleInput(string input)
        {
            while (!double.TryParse(input, out double result))
            {
                Console.WriteLine("Неверный ввод. Пожалуйста, введите число:");
                input = Console.ReadLine();
            }
            return double.Parse(input);
        }
    }
}