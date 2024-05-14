

namespace GOAP_System.Goal
{
    public class GoalStayAlive : GoalBase
    {
        private int _priority = 0;
        private BoardAwareness _awareness;

        public override bool CanRun()
        {
            return true;
        }

        public override int CalculatePriority()
        {
            // if (_awareness.CalculatePlayerCardDamage() >= _awareness.OpponentHeroHealth())
            // {
            //     _priority = 100;
            // }
            // return _awareness.CalculatePlayerCardDamage();
            return 1;
        }
    }
}