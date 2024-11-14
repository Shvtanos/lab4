using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    // Задание 2: Вставить элемент F слева и справа от каждого элемента E
    public static LinkedList<double> InsertAroundElement(LinkedList<double> list, double e, double f)
    {
        var node = list.First;
        while (node != null)
        {
            if (node.Value == e)
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

    // Задание 5: Обработка данных абитуриентов
    public static void ProcessApplicants(string inputFilePath, string outputFilePath)
    {
        // Словарь для хранения абитуриентов, где ключ - уникальный идентификатор (например, фамилия + имя)
        Dictionary<string, Applicant> applicants = new Dictionary<string, Applicant>();

        using (StreamReader reader = new StreamReader(inputFilePath)) // Открываем файл для чтения
        {
            int count = int.Parse(reader.ReadLine()); // Читаем количество абитуриентов

            for (int i = 0; i < count; i++) // Перебираем данные всех абитуриентов
            {
                string[] parts = reader.ReadLine().Split(' '); // Разбиваем строку на части (фамилия, имя, баллы)
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
                    // Добавляем абитуриента в словарь с уникальным ключом
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
        var sortedApplicants = applicants.Values.OrderBy(applicant => applicant.LastName).ThenBy(applicant => applicant.FirstName).ToList(); // Сортируем абитуриентов по фамилии и имени, используя LINQ

        // Сериализация и сохранение в XML
        XmlSerializer serializer = new XmlSerializer(typeof(List<Applicant>)); // Создаем объект сериализатора
        using (FileStream fs = new FileStream(outputFilePath, FileMode.Create)) // Открываем файл для записи
        {
            serializer.Serialize(fs, sortedApplicants); // Сериализуем и сохраняем список абитуриентов в XML файл
        }

        Console.WriteLine("Допущенные абитуриенты успешно сохранены в XML файл."); // Выводим сообщение об успешной записи данных
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
