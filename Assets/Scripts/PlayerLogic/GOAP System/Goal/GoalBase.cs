using System;
using System.Collections.Generic;
using DefaultNamespace;
using Interfaces;
using UnityEngine;

public abstract class GoalBase : MonoBehaviour, IGoal
{
    protected PlayerManager player;
    // protected GOAPUI DebugUI;
    
    protected ActionBase LinkedAction;
    protected ManagerGOAPUI DebugUI;

    private void Awake()
    {
        DebugUI =  GameObject.Find("GoapUI").GetComponent<ManagerGOAPUI>();
        player = GetComponentInParent<PlayerManager>();
        if (!player.isManuallyControlled)
        {
            EventManager.OnEnvironmentChange.AddListener(UpdateGoalStats);
        }
    }

    public abstract bool CanRun();

    public abstract int CalculatePriority();
    
    public virtual void OnActivated(ActionBase linkedAction)
    {
        LinkedAction = linkedAction;
    }
    
    public virtual void OnDeactivated()
    {
        LinkedAction = null;
    }
    
    public virtual void UpdateGoalStats()
    {
         GoalStatus status = GoalStatus.Paused;
        
        if (LinkedAction != null)
        {
            status = GoalStatus.Running;
        }
        DebugUI.UpdateGoal(player, this, GetType().Name, CalculatePriority(), status);
    }
    
    public virtual void OnGoalTick()
    {
        
        
    }
}

public enum GoalStatus
{
    Running, 
    Paused
}