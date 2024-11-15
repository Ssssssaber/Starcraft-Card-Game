using System;
using System.Collections.Generic;
using DefaultNamespace;
using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR;


public class TurnSystem : MonoBehaviour
{
    public bool isBottomPlayerTurn;
    public TextMeshProUGUI TurnText;

    public Button endTurnButton;
    public TextMeshProUGUI buttonText;
    private int TurnCount = 0;
    [FormerlySerializedAs("manaText")] public TextMeshProUGUI PlayerManaText;
    public TextMeshProUGUI OpponentManaText;
    public static TurnSystem instance;
    public PlayerManager player;
    public PlayerManager opponent;
    public TurnEffectManager turnEffectManager {get; private set;}
    public void Init()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("You are trying to create another turn system gameobject");
            Destroy(this);
        }
        else
        {
            player = BoardAwareness.Instance.player;
            opponent = BoardAwareness.Instance.opponent;
            EventManager.OnBoardInitialized.AddListener(ResetSystem);
            turnEffectManager = new TurnEffectManager();
            instance = this;
        }
    }

    // private void Start()
    // {
    //     ResetSystem();
    // }

    public void ResetSystem()
    {
        TurnCount = 0;
        isBottomPlayerTurn = true;
        opponent.ManaComponent.RefreshManaOnTurn();
        player.ManaComponent.RefreshManaOnTurn();
        turnEffectManager.ResetTurnEffects();
    }


    public int GetTurnCount()
    {
        return TurnCount;
    }  

    public void UpdateTurnStats()
    {
        
        if (isBottomPlayerTurn)
        {
            TurnCount += 1;
            TurnText.text = $"Your turn {TurnCount}";
            buttonText.text = "End your turn";
        }
        else
        {
            TurnText.text = $"Opponents turn {TurnCount}";
            buttonText.text = "End opponent's turn";
        }
    }


    public void SwitchTurn()
    {
        if (isBottomPlayerTurn)
        {
            EndPlayerTurn();
        }
        else 
        {
            EndOpponentTurn();
        }

        UpdateTurnStats();
    }

    public void EndPlayerTurn()
    {
        isBottomPlayerTurn = false;
        EventManager.PlayerTurnEnded();
        EndTurn(player, turnEffectManager);
    }

    public void EndOpponentTurn()
    {
        isBottomPlayerTurn = true;
        EventManager.OpponentTurnEnded();
        EndTurn(opponent, turnEffectManager);    
    }

    private void EndTurn(PlayerManager player, TurnEffectManager effectsManager)
    {
        if (player.ControlType == PlayerControl.Manual)
        {
            player.Hand.DisableDragAbility();    
        }
        if (player.OpposingPlayer.ControlType == PlayerControl.Manual)
        {
            player.OpposingPlayer.Hand.RefreshDragAbility();
        }

        player.Table.DisableAttackAbility();
        player.OpposingPlayer.Table.RefreshAttackAbility();
        player.OpposingPlayer.ManaComponent.RefreshManaOnTurn();
        effectsManager.ImplementTurnEffects(player.Team);
    }
}
