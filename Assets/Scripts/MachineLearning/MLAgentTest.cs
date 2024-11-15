using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEditor.Build;
using TMPro;
using System;
using UnityEngine.Animations;

public class MLAgentTest : Agent
{
    [SerializeField] private PlayerManager player;
    [SerializeField] private TMP_Text episodeCountText;
    [SerializeField] private TMP_Text stepCountText;
    [SerializeField] private int maxStep = 100;
    [SerializeField] private TMP_Text reward;
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

    private void Init()
    {
        if (player.ControlType != PlayerControl.ML) return;
        EventManager.OnPlayerTurnStart.AddListener(CallForDecision);
        EventManager.OnPlayerTurnStart.AddListener(CollectEpisodeData);
// 
        // EventManager.OnGameEnded.AddListener(EndGame);

        EventManager.OnOpponentTurnEnd.AddListener(CallForDecision);  
        EventManager.OnOpponentTurnEnd.AddListener(CollectEpisodeData);

        EventManager.OnHeroDied.AddListener(EndGameTeam);
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
		if (player.ControlType != PlayerControl.ML) return;
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
                // Academy.Instance.EnvironmentStep();
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
        // Academy.Instance.EnvironmentStep();

        StartCoroutine(SwitchTurnWithPause());
    }

    private IEnumerator SwitchTurnWithPause()
    {
        yield return new WaitForSeconds(GameUtilities.ACTION_WAIT_TIME);
        TurnSystem.instance.SwitchTurn();
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
    }

    

    private void EndGameTeam(Team winner)
    {
        if (winner == Team.Opponent)
        {
            AddReward(-100f);
            Academy.Instance.EnvironmentStep();
        }
        else
        {
            AddReward(100f);
            Academy.Instance.EnvironmentStep();
        }

        reward.text = GetCumulativeReward().ToString();
    }

    private void EndGame()
    {
        gameReset = true;
        EndEpisode();
    }
}
