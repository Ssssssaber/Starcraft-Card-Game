using System;
using System.Collections.Generic;
using GOAP_System.Action;
using UnityEngine;

namespace GOAP_System.Goal
{
    public class GoalIdle : GoalBase
    {
        [SerializeField] private int Priority = 5;
        

        public override bool CanRun()
        {
            return true;
        }

        public override int CalculatePriority()
        {
            return Priority;
        }
    }
}