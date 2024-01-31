using RogueLike.GameObjects;

namespace RogueLike.Core
{
    public class LevelGenerator
    {
        private int _width, _height;
        private int _minAmountEbaka, _minAmountHoboWithShotgun, _minAmountRiches, _maxAmountEbaka, _maxAmountHoboWithShotgun, _maxAmountRiches;
        private List<PescerkaWay> _pescerkaMap;
        private List<Unit> _units;
        private List<Riches> _richesMap;
        public List<string> _logi = new List<string> { };
        public int _score = 0;
        public int _levelCount = 0;
        Drawing _draw = new Drawing();
        public Patsan _patsan;
        public List<Bullet> _bullets = new List<Bullet> { };

        public LevelGenerator(int width, int height, int minAmountEbaka, int minAmountHoboWithShotgun, int minAmountRiches, int maxAmountEbaka, int maxAmountHoboWithShotgun,int maxAmountRiches)
        {
            _width = width;
            _height = height;
            _minAmountEbaka = minAmountEbaka;
            _minAmountHoboWithShotgun = minAmountHoboWithShotgun;
            _minAmountRiches = minAmountRiches;
            _maxAmountEbaka= maxAmountEbaka;
            _maxAmountHoboWithShotgun= maxAmountHoboWithShotgun;
            _maxAmountRiches= maxAmountRiches;
        }

        public void GenerateWorld()
        {
            Random random = new Random();
            PescerkaGenerator pescerkaGenerator = new PescerkaGenerator(_width, _height, random);
            _pescerkaMap = pescerkaGenerator.GetPescerka();

            RichesGenerator richesGenerator = new RichesGenerator(_pescerkaMap, _width, _height, random.Next(_minAmountRiches,_maxAmountRiches + 1), random);
            _richesMap = richesGenerator.GetRiches();

            UnitGenerator unitGenerator = new UnitGenerator(_pescerkaMap, _width, _height, random.Next(_minAmountEbaka, _maxAmountEbaka + 1), random.Next(_minAmountHoboWithShotgun, _maxAmountHoboWithShotgun + 1), random);
            _units = unitGenerator.GetUnit();

            foreach (var units in _units)
                if (units.Symbol == 'Ф')
                    _patsan = (Patsan)units;

            _levelCount++;
            _logi.Add("Пацан попал на " + _levelCount + " этаж подземелья");

            _draw.DrawLog(_width, _patsan, _logi, _score);
        }

        public void RunGame()
        {
            Console.SetWindowSize(_width + 110, Console.WindowHeight);

            bool isRunning = true;
            bool isWinFloor = true;

            while (isRunning)
            {
                if(isWinFloor)
                {
                    GenerateWorld();
                    isWinFloor = false;
                }
                else
                {
                    LevelUpdate levelUpdate = new LevelUpdate(_width, _height, _pescerkaMap, _units, _patsan, _richesMap, _logi, _score, _bullets);
                    levelUpdate.Update();
                    _score = levelUpdate.GetScore();

                    isRunning = levelUpdate.GetIsRunningGame();
                    isWinFloor = levelUpdate.GetIsWinFloor();
                }                
            }
        }


        public void StartScreen()
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
        }
    }
}
