using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

public class Collection
{
    // Задание 1: Перевернуть список
    public static List<T> ReverseList<T>(List<T> list)
    {
        int left = 0;
        int right = list.Count - 1;

        while (left < right)
        {
            T temp = list[left];
            list[left] = list[right];
            list[right] = temp;

            left++;
            right--;
        }
        return list;
    }

    // Задание 2: Вставить элемент F слева и справа от каждого элемента E (обобщенная версия)
    public static LinkedList<T> InsertAroundElement<T>(LinkedList<T> list, T e, T f)
    {
        var node = list.First;
        while (node != null)
        {
            if (EqualityComparer<T>.Default.Equals(node.Value, e))
            {
                list.AddBefore(node, f);
                node = list.AddAfter(node, f).Next;
            }
            else
            {
                node = node.Next;
            }
        }
        return list;
    }

    // Задание 3: Определить посещаемость дискотек
    public static void DiscoAnalysis(HashSet<string> discos, Dictionary<string, HashSet<string>> studentVisits)
    {
        // Множество для дискотек, которые посетили все студенты
        HashSet<string> allStudentsDiscos = new HashSet<string>(discos);

        // Множество для дискотек, которые посетили хотя бы некоторые студенты
        HashSet<string> someStudentsDiscos = new HashSet<string>();

        // Множество для всех посещённых студентами дискотек
        HashSet<string> allVisitedDiscos = new HashSet<string>();

        // Проходим по всем записям посещений студентов
        foreach (var student in studentVisits.Values)
        {
            // Пересекаем с множеством, чтобы оставить только те дискотеки, которые посещены всеми студентами
            allStudentsDiscos.IntersectWith(student);

            // Объединяем с множеством, чтобы собрать все дискотеки, которые посещены хотя бы одним студентом
            someStudentsDiscos.UnionWith(student);

            // Добавляем все посещенные студентами дискотеки
            allVisitedDiscos.UnionWith(student);
        }

        // Дискотеки, которые не были посещены ни одним студентом
        HashSet<string> noStudentsDiscos = new HashSet<string>(discos);
        noStudentsDiscos.ExceptWith(allVisitedDiscos);  // исключаем все посещённые дискотеки

        // Выводим результаты
        Console.WriteLine("Дискотеки, которые посетили все студенты: " + string.Join(", ", allStudentsDiscos));
        Console.WriteLine("Дискотеки, которые посетили некоторые студенты: " + string.Join(", ", someStudentsDiscos));
        Console.WriteLine("Дискотеки, которые не посетил ни один студент: " + string.Join(", ", noStudentsDiscos));

    }


    // Задание 4: Печать символов в алфавитном порядке из слов с четными номерами
    public static void PrintEvenWordCharacters(string filePath)
    {
        HashSet<char> characters = new HashSet<char>(); // Создаем множество для хранения уникальных символов.

        using (StreamReader reader = new StreamReader(filePath)) // Открываем файл для чтения
        {
            int wordNumber = 1; // Номер текущего слова
            string[] words = reader.ReadToEnd().Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries); // Читаем все слова из файла и разделяем их по пробелам и символам новой строки

            foreach (string word in words) // Перебираем каждое слово
            {
                if (wordNumber % 2 == 0) // Если номер слова четный
                {
                    foreach (char c in word) // Перебираем каждый символ в слове
                    {
                        if (char.IsLetter(c)) characters.Add(char.ToLower(c)); // Если символ — это буква, добавляем его в множество (без учета регистра)
                    }
                }
                wordNumber++; // Увеличиваем номер слова
            }
        }

        var sortedCharacters = characters.OrderBy(c => c).ToArray(); // Сортируем множество символов в алфавитном порядке и преобразуем в массив
        Console.WriteLine("Символы в алфавитном порядке: " + string.Join(", ", sortedCharacters)); // Выводим отсортированные символы
    }

    // Задание 5
    // Метод для генерации исходного файла с данными абитуриентов через ввод пользователя
    public static void GenerateApplicantData(string filePath)
    {
        Console.Write("Введите количество абитуриентов: ");
        int count;

        // Проверка корректности ввода количества абитуриентов
        while (!int.TryParse(Console.ReadLine(), out count) || count <= 0)
        {
            Console.Write("Некорректное значение! Введите положительное целое число: ");
        }

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(count); // Записываем количество абитуриентов в первую строку файла

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"\nВведите данные для абитуриента #{i + 1}:");

                // Запрашиваем фамилию и проверяем, что это только буквы
                string lastName;
                while (true)
                {
                    Console.Write("Фамилия: ");
                    lastName = Console.ReadLine();
                    if (Regex.IsMatch(lastName, @"^[a-zA-Zа-яА-Я]+$"))
                        break;
                    Console.WriteLine("Некорректная фамилия! Используйте только буквы.");
                }

                // Запрашиваем имя и проверяем, что это только буквы
                string firstName;
                while (true)
                {
                    Console.Write("Имя: ");
                    firstName = Console.ReadLine();
                    if (Regex.IsMatch(firstName, @"^[a-zA-Zа-яА-Я]+$"))
                        break;
                    Console.WriteLine("Некорректное имя! Используйте только буквы.");
                }

                // Запрашиваем и проверяем баллы, чтобы они были целыми числами от 0 до 100
                int score1 = ProvScore("Балл 1");
                int score2 = ProvScore("Балл 2");
                int score3 = ProvScore("Балл 3");

                // Записываем данные абитуриента в файл
                writer.WriteLine($"{lastName} {firstName} {score1} {score2} {score3}");
            }
        }

        Console.WriteLine("Исходный файл с данными абитуриентов успешно создан.");
    }

    // Метод для ввода и проверки баллов
    private static int ProvScore(string prompt)
    {
        int score;
        while (true)
        {
            Console.Write($"{prompt}: ");
            if (int.TryParse(Console.ReadLine(), out score) && score >= 0 && score <= 100)
                return score;
            Console.WriteLine("Некорректное значение! Введите целое число от 0 до 100.");
        }
    }

    // Задание 5: Обработка данных абитуриентов
    public static void ProcessApplicants(string inputFilePath, string outputFilePath)
    {
        // Словарь для хранения абитуриентов, где ключ - уникальный идентификатор (например, фамилия + имя)
        Dictionary<string, Applicant> applicants = new Dictionary<string, Applicant>();

        using (StreamReader reader = new StreamReader(inputFilePath))
        {
            int count = int.Parse(reader.ReadLine());

            for (int i = 0; i < count; i++)
            {
                string[] parts = reader.ReadLine().Split(' ');
                string lastName = parts[0];
                string firstName = parts[1];
                int score1 = int.Parse(parts[2]);
                int score2 = int.Parse(parts[3]);
                int score3 = int.Parse(parts[4]);

                // Создаем уникальный ключ на основе фамилии и имени
                string key = lastName + "_" + firstName;

                // Проверка условий допуска абитуриента
                int sum = score1 + score2 + score3;
                if (score1 >= 30 && score2 >= 30 && score3 >= 30 && sum >= 140)
                {
                    // Добавляем абитуриента в словарь
                    applicants[key] = new Applicant
                    {
                        LastName = lastName,
                        FirstName = firstName,
                        Score1 = score1,
                        Score2 = score2,
                        Score3 = score3
                    };
                }
            }
        }

        // Сортируем абитуриентов по фамилии в алфавитном порядке для вывода
        var sortedApplicants = applicants.Values
            .OrderBy(applicant => applicant.LastName)
            .ThenBy(applicant => applicant.FirstName)
            .ToList();

        // Сериализация и сохранение в XML
        XmlSerializer serializer = new XmlSerializer(typeof(List<Applicant>));
        using (FileStream fs = new FileStream(outputFilePath, FileMode.Create))
        {
            serializer.Serialize(fs, sortedApplicants);
        }

        Console.WriteLine("Допущенные абитуриенты успешно сохранены в XML файл.");
    }
}

    public class Applicant
{
    //public string LastName { get; set; }
    //public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public int Score1 { get; set; }
    public int Score2 { get; set; }
    public int Score3 { get; set; }
}
