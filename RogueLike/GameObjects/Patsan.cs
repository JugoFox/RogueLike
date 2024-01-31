using System.Numerics;

namespace RogueLike.GameObjects
{
    public class Patsan : Unit
    {
        public Patsan(char symbol, Vector2 position , int health, int damage) : base(symbol, position, health, damage)
        {
            Position = position;
            Health = health;
            Damage = damage;
        }
    }
}
