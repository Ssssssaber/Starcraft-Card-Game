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
    private List<TurnEffect> _playerTurnEffects = new List<TurnEffect>();
    private List<TurnEffect> _opponentTurnEffects = new List<TurnEffect>();
    private void Awake()
    {
        player = BoardAwareness.Instance.player;
        opponent = BoardAwareness.Instance.opponent;
        if (instance != null && instance != this)
        {
            Debug.Log("You are trying to create another turn system gameobject");
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public int GetTurnCount()
    {
        return TurnCount;
    }

    public void AddOpponentTurnEffect(TurnEffect effect)
    {
        _opponentTurnEffects.Add(effect);
    }

    public void AddPlayerTurnEffect(TurnEffect effect)
    {
        _playerTurnEffects.Add(effect);
    }

    private void RemovePlayerTurnEffect(TurnEffect effect)
    {
        if (effect.Duration <= 0)
            _playerTurnEffects.Remove(effect);
    }
    private void RemoveOpponentTurnEffect(TurnEffect effect)
    {
        if (effect.Duration <= 0)
            _opponentTurnEffects.Remove(effect);
    }

    private void ImplementTurnEffects(ref List<TurnEffect> effects)
    {
        List<TurnEffect> expiredEffects = new List<TurnEffect>();
        foreach (var effect in effects)
        {
            switch (effect.Duration)
            {
                case (1):
                    effect.ImplementMethod();
                    // Debug.Log($"EffectName: {effect.GetDesc()} : {effect.Duration}");
                    expiredEffects.Add(effect);
                    break;
                case (0):
                    Debug.Log($"EffectName: {effect.GetDesc()} : {effect.Duration}");
                    break;
                default:
                    effect.ImplementMethod();
                    break;
            }
        }

        foreach (var effect in expiredEffects)
        {
            effects.Remove(effect);
        }
        // PrintTurnEffectsInfo(effects);
    }

    private void PrintTurnEffectsInfo(List<TurnEffect> effects)
    {
        string dbg = $"EFFECTS: ";
        foreach (var effect in effects)
        {
            dbg += $"{effect.GetDesc()} : {effect.Duration}; ";
        }
        Debug.Log(dbg);
    }
    
    private void Start()
    {
        // endTurnButton.onClick.AddListener(OnClick);
        isBottomPlayerTurn = true;
        // EventManager.OnPlayerTurnEnd.AddListener(EndPlayerTurn);
        // EventManager.OnOpponentTurnEnd.AddListener(EndOpponentTurn);
        opponent.ManaComponent.RefreshManaOnTurn();
        player.ManaComponent.RefreshManaOnTurn();
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
        EndTurn(player, ref _opponentTurnEffects);
    }

    public void EndOpponentTurn()
    {
        isBottomPlayerTurn = true;
        EventManager.OpponentTurnEnded();
        EndTurn(opponent, ref _playerTurnEffects);    
    }

    private void EndTurn(PlayerManager player, ref List<TurnEffect> turnEffects)
    {
        if (player.isManuallyControlled)
        {
            player.Hand.DisableDragAbility();    
        }
        if (player.OpposingPlayer.isManuallyControlled)
        {
            player.OpposingPlayer.Hand.RefreshDragAbility();
        }

        player.Table.DisableAttackAbility();
        player.OpposingPlayer.Table.RefreshAttackAbility();
        player.OpposingPlayer.ManaComponent.RefreshManaOnTurn();
        ImplementTurnEffects(ref turnEffects);
    }
}
