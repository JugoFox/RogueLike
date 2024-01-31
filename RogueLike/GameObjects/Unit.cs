using System.Numerics;

namespace RogueLike.GameObjects
{
    public class Unit
    {
        public Vector2 Position { get; set; }
        public char Symbol { get; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public int Cooldown { get; set; }

        private enum direction
        {
            Up = 0,
            Left = 1,
            Down = 2,              
            Right = 3
        }       

        public Unit(char symbol, Vector2 position , int health, int damage)
        {
            Symbol = symbol;
            Position = position;
            Health = health;
            Damage = damage;
        }

        public Unit(char symbol, Vector2 position, int health, int damage, int cooldown)
        {
            Symbol = symbol;
            Position = position;
            Health = health;
            Damage = damage;
            Cooldown = cooldown;
        }

        public void Move(int x, int y)
        {
           Position = new Vector2(Position.X + x, Position.Y + y);
        }

        public void TakeDamage(int damageAmount)
        {
            Health -= damageAmount;
        }
    }
}
