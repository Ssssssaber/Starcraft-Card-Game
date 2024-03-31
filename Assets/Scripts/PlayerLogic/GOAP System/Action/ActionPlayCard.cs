using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP_System.Action
{
    public class ActionPlayCard : ActionBase
    {
        private List<System.Type> _supportedGoals = new List<System.Type>() {typeof(GoalGoFace)};
        // private List
        [SerializeField] private int _cost = 10;
        [SerializeField] private OpponentBehaviour _oppBehaviour;
        public override List<Type> GetSupportedGoals()
        {
            throw new NotImplementedException();
        }

        public override float GetCost()
        {
            throw new NotImplementedException();
        }
    }
}