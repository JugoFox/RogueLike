using RogueLike.GameObjects;
using System.Numerics;

namespace RogueLike.Core
{
    public class UnitGenerator
    {
        private List<PescerkaWay> _way = new List<PescerkaWay>();
        private int _width, _height;
        private int _amountEbaka, _amountHoboWithShotgun;
        private Random _random;
        private Patsan _patsan;
        private List<Unit> _units = new List<Unit>();

        public UnitGenerator(List<PescerkaWay> way, int width, int height, int amountEbaka, int amountHoboWithShotgun, Random random)
        {
            _way = way;
            _width = width;
            _height = height;
            _random = random;
            _amountEbaka = amountEbaka;
            _amountHoboWithShotgun = amountHoboWithShotgun;
        }

        public void GenerateUnit()
        {
            PatsanGenerate();
            EbakaGenerate();
            HoboWithShotgunGenerate();
        }

        public void PatsanGenerate()
        {
            bool isSetPatsan = true;

            while (isSetPatsan)
            {
                foreach (var way in _way)
                {
                    if (way.Position.X == _random.Next(1, _width - 2) && way.Position.Y == _random.Next(1, _height - 2))
                    {
                        _patsan = new Patsan('Ф', new Vector2(way.Position.X, way.Position.Y), 150, 10);
                        _units.Add(_patsan);
                        isSetPatsan = false;
                        break;
                    }
                }
            }
        }

        public void EbakaGenerate()
        {
            int countPosition = 0;
            int countSpawn = 0;

            foreach (var way in _way)
                countPosition++;

            if (_amountEbaka > countPosition / 30)
            {
                while (countSpawn < countPosition / 30)
                {
                    foreach (var way in _way)
                    {
                        if (way.Position.X == _random.Next(1, _width - 2) && way.Position.Y == _random.Next(1, _height - 2))
                        {
                            _units.Add(new Ebaka('Т', new Vector2(way.Position.X, way.Position.Y), 10, 10, 2));
                            countSpawn++;
                        }
                    }
                }
            }
            else
            {
                while (countSpawn < _amountEbaka)
                {
                    foreach (var way in _way)
                    {
                        if (way.Position.X == _random.Next(1, _width - 2) && way.Position.Y == _random.Next(1, _height - 2))
                        {
                            _units.Add(new Ebaka('Т', new Vector2(way.Position.X, way.Position.Y), 10, 10, 2));
                            countSpawn++;
                        }
                    }
                }
            }
        }

        public void HoboWithShotgunGenerate()
        {
            int countPosition = 0;
            int countSpawn = 0;

            foreach (var way in _way)
                countPosition++;

            if (_amountHoboWithShotgun > countPosition / 30)
            {
                while (countSpawn < countPosition / 30)
                {
                    foreach (var way in _way)
                    {
                        if (way.Position.X == _random.Next(1, _width - 2) && way.Position.Y == _random.Next(1, _height - 2))
                        {
                            _units.Add(new HoboWithShotgun('Д', new Vector2(way.Position.X, way.Position.Y), 10, 10, 5));
                            countSpawn++;
                        }
                    }
                }
            }
            else
            {
                while (countSpawn < _amountHoboWithShotgun)
                {
                    foreach (var way in _way)
                    {
                        if (way.Position.X == _random.Next(1, _width - 2) && way.Position.Y == _random.Next(1, _height - 2))
                        {
                            _units.Add(new HoboWithShotgun('Д', new Vector2(way.Position.X, way.Position.Y), 10, 10, 6));
                            countSpawn++;
                        }
                    }
                }
            }
        }

        public List<Unit> GetUnit()
        {
            Drawing draw = new Drawing();

            GenerateUnit();
            draw.PrintUnit(_units, _width, _height);
            return _units;
        }
    }
}
