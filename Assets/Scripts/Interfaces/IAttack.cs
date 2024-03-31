namespace Interfaces
{
    public interface IAttack
    {
        public int Attack { get; }
        public void IncreaseAttack(int amount);
        public void DecreaseAttack(int amount);
    }
}