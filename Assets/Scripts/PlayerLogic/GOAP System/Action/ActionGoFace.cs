
using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ActionGoFace : ActionBase
{
    private List<System.Type> _supportedGoals = new List<System.Type>() {typeof(GoalGoFace)};
    [SerializeField] private int _cost = 10;
    [SerializeField] private OpponentBehaviour _oppBehaviour;

    private void Start()
    {
        _oppBehaviour = player.AI;
    }
    
    public override List<Type> GetSupportedGoals()
    {
        return _supportedGoals;
    }

    public override float GetCost()
    {
        return _cost;
    }

    public override void OnActivated(GoalBase linkedGoal)
    {
        base.OnActivated(linkedGoal);
        
    }

    public override void OnTick()
    {
        StartCoroutine(_oppBehaviour.GoFace());
    }
}