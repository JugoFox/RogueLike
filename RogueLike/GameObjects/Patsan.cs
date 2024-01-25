namespace RogueLike.GameObjects
{
    public class Patsan : Unit
    {
        public Patsan(char symbol, int x, int y, int health, int damage) : base(symbol, x, y, health, damage)
        {
            X = x;
            Y = y;
            Health = health;
            Damage = damage;
        }
    }
}
