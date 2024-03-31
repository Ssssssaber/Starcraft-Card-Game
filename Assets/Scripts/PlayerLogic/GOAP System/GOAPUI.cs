using System.Collections.Generic;
using Codice.CM.SEIDInfo;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GOAPUI : MonoBehaviour
{
    [FormerlySerializedAs("GoalPrefab")] [SerializeField] private GameObject goalPrefab;
    [SerializeField] private PlayerManager player;
    private RectTransform goapTransform;
    private float goalHeight;
    private Dictionary<MonoBehaviour, GoalUI> DisplayedGoals = new Dictionary<MonoBehaviour, GoalUI>();
    
    private void Start()
    {
        goapTransform = GetComponent<RectTransform>();
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        goalHeight = grid.cellSize.y + grid.spacing.y + grid.padding.top;
    }
    public void UpdateGoal(MonoBehaviour goal, string name, float priority, GoalStatus status)
    {
        if (!DisplayedGoals.ContainsKey(goal))
        {
            GameObject uiGO = Instantiate(goalPrefab, Vector3.zero, Quaternion.identity);
            uiGO.transform.SetParent(goapTransform);
            DisplayedGoals[goal] = uiGO.GetComponent<GoalUI>();
            goapTransform.offsetMin = new Vector2(goapTransform.offsetMin.x, 1080 - goalHeight * DisplayedGoals.Count);           
        }

        DisplayedGoals[goal].UpdateGoalInfo(name, priority, status);

    }
}
