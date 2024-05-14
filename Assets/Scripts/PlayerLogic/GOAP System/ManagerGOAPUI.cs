
using UnityEngine;

public class ManagerGOAPUI : MonoBehaviour
{
    [SerializeField] private GOAPUI playerGOAP;
    [SerializeField] private GOAPUI opponentGOAP;
    
    public void UpdateGoal(PlayerManager player, MonoBehaviour goal, string name, float priority, GoalStatus status)
    {
        if (player.Team == Team.Player)
        {
            playerGOAP.UpdateGoal(goal, name, priority, status);
        }
        else 
        {
            opponentGOAP.UpdateGoal(goal, name, priority, status);
        }
    }
    private void ShowGOAP(string message)
    {
        if (message == "player")
            playerGOAP.gameObject.SetActive(true);
        else 
            opponentGOAP.gameObject.SetActive(true);
    }
}
