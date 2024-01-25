namespace RogueLike.Core
{
    public class UnitGenerator
    {
        private int[,] _pescerka;
        private char[,] _unit;
        private int _width, _height;

        public UnitGenerator(int[,] pescerka, int width, int height)
        {
            _pescerka = pescerka;
            _width = width;
            _height = height;
            _unit = new char[width, height];
        }

        public void GenerateUnit()
        {
            PatsanGenerate();
            EbakaGenerate();
            HoboWithShotgunGenerate();

            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                    if (_pescerka[x, y] == 1 || _pescerka[x, y] == 0)
                        _unit[x, y] = ' ';
                    else if (_pescerka[x, y] == 2)
                        _unit[x, y] = ' ';
                    else if (_pescerka[x, y] == 8)
                        _unit[x, y] = 'Ф';
                    else if (_pescerka[x, y] == 4)
                        _unit[x, y] = 'Т';
                    else if (_pescerka[x, y] == 5)
                        _unit[x, y] = 'Д';
        }

        public void PatsanGenerate()
        {
            bool isSetPatsan = true;

            while (isSetPatsan)
            {
                Random rand = new Random();
                int x = rand.Next(1, _width - 2);
                int y = rand.Next(1, _height - 2);

                if (_pescerka[x, y] == 0)
                {
                    _pescerka[x, y] = 8;
                    isSetPatsan = false;
                }
            }
        }

        public void EbakaGenerate()
        {
            int countPosition = 0;
            int countSpawn = 0;

            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                    if (_pescerka[x, y] == 0)
                        countPosition++;

            while (countSpawn < countPosition / 30)
            {
                Random rand = new Random();
                int x = rand.Next(1, _width - 2);
                int y = rand.Next(1, _height - 2);

                if (_pescerka[x, y] == 0)
                {
                    _pescerka[x, y] = 4;
                    countSpawn++;
                }
            }
        }

        public void HoboWithShotgunGenerate()
        {
            int countPosition = 0;
            int countSpawn = 0;

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_pescerka[x, y] == 0)
                        countPosition++;
                }
            }

            while (countSpawn < countPosition / 30)
            {
                Random rand = new Random();
                int x = rand.Next(1, _width - 2);
                int y = rand.Next(1, _height - 2);

                if (_pescerka[x, y] == 0)
                {
                    _pescerka[x, y] = 5;
                    countSpawn++;
                }
            }
        }

        public char[,] GetUnit()
        {
            Drawing draw = new Drawing();

            GenerateUnit();
            draw.PrintUnit(_unit, _width, _height);
            return _unit;
        }

    }
}
