using System;
using System.Collections.Generic;

namespace CrosswordGame
{
    class Program
    {
        // Структура для хранения вопроса и ответа
        struct CrosswordClue
        {
            public string Question;
            public string Answer;
            public bool IsAnswered;

            public CrosswordClue(string question, string answer)
            {
                Question = question;
                Answer = answer.ToUpper();
                IsAnswered = false;
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║         КОНСОЛЬНЫЙ КРОССВОРД         ║");
            Console.WriteLine("╚══════════════════════════════════════╝");

            bool playAgain = true;

            while (playAgain)
            {
                PlayCrossword();

                Console.Write("\nХочешь разгадать другой кроссворд? (д/н): ");
                string response = Console.ReadLine().ToLower();
                playAgain = (response == "д" || response == "да" || response == "y" || response == "yes");
            }

            Console.WriteLine("\nСпасибо за игру! До свидания!");
        }

        static void PlayCrossword()
        {
            // Создаем кроссворд
            List<CrosswordClue> clues = new List<CrosswordClue>
            {
                new CrosswordClue("Столица Франции", "ПАРИЖ"),
                new CrosswordClue("Язык программирования", "СИШАРП"),
                new CrosswordClue("Операционная система", "ВИНДОВС"),
                new CrosswordClue("Самая высокая гора в мире", "ЭВЕРЕСТ"),
                new CrosswordClue("Планета, на которой мы живем", "ЗЕМЛЯ"),
                new CrosswordClue("Цвет солнца", "ЖЁЛТЫЙ"),
                new CrosswordClue("1000 метров - это 1 ...", "КИЛОМЕТР"),
                new CrosswordClue("День недели после пятницы", "СУББОТА")
            };

            int score = 0;
            int totalQuestions = clues.Count;

            Console.WriteLine($"\nВсего вопросов: {totalQuestions}");
            Console.WriteLine("Вводи ответы БОЛЬШИМИ буквами (можно с Ё)\n");

            // Основной игровой цикл
            for (int i = 0; i < clues.Count; i++)
            {
                if (!clues[i].IsAnswered)
                {
                    DisplayCurrentProgress(clues, i + 1);

                    Console.WriteLine($"\nВопрос {i + 1}: {clues[i].Question}");
                    Console.Write("Ваш ответ: ");

                    string userAnswer = Console.ReadLine().ToUpper().Trim();

                    // Проверка ответа
                    if (userAnswer == clues[i].Answer)
                    {
                        Console.WriteLine("✅ Правильно!");
                        clues[i] = new CrosswordClue(clues[i].Question, clues[i].Answer) { IsAnswered = true };
                        score++;
                    }
                    else
                    {
                        Console.WriteLine("❌ Неправильно!");
                        Console.WriteLine($"Подсказка: слово состоит из {clues[i].Answer.Length} букв");

                        // Даем вторую попытку
                        Console.Write("Попробуй еще раз: ");
                        userAnswer = Console.ReadLine().ToUpper().Trim();

                        if (userAnswer == clues[i].Answer)
                        {
                            Console.WriteLine("✅ Правильно со второй попытки!");
                            clues[i] = new CrosswordClue(clues[i].Question, clues[i].Answer) { IsAnswered = true };
                            score++;
                        }
                        else
                        {
                            Console.WriteLine($"❌ Неправильно! Правильный ответ: {clues[i].Answer}");
                        }
                    }

                    ShowScore(score, i + 1);
                }
            }

            // Финальный результат
            Console.WriteLine("\n╔══════════════════════════════════════╗");
            Console.WriteLine($"║  ИГРА ОКОНЧЕНА!                      ║");
            Console.WriteLine($"║  Правильных ответов: {score}/{totalQuestions}              ║");
            Console.WriteLine($"║  Процент правильных: {score * 100 / totalQuestions}%                ║");
            Console.WriteLine("╚══════════════════════════════════════╝");

            // Оценка
            if (score == totalQuestions)
                Console.WriteLine("🏆 ПРЕВОСХОДНО! Ты отгадал все слова!");
            else if (score >= totalQuestions * 0.7)
                Console.WriteLine("👍 Хороший результат!");
            else
                Console.WriteLine("👆 В следующий раз получится лучше!");
        }

        static void DisplayCurrentProgress(List<CrosswordClue> clues, int currentQuestion)
        {
            Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine("ТЕКУЩИЙ ПРОГРЕСС:");

            int answered = 0;
            foreach (var clue in clues)
            {
                if (clue.IsAnswered)
                {
                    Console.Write($"✅ {clue.Answer} ");
                    answered++;
                }
            }

            if (answered == 0)
                Console.Write("Пока нет отгаданных слов");

            Console.WriteLine($"\nОтгадано: {answered}/{clues.Count}");
            Console.WriteLine($"Вопрос {currentQuestion}/{clues.Count}");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        }

        static void ShowScore(int score, int total)
        {
            Console.WriteLine($"\nСчет: {score}/{total}");

            // Прогресс бар
            Console.Write("Прогресс: [");
            int progress = score * 20 / total; // 20 символов в прогресс баре
            Console.Write(new string('█', progress));
            Console.Write(new string('░', 20 - progress));
            Console.WriteLine("]");
        }
    }
}