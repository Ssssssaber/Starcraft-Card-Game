namespace Interfaces
{
    public interface IHealth
    {
        public int Health { get; }
        public void Damage(int amount);
        public void Heal(int amount);
        public void IncreaseHealth(int amount);
        public void DecreaseHealth(int amount);
    }
}