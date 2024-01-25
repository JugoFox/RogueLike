namespace RogueLike.GameObjects
{
    public class Ebaka : Unit
    {        
        public Ebaka(char symbol, int x, int y, int health, int damage, int cooldown) : base(symbol, x, y, health, damage, cooldown)
        {
            X = x;
            Y = y;
            Health = health;
            Damage = damage;
            Cooldown = cooldown;
        }
    }
}
