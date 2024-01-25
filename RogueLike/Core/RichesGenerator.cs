using RogueLike.GameObjects;

namespace RogueLike.Core
{
    public class RichesGenerator
    {
        private int[,] _pescerka;
        private char[,] _riches;
        private int _width, _height;

        public RichesGenerator(int[,] pescerka, int width, int height)
        {
            _pescerka = pescerka;
            _width = width;
            _height = height;
            _riches = new char[width, height];
        }

        public void RichesGenerate()
        {
            int countPosition = 0;
            int countSpawn = 0;

            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                    if (_pescerka[x, y] == 0)
                        countPosition++;

            while (countSpawn < countPosition / 20)
            {
                Random rand = new Random();
                int x = rand.Next(1, _width - 2);
                int y = rand.Next(1, _height - 2);

                if (_pescerka[x, y] == 0)
                {
                    _riches[x, y] = '.';
                    countSpawn++;
                }
            }
        }

        public char[,] GetRiches()
        {
            Drawing draw = new Drawing();

            RichesGenerate();
            draw.PrintRiches(_riches, _width, _height);
            return _riches;
        }
    }
}
