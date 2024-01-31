using System.Numerics;

namespace RogueLike.GameObjects
{
    public class HoboWithShotgun : Unit
    {
        public HoboWithShotgun(char symbol, Vector2 position, int health, int damage, int cooldown) : base(symbol, position, health, damage, cooldown)
        {
            Position = position;
            Health = health;
            Damage = damage;
            Cooldown = cooldown;
        }        
    }
}
