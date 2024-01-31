using RogueLike.GameObjects;

namespace RogueLike.Core
{
    public class Drawing
    {
        public void PrintUnit(char[,] unit, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (unit[x, y] == 'Ф')
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(unit[x, y]);
                    }
                    else if (unit[x, y] == 'Т')
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(unit[x, y]);
                    }
                    else if (unit[x, y] == 'Д')
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(unit[x, y]);
                    }
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, 0);
        }

        public void PrintRiches(char[,] riches, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (riches[x, y] == '.')
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(riches[x, y]);
                    }
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, 0);
        }


        public void PrintPescerka(int[,] pescerka, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (pescerka[x, y] == 1)
                        Console.Write('#');
                    else if (pescerka[x, y] == 0)
                        Console.Write(' ');
                    else if (pescerka[x, y] == 2)
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
        }


        public void DrawLog(int width, int height, Patsan patsan, List<string> logi, List<int> score)
        {
            int _score = 0;

            foreach (int Score in score)
                _score += Score;

            Console.SetCursorPosition(width + 6, 3);
            Console.Write("Здоровье: " + patsan.Health);

            Console.SetCursorPosition(width + 6, 5);
            Console.Write("Счет: " + _score);

            Console.SetCursorPosition(width + 6, 7);
            Console.Write("Сообщения:");

            var reversLogi = new List<string>();
            reversLogi = logi.ToList();

            reversLogi.Reverse();
            if (reversLogi.Count != 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.SetCursorPosition(width + 6, 8 + i);
                    for (int j = 0; j < 100; j++)
                        Console.Write(' ');
                }

                if (reversLogi.Count < 10)
                {
                    for (int i = 0; i < reversLogi.Count; i++)
                    {
                        Console.SetCursorPosition(width + 7, 8 + i);
                        Console.Write(reversLogi[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Console.SetCursorPosition(width + 6, 8 + i);
                        Console.Write(reversLogi[i]);
                    }
                }
            }
        }
    }
}
