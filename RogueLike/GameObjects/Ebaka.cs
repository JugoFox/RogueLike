using System.Numerics;

namespace RogueLike.GameObjects
{
    public class Ebaka : Unit
    {
        public Ebaka(char symbol, Vector2 position, int health, int damage, int cooldown) : base(symbol, position, health, damage, cooldown)
        {
            Position = position;
            Health = health;
            Damage = damage;
            Cooldown = cooldown;
        }
    }
}
