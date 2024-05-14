using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GoalUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Slider _priority;
    [FormerlySerializedAs("Status")] [SerializeField] private TextMeshProUGUI _fraction;
    private int _maxPrio = 50;
    private void Start()
    {
        if (_priority != null)
        {
            _maxPrio = (int) _priority.maxValue;
        }
    }

    public void UpdateGoalInfo(string name, float priority, GoalStatus status)
    {
        _name.text = name;
        _name.color = status == GoalStatus.Paused ? Color.white : Color.cyan;
        _fraction.text = $"{priority}/{_maxPrio}";
        _priority.value = priority;
    } }
