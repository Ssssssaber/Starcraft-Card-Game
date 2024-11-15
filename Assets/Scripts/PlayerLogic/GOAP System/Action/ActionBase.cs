
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour
{
    protected GoalBase _linkedGoal;
    protected PlayerManager player;
    public abstract List<System.Type> GetSupportedGoals();
    private void Awake()
    {
        player = GetComponentInParent<PlayerManager>();
    }
    public abstract float GetCost();

    public virtual void OnActivated(GoalBase linkedGoal)
    {
        _linkedGoal = linkedGoal;
    }

    public virtual void OnDeactivated()
    {
        _linkedGoal = null;
    }

    public virtual void OnTick()
    {
        
    }

    public virtual bool getIsAggressive()
    {
        return false;
    }
}