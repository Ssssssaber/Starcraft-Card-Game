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
        if (GameObject.Find("GoapUI") != null)
        {
            DebugUI =  GameObject.Find("GoapUI").GetComponent<ManagerGOAPUI>();
        }
        
    }

	public void Init()
	{
		player = GetComponentInParent<PlayerManager>();
        if (player.ControlType == PlayerControl.GOAP)
        {
            EventManager.OnEnvironmentChange.AddListener(UpdateGoalStats);
        }
		else
		{
			gameObject.SetActive(false);
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
        if (DebugUI != null)
        {
            DebugUI.UpdateGoal(player, this, GetType().Name, CalculatePriority(), status);
        }
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