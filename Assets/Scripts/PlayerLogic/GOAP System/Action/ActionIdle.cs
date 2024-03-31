using System;
using System.Collections.Generic;
using GOAP_System.Goal;
using UnityEngine;

namespace GOAP_System.Action
{
    public class ActionIdle : ActionBase
    {
        [SerializeField] private List<System.Type> _supportedGoals = new List<System.Type>() {typeof(GoalIdle)};
        [SerializeField] private int _cost = 0;


        public override List<Type> GetSupportedGoals()
        {
            return _supportedGoals;
        }

        public override float GetCost()
        {
            return _cost;
        }
    }
}