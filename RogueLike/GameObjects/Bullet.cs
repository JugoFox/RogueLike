namespace RogueLike.GameObjects
{
    public class Bullet : Unit
    {
        public Bullet(char symbol, int x, int y, int health, int damage) : base(symbol, x, y, health, damage)
        {
            X = x;
            Y = y;
            Health = health;
            Damage = damage;

        }
    }
}
