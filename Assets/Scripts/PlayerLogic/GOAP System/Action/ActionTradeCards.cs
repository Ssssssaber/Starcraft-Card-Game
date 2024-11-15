
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ActionTradeCards : ActionBase
{
    [SerializeField] private List<System.Type> _supportedGoals = new List<System.Type>() {typeof(GoalControlTable)};
    [SerializeField] private int _cost;
    [SerializeField] private OpponentBehaviour _oppBehaviour;
    
    private void Start()
    {
        _oppBehaviour = player.AI;
    }
    
    public override List<System.Type> GetSupportedGoals()
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
        StartCoroutine(_oppBehaviour.TradeThenGoFace());
    }

    public override bool getIsAggressive()
    {
        return false;
    }
}