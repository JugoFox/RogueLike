namespace RogueLike.Core
{
    public class PescerkaGenerator
    {
        private int[,] _pescerka;
        private int _width, _height;
        private Random _random;

        public PescerkaGenerator(int width, int height, Random random)
        {
            _width = width;
            _height = height;
            _pescerka = new int[width, height];
            _random = random;
        }

        public void GeneratePescerka()
        {
            InitializePescerka();
            CarvePassagesFrom(1, 1);
            AddExit();
        }

        private void InitializePescerka()
        {
            for (int x = 0; x < _width; x++)
                for (int y = 0; y < _height; y++)
                    _pescerka[x, y] = 1;
        }

        private void CarvePassagesFrom(int currentX, int currentY)
        {
            List<Direction> directions = GetShuffledDirections();

            foreach (var direction in directions)
            {
                int nextX = currentX + direction.X * 2;
                int nextY = currentY + direction.Y * 2;

                if (IsInsidePescerka(nextX, nextY) && _pescerka[nextX, nextY] == 1)
                {
                    _pescerka[currentX + direction.X, currentY + direction.Y] = 0;
                    _pescerka[nextX, nextY] = 0;
                    CarvePassagesFrom(nextX, nextY);
                }
            }
        }

        private List<Direction> GetShuffledDirections()
        {
            List<Direction> directions = new List<Direction>
            {
                new Direction(0, -1),
                new Direction(0, 1),
                new Direction(-1, 0),
                new Direction(1, 0)
            };

            for (int i = 0; i < directions.Count; i++)
            {
                int swapIndex = _random.Next(i, directions.Count);
                (directions[i], directions[swapIndex]) = (directions[swapIndex], directions[i]);
            }

            return directions;
        }

        private void AddExit()
        {
            int exitSide = _random.Next(4);

            switch (exitSide)
            {
                case 0:
                    _pescerka[_width / 2, 0] = 2;
                    _pescerka[_width / 2, 1] = 0;
                    break;
                case 1:
                    _pescerka[_width - 1, _height / 2] = 2;
                    _pescerka[_width - 2, _height / 2] = 0;
                    _pescerka[_width - 3, _height / 2] = 0;
                    break;
                case 2:
                    _pescerka[_width / 2, _height - 1] = 2;
                    _pescerka[_width / 2, _height - 2] = 0;
                    _pescerka[_width / 2, _height - 3] = 0;
                    break;
                case 3:
                    _pescerka[0, _height / 2] = 2;
                    _pescerka[1, _height / 2] = 0;
                    break;
            }
        }

        private bool IsInsidePescerka(int x, int y)
        {
            return x > 0 && x < _width - 1 && y > 0 && y < _height - 1;
        }

        public int[,] GetPescerka()
        {
            Drawing draw = new Drawing();

            GeneratePescerka();
            draw.PrintPescerka(_pescerka, _width, _height);
            return _pescerka;
        }

        private class Direction
        {
            public int X { get; }
            public int Y { get; }

            public Direction(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
