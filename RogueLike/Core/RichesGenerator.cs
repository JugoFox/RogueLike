using RogueLike.GameObjects;
using System;
using System.Numerics;

namespace RogueLike.Core
{
    public class RichesGenerator
    {
        private List<PescerkaWay> _pescerkaWay = new List<PescerkaWay>();
        private int _width, _height;
        private int _amountRiches;
        private Random _random;
        private List<Riches> _riches = new List<Riches>();

        public RichesGenerator(List<PescerkaWay> pescerkaWay, int width, int height, int amountRiches, Random random)
        {
            _pescerkaWay = pescerkaWay;
            _width = width;
            _height = height;
            _amountRiches = amountRiches;
            _random = random;
        }

        private void GenerateRiches()
        {
            int countPosition = 0;
            int countSpawn = 0;

            foreach (var pescerkaWay in _pescerkaWay)
                countPosition++;

            if(_amountRiches > countPosition / 20)
            {
                while (countSpawn < countPosition / 20)
                {
                    foreach (var way in _pescerkaWay)
                    {
                        if (way.Position.X == _random.Next(1, _width - 2) && way.Position.Y == _random.Next(1, _height - 2))
                        {
                            _riches.Add(new Riches('.', new Vector2(way.Position.X, way.Position.Y), _random.Next(0, 6), 1));
                            countSpawn++;
                        }
                    }
                }
            }
            else
            {                
                while (countSpawn < _amountRiches)
                {
                    foreach (var way in _pescerkaWay)
                    {
                        if (way.Position.X == _random.Next(1, _width - 2) && way.Position.Y == _random.Next(1, _height - 2))
                        {
                            _riches.Add(new Riches('.', new Vector2(way.Position.X, way.Position.Y), _random.Next(0, 6), 1));
                            countSpawn++;
                        }
                    }
                }
            }

        }

        public List<Riches> GetRiches()
        {
            Drawing draw = new Drawing();

            GenerateRiches();
            draw.PrintRiches(_riches, _width, _height);
            return _riches;
        }
    }
}
