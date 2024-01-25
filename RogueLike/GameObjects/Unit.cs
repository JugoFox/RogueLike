namespace RogueLike.GameObjects
{
    public class Unit
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Symbol { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public int Cooldown { get; set; }

        public Unit(char symbol, int x, int y, int health, int damage, int cooldown)
        {
            Symbol = symbol;
            X = x;
            Y = y;
            Health = health;
            Damage = damage;
            Cooldown = cooldown;
        }

        public Unit(char symbol, int x, int y, int health, int damage)
        {
            Symbol = symbol;
            X = x;
            Y = y;
            Health = health;
            Damage = damage;
        }

        public void Move(int x, int y)
        {
            X += x;
            Y += y;
        }

        public void TakeDamage(int damageAmount)
        {
            Health -= damageAmount;
        }
    }
}
