using System;
using System.Collections.Generic;
using System.Linq;

namespace CrosswordGridGame
{
    class Program
    {
        class Word
        {
            public string Text, Question;
            public int Row, Col, Number;
            public bool IsHorizontal, IsGuessed;

            public Word(string text, int row, int col, bool isHorizontal, string question, int number)
            {
                Text = text.ToUpper();
                Question = question;
                Row = row;
                Col = col;
                IsHorizontal = isHorizontal;
                Number = number;
                IsGuessed = false;
            }

            public bool Check(string answer) => answer.ToUpper() == Text;
        }

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            do
            {
                Play();
                Console.Write("\n\nЕще раз? (д/н): ");
            } while (Console.ReadLine()?.ToLower() is "д" or "да" or "y" or "yes");
            Console.WriteLine("\nСпасибо за игру!");
        }

        static void Play()
        {
            var words = new List<Word>
            {
                new Word("ПАРИЖ", 2, 2, true, "Столица Франции", 1),
                new Word("СИШАРП", 4, 1, true, "Язык программирования", 2),
                new Word("ВИНДОВС", 6, 2, true, "Операционная система", 3),
                new Word("ЭВЕРЕСТ", 8, 3, true, "Самая высокая гора", 4),
                new Word("КОТ", 2, 7, false, "Домашнее животное", 5),
                new Word("СОЛНЦЕ", 4, 8, false, "Центральная звезда", 6),
                new Word("МОРЕ", 6, 9, false, "Большое соленое пространство", 7),
                new Word("ЗЕМЛЯ", 8, 7, false, "Наша планета", 8)
            };

            var grid = new char[15, 15];
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    grid[i, j] = ' ';

            // Размещаем слова на сетке
            foreach (var w in words)
                for (int i = 0; i < w.Text.Length; i++)
                {
                    int r = w.Row + (w.IsHorizontal ? 0 : i);
                    int c = w.Col + (w.IsHorizontal ? i : 0);
                    if (grid[r, c] == ' ') grid[r, c] = '□';
                    else if (grid[r, c] == '□') grid[r, c] = '■';
                }

            int score = 0;
            while (score < words.Count)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════╗");
                Console.WriteLine("║           КОНСОЛЬНЫЙ КРОССВОРД             ║");
                Console.WriteLine("╚════════════════════════════════════════════╝\n");

                ShowGrid(grid, words);
                ShowQuestions(words);

                Console.WriteLine($"\nПрогресс: {score}/{words.Count}");

                Console.Write($"\nВыберите номер (1-{words.Count}): ");
                if (!int.TryParse(Console.ReadLine(), out int num) || num < 1 || num > words.Count)
                {
                    Console.WriteLine("Неверный номер!");
                    Console.ReadKey();
                    continue;
                }

                var word = words[num - 1];
                if (word.IsGuessed)
                {
                    Console.WriteLine("Уже разгадано!");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine($"\nВопрос {num}: {word.Question}");
                Console.Write($"Слово ({word.Text.Length} букв): ");
                string answer = Console.ReadLine()?.ToUpper().Trim();

                if (word.Check(answer))
                {
                    Console.WriteLine("Правильно!");
                    word.IsGuessed = true;
                    score++;

                    // Заполняем слово
                    for (int i = 0; i < word.Text.Length; i++)
                    {
                        int r = word.Row + (word.IsHorizontal ? 0 : i);
                        int c = word.Col + (word.IsHorizontal ? i : 0);
                        grid[r, c] = word.Text[i];
                    }
                }
                else
                {
                    Console.WriteLine($"Неправильно! Подсказка: {word.Text[0]}...{word.Text[^1]}");
                }

                if (score < words.Count)
                {
                    Console.WriteLine("\nНажмите любую клавишу...");
                    Console.ReadKey();
                }
            }

            Console.Clear();
            ShowGrid(grid, words);
            Console.WriteLine("\n╔════════════════════════════════════════════╗");
            Console.WriteLine("║        ПОЗДРАВЛЯЕМ! КРОССВОРД РАЗГАДАН!     ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");
            Console.WriteLine($"\nРезультат: {score}/{words.Count} слов");
        }

        static void ShowGrid(char[,] grid, List<Word> words)
        {
            // Находим границы
            int minR = 15, maxR = 0, minC = 15, maxC = 0;
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    if (grid[i, j] != ' ')
                    {
                        minR = Math.Min(minR, i);
                        maxR = Math.Max(maxR, i);
                        minC = Math.Min(minC, j);
                        maxC = Math.Max(maxC, j);
                    }

            minR = Math.Max(0, minR - 1);
            maxR = Math.Min(14, maxR + 1);
            minC = Math.Max(0, minC - 1);
            maxC = Math.Min(14, maxC + 1);

            // Заголовок
            Console.Write("    ");
            for (int j = minC; j <= maxC; j++)
                Console.Write($"{j + 1,3}");
            Console.WriteLine("\n   ┌" + new string('─', (maxC - minC + 1) * 3 + 1) + "┐");

            // Сетка
            for (int i = minR; i <= maxR; i++)
            {
                Console.Write($"{i + 1,2} │");
                for (int j = minC; j <= maxC; j++)
                {
                    char c = grid[i, j];
                    string ch = c switch
                    {
                        ' ' => "   ",
                        '□' => GetNumberAt(words, i, j) ?? " · ",
                        '■' => " ╬ ",
                        _ => $" {c} "
                    };
                    Console.Write(ch);
                }
                Console.WriteLine("│");
            }
            Console.WriteLine("   └" + new string('─', (maxC - minC + 1) * 3 + 1) + "┘");
            Console.WriteLine("\n· - пустая клетка  ╬ - пересечение  цифры - номер слова");
        }

        static string GetNumberAt(List<Word> words, int row, int col)
        {
            foreach (var w in words.Where(w => !w.IsGuessed))
            {
                if (w.IsHorizontal && row == w.Row && col >= w.Col && col < w.Col + w.Text.Length && col == w.Col)
                    return $"{w.Number,2}";
                if (!w.IsHorizontal && col == w.Col && row >= w.Row && row < w.Row + w.Text.Length && row == w.Row)
                    return $"{w.Number,2}";
            }
            return null;
        }

        static void ShowQuestions(List<Word> words)
        {
            Console.WriteLine("\n╔════════════════════════════════════════════╗");
            Console.WriteLine("║                  ВОПРОСЫ                   ║");
            Console.WriteLine("╠════════════════════════════════════════════╣");

            var horizontal = words.Where(w => w.IsHorizontal && !w.IsGuessed).ToList();
            var vertical = words.Where(w => !w.IsHorizontal && !w.IsGuessed).ToList();

            if (horizontal.Any())
            {
                Console.WriteLine("║  ПО ГОРИЗОНТАЛИ:");
                foreach (var w in horizontal)
                    Console.WriteLine($"║    {w.Number}. {w.Question}");
            }

            if (vertical.Any())
            {
                Console.WriteLine("║  ПО ВЕРТИКАЛИ:");
                foreach (var w in vertical)
                    Console.WriteLine($"║    {w.Number}. {w.Question}");
            }

            var guessed = words.Where(w => w.IsGuessed).ToList();
            if (guessed.Any())
            {
                Console.WriteLine("║");
                Console.WriteLine("║  РАЗГАДАНО:");
                foreach (var w in guessed)
                    Console.WriteLine($"║    {w.Number}. {w.Text} {(w.IsHorizontal ? "→" : "↓")}");
            }

            Console.WriteLine("╚════════════════════════════════════════════╝");
        }
    }
}