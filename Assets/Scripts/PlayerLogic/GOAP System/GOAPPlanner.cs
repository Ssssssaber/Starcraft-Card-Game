using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GOAP_System
{
    public class GOAPPlanner : MonoBehaviour
    {
        private GoalBase[] _goals;
        private ActionBase[] _actions;

        private GoalBase _activeGoal;
        private ActionBase _activeAction;

        [SerializeField] private OpponentBehaviour _opponent;
        
        
        
        private void Awake()
        {
            _goals = GetComponents<GoalBase>();
            _actions = GetComponents<ActionBase>();
        }

        public ActionBase ChooseAction()
        {
            GoalBase bestGoal = null;
            ActionBase bestAction = null;
            
            // choosing the best goal
            foreach (var goal in _goals)
            {
                goal.OnGoalTick();

                if (!goal.CanRun())
                {
                    continue;
                }

                if (bestGoal != null)
                {
                    if (bestGoal.CalculatePriority() < goal.CalculatePriority())
                    {
                        bestGoal = goal;
                    }
                }
                else
                {
                    bestGoal = goal;
                }

                if (_activeGoal != null)
                {
                    _activeGoal.OnDeactivated();
                }
                _activeGoal = bestGoal;
            }
            
            foreach (var action in _actions)
            {
                // Debug.Log(bestGoal.GetType());
                if (action.GetSupportedGoals().Contains(bestGoal.GetType()))
                {
                    if (bestAction != null)
                    {
                        if (bestAction.GetCost() > action.GetCost())
                        {
                            bestAction = action;
                        }
                    }
                    else
                    {
                        bestAction = action;
                    }
                }
            }
            if (_activeAction != null)
            {
                _activeAction.OnDeactivated();
            }

            _activeAction = bestAction;
            
            _activeGoal.OnActivated(_activeAction);
            if (_activeAction != null) _activeAction.OnActivated(_activeGoal);

            return bestAction;
        }
        
       
        
        // public ActionBase PlanActions()
        // {
        //     GoalBase bestGoal = null;
        //     ActionBase bestAction = null;
        //
        //     foreach (var goal in _goals)
        //     {
        //         goal.OnGoalTick();
        //         Debug.Log(goal.GetType() + " with priority of " + goal.CalculatePriority());
        //
        //         if (!goal.CanRun())
        //         {
        //             continue;
        //         }
        //
        //         if (bestGoal != null)
        //         {
        //             if (goal.CalculatePriority() < bestGoal.CalculatePriority())
        //                 continue;
        //         }
        //
        //         ActionBase candidateAction = null;
        //
        //         foreach (var action in _actions)
        //         {
        //             if (!action.GetSupportedGoals().Contains(goal.GetType()))
        //             {
        //                 continue;
        //             }
        //
        //             if (candidateAction == null || action.GetCost() < candidateAction.GetCost())
        //             {
        //                 candidateAction = action;
        //             }
        //         }
        //
        //         if (candidateAction != null)
        //         {
        //             bestGoal = goal;
        //             bestAction = candidateAction;
        //         }
        //     }
        //
        //     if (_activeGoal == null)
        //     {
        //         _activeGoal = bestGoal;
        //         _activeAction = bestAction;
        //         if (_activeGoal != null)
        //             _activeGoal.OnActivated(_activeAction);
        //         if (_activeAction != null)
        //             _activeAction.OnActivated(_activeGoal);            
        //     }
        //     else if (_activeGoal == bestGoal)
        //     {
        //         if (_activeAction != bestAction && bestAction != null)
        //         {
        //             _activeAction.OnDeactivated();
        //             
        //             _activeAction = bestAction;
        //             
        //             _activeAction.OnActivated(_activeGoal);
        //         }
        //     }
        //     else if (_activeGoal != bestGoal)
        //     {
        //         _activeGoal.OnDeactivated();
        //         _activeAction.OnDeactivated();
        //
        //         _activeGoal = bestGoal;
        //         _activeAction = bestAction;
        //
        //         if (_activeGoal != null)
        //         {
        //             _activeGoal.OnActivated(_activeAction);
        //         }
        //
        //         if (_activeAction != null)
        //         {
        //             _activeAction.OnActivated(_activeGoal);
        //         }
        //     }
        //
        //     if (_activeAction != null)
        //     {
        //         _activeAction.OnTick();
        //     }
        //     
        //     Debug.Log("the best goal is " + bestGoal.GetType() + " with priority of " + bestGoal.CalculatePriority());
        //
        //     return bestAction;
        // }
    }
}