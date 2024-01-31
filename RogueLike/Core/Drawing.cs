using RogueLike.GameObjects;
using System.Drawing;

namespace RogueLike.Core
{
    public class Drawing
    {
        public void PrintPescerka(List<PescerkaWay> way, int width, int height)
        {
            foreach (var Way in way)
            {
                Console.SetCursorPosition((int)Way.Position.X, (int)Way.Position.Y);
                Console.Write(Way.Symbol);
            }

            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
        }

        public void PrintUnit(List<Unit> unit, int width, int height)
        {
            foreach(var Unit in unit)
            {                
                Console.SetCursorPosition((int)Unit.Position.X, (int)Unit.Position.Y);

                if(Unit.Symbol =='Ф')
                    Console.ForegroundColor = ConsoleColor.Cyan;
                else if(Unit.Symbol == 'Т')
                    Console.ForegroundColor = ConsoleColor.Blue;
                else if (Unit.Symbol == 'Д')
                    Console.ForegroundColor = ConsoleColor.Magenta;

                Console.Write(Unit.Symbol);
                Console.ResetColor();
            }
            Console.SetCursorPosition(0, 0);
        }

        public void PrintRiches(List<Riches> riches, int width, int height)
        {
            foreach(var Riches in riches)
            {
                Console.SetCursorPosition((int)Riches.Position.X, (int)Riches.Position.Y);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(Riches.Symbol);
                Console.ResetColor();
            }
            Console.SetCursorPosition(0, 0);
        }

        public void PrintBullet(List<Bullet> bullet, int width, int height)
        {
            foreach (var Bullet in bullet)
            {
                Console.SetCursorPosition((int)Bullet.Position.X, (int)Bullet.Position.Y);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(Bullet.Symbol);
                Console.ResetColor();
            }
            Console.SetCursorPosition(0, 0);
        }

        public void DrawLog(int width, Patsan patsan, List<string> logi, int score)
        {
            Console.SetCursorPosition(width + 6, 3);
            Console.Write("Здоровье: " + patsan.Health);

            Console.SetCursorPosition(width + 6, 5);
            Console.Write("Счет: " + score);

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
                        Console.SetCursorPosition(width + 6, 8 + i);
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
