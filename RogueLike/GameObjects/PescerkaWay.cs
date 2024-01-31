using System.Numerics;

namespace RogueLike.GameObjects
{
    public class PescerkaWay
    {
        public Vector2 Position { get; set; }
        public int TypeWay { get; set; }
        public char Symbol { get; set; }

        public PescerkaWay(char symbol, Vector2 pos, int type)
        {
            Symbol = symbol;
            Position = pos;
            TypeWay = type;
        }
    }
}
