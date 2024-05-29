using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEditor.Build;
using TMPro;
using System;

public class MLAgentTest : Agent
{
    [SerializeField] private PlayerManager player;
    [SerializeField] private TMP_Text episodeCountText;
    [SerializeField] private TMP_Text stepCountText;
    [SerializeField] private int maxStep = 100;
    private int startStepCount = 0;
    private bool gameReset = false;
    private int episodeCount
    {
        get { return _episodeCount; }
        set 
        {
            _episodeCount = value;
            episodeCountText.text = $"Episode: {value}";
        }
    }
    private int _episodeCount = 0;

    private int stepCount
    {
        get { return _stepCount; }
        set 
        {
            _stepCount = value;
            stepCountText.text = $"Step: {value}";
        }
    }
    private int _stepCount = 0;

    private void Awake()
    {
        Academy.Instance.AutomaticSteppingEnabled = false;
        // LazyInitialize();
    }

    private void Start()
    {
        
        EventManager.OnPlayerTurnStart.AddListener(CallForDecision);
        EventManager.OnPlayerTurnStart.AddListener(CollectEpisodeData);
// 
        EventManager.OnGameEnded.AddListener(EndGame);

        EventManager.OnOpponentTurnEnd.AddListener(CallForDecision);  
        EventManager.OnOpponentTurnEnd.AddListener(CollectEpisodeData);
    }

    private void CallForDecision()
    {
        this.RequestDecision();
    }
    
    private void CollectEpisodeData()
    {
        Academy.Instance.EnvironmentStep();
        stepCount = Academy.Instance.StepCount;
        if (stepCount - startStepCount > maxStep)
        {
            EndGame();
        }        
    }

    public override void OnEpisodeBegin()
    {
        episodeCount++;
        startStepCount = stepCount;
        if (gameReset)
        {
            EventManager.GameStarted();
            gameReset = false;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(TurnSystem.instance.GetTurnCount());
        // PlayerInfo
        sensor.AddObservation(BoardAwareness.Instance.GetPlayerHandCards(player));
        sensor.AddObservation(BoardAwareness.Instance.GetPlayerTableCards(player));
        sensor.AddObservation(BoardAwareness.Instance.GetPlayerManaPoints(player));
        // EnemyInfo
        sensor.AddObservation(BoardAwareness.Instance.GetPlayerHandCardsCount(player.OpposingPlayer));
        sensor.AddObservation(BoardAwareness.Instance.GetPlayerTableCards(player));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        GameUtilities.PrintArray(actions.DiscreteActions);
        if (player.ControlType != PlayerControl.ML)
        {
            return;
        }

        for (int i = 0; i < GameUtilities.MAX_HAND_CAPACITY; i++)
        {
            if (actions.DiscreteActions[i] == 0)
            {
                continue;
            }
            if (!player.TryPlayCardById(i))
            {
                AddReward(-1f);
            }
        }

        int cardId = 0;
        for (int i =  GameUtilities.MAX_HAND_CAPACITY; i < actions.DiscreteActions.Length; i++)
        {
            if (!player.TryAttackTarget(cardId, actions.DiscreteActions[i]))
            {
                AddReward(-1f);
            }
            else
            {
                Debug.Log($"card {cardId} attacks {i}");
            }
            cardId++;
        }

        StartCoroutine(SwitchTurnWithPause());
    }

    private IEnumerator SwitchTurnWithPause()
    {
        yield return new WaitForSeconds(5f);
        TurnSystem.instance.SwitchTurn();
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
    }

    private void OnWin()
    {
        AddReward(100f);
    }

    private void OnDefeat()
    {
        AddReward(100f);
    }

    private void EndGame()
    {
        gameReset = true;
        EndEpisode();
    }
}
