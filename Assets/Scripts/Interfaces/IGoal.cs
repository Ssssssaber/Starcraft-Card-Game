
namespace Interfaces
{
    public interface IGoal
    {
        public bool CanRun();

        public int CalculatePriority();

        public void OnActivated(ActionBase linkedAction);

        public void OnDeactivated();


        public void OnGoalTick();
    
    }
}