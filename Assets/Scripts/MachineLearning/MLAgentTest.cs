using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEditor.Build;

public class MLAgentTest : Agent
{
    [SerializeField] private PlayerManager player;


    private void Start()
    {
        Academy.Instance.AutomaticSteppingEnabled = false;
        EventManager.OnPlayerTurnStart.AddListener(CollectEpisodeData);
        // EventManager.OnPlayerTurnStart.AddListener(CallForDecision);

        EventManager.OnGameEnded.AddListener(EndGame);

        EventManager.OnEnvironmentChange.AddListener(CollectEpisodeData);
        // EventManager.OnOpponentTurnEnd.AddListener(CallForDecision);  
    }

    private void CallForDecision()
    {
        this.RequestDecision();
    }

    public override void OnEpisodeBegin()
    {
        /*
        Set everything to a previous position:
        1. Heroes
        2. Decks
        3. Table
        4. Hand
        5. Turn system: turn count + turn effects

        Start the game again

        */
        base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        /*
        Data to collect:
        Game Info
        - Turn count (0 <= int < 100)
        
        Player Info
        - Array of cards' ids in hand 
            [1, 2, 3, 0, 0, 0, 0] -- length = 7
            0 means that the is no card
        - Array of player table cards
            [1, 2, 3, 0, 0, 0, 0] -- length = 7
            0 means that the is no card
        - Number of Mana points of player (0 <= int <= 10)
        
        Enemy info
        - Number of enemy player cards (0 <= int < 7)
        - Array of enemy table cards 
            [1, 2, 3, 0, 0, 0, 0] -- length = 7
            0 means that the is no card
        
        
        Additional
        - Turn effects array for both players
            ([1, 2, 3, 0, 0, 0, 0]) 
        - (probably: Array of cards' manacost)


        TOTAL COUNT: 24
        */

        Debug.Log("Collecting");
        // GameInfo
        // sensor.
        sensor.AddObservation(TurnSystem.instance.GetTurnCount());
        // PlayerInfo
        sensor.AddObservation(BoardAwareness.Instance.GetPlayerHandCards(player));
        sensor.AddObservation(BoardAwareness.Instance.GetPlayerTableCards(player));
        sensor.AddObservation(BoardAwareness.Instance.GetPlayerManaPoints(player));
        // EnemyInfo
        sensor.AddObservation(BoardAwareness.Instance.GetPlayerHandCardsCount(player.OpposingPlayer));
        sensor.AddObservation(BoardAwareness.Instance.GetPlayerTableCards(player));

        // Debug.Log(sensor.GetObservations());
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        for (int i = 0; i < GameUtilities.MAX_HAND_CAPACITY; i++)
        {
            GameUtilities.PrintArray(actions.DiscreteActions);
            if (actions.DiscreteActions[i] == 1)
            {
                Debug.Log($"card on pos{i} will be played");
            }
            else
            {
                Debug.Log($"card on pos{i} is not played");
            }
            /*
            // Try to play card at that position. If cannot play then set negative reward
            
            
            */
            if (player.CanPlayCardByPositonInHand(i))
            {
                player.PlayCardByPositionInHand(i);
            }
            else 
            {
                AddReward(-1f);
            }
        }

        for (int i = 0; i < GameUtilities.MAX_TABLE_CAPACITY; i++)
        {
            /*
            if (player.CanCardTryAttackTargetById(cardId, targetId))
            {
                player.CardAttactTargetById(cardId, targetId);
            }
            else 
            {
                AddReward(-1f)
            }
            */
        }

        // TurnSystem.instance.SwitchTurn();
        StartCoroutine(SwitchTurnWithPause());
    }


    private IEnumerator SwitchTurnWithPause()
    {
        yield return new WaitForSeconds(1f);
        TurnSystem.instance.SwitchTurn();
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
    }

    private void CollectEpisodeData()
    {
        Academy.Instance.EnvironmentStep();
    }

    private void EndGame()
    {
        EndEpisode();
    }
}
