using RogueLike.GameObjects;

namespace RogueLike.Core
{
    public class LevelGenerator
    {
        private int[,] PescerkaMap;
        private char[,] UnitMap;
        private char[,] RichesMap;
        public int Width = 40;
        public int Height = 20;
        public List<Unit> Enemies = new List<Unit> { };
        public Patsan patsan;
        Drawing draw = new Drawing();
        public List<Bullet> _bullets = new List<Bullet> { };
        public List<string> logi = new List<string> { };
        public List<Riches> _riches = new List<Riches> { };
        public List<int> Score = new List<int> { 0 };
        Random random = new Random();

        public void GenerateWorld()
        {
            string gameDescription = "Игра:\n" +
                                     "В подземелье я пойду кучалу лута там найду\n\n" +
                                     "Управление:\n" +
                                     "W - перемещение вверх          UpArrow - удар вверх\n" +
                                     "A - перемещение влево          LeftArrow - удар влево\n" +
                                     "S - перемещение вниз           DownArrow -  удар вниз\n" +
                                     "D - перемещение вправо         RightArrow - удар в право\n\n" +
                                     "Esc - выход из игры\n\n" +
                                     "Игровые обозначения:\n" +
                                     "Ф - Игрок. Деловой пацан, ходит все время уперев руки в бока\n" +
                                     "Т - Враг. Т-позер, видимо кто-то забыл добавить ему анимацию\n" +
                                     "Д - Враг. Далек представитель могущественной расой, нацеленной на завоевание Вселенной\n" +
                                     ". - Сокровища. Разбросанные по подземелью безделушки\n\n" +
                                     "Цель:\n" +
                                     "Выбраться из подземелья набрав наибольшее колличество очков\n\n" +
                                     "P.S. Игра содержит баги";

            Console.Write(gameDescription);
            Console.ReadKey();
            Console.Clear();

            Random random = new Random();
            PescerkaGenerator pescerkaGenerator = new PescerkaGenerator(Width, Height, random);
            PescerkaMap = pescerkaGenerator.GetPescerka();

            UnitGenerator unitGenerator = new UnitGenerator(PescerkaMap, Width, Height);
            UnitMap = unitGenerator.GetUnit();

            RichesGenerator richesGenerator = new RichesGenerator(PescerkaMap, Width, Height);
            RichesMap = richesGenerator.GetRiches();

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (UnitMap[x, y] == 'Ф')
                        patsan = new Patsan('Ф', x, y, 150, 10);
                    else if (UnitMap[x, y] == 'Т')
                    {
                        Ebaka ebaka = new Ebaka('Т', x, y, 10, 10, 2);
                        Enemies.Add(ebaka);
                    }
                    else if (UnitMap[x, y] == 'Д')
                    {
                        HoboWithShotgun hoboWithShotgun = new HoboWithShotgun('Д', x, y, 10, 5);
                        Enemies.Add(hoboWithShotgun);
                    }

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (RichesMap[x, y] == '.')
                    {
                        Riches riches = new Riches(x, y, random.Next(0, 6), 1);
                        _riches.Add(riches);
                    }

            draw.DrawLog(Width, Height, patsan, logi, Score);
        }

        public void RunGame()
        {
            Console.SetWindowSize(150, Console.WindowHeight);

            bool isRunning = true;

            while (isRunning)
            {
                LevelUpdate levelUpdate = new LevelUpdate(patsan, Enemies, UnitMap, PescerkaMap, logi, _bullets, RichesMap, _riches, Score);
                levelUpdate.Update();

                isRunning = levelUpdate.GetIsRunningGame();
            }
        }
    }
}
