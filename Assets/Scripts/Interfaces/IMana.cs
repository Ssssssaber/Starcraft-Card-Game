namespace Interfaces
{
    public interface IMana
    {
        public int Mana { get; }
        public void IncreaseManaCost(int amount);
        public void DecreaseManaCost(int amount);
    }
}