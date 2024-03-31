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
    public static int TurnCount = 0;
    [FormerlySerializedAs("manaText")] public TextMeshProUGUI PlayerManaText;
    public TextMeshProUGUI OpponentManaText;
    public static TurnSystem instance;
    public PlayerManager player;
    public PlayerManager opponent;
    private List<TurnEffect> _playerTurnEffects = new List<TurnEffect>();
    private List<TurnEffect> _opponentTurnEffects = new List<TurnEffect>();
    private void Awake()
    {
        player = BoardAwareness.awareness.player;
        opponent = BoardAwareness.awareness.opponent;
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
        // Debug.Log(_playerTurnEffects);
        List<TurnEffect> expiredEffects = new List<TurnEffect>();
        // PrintTurnEffectsInfo(effects);
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
        endTurnButton.onClick.AddListener(OnClick);
        isBottomPlayerTurn = true;
        EventManager.OnPlayerTurnEnd.AddListener(EndPlayerTurn);
        EventManager.OnOpponentTurnEnd.AddListener(EndOpponentTurn);
        opponent.ManaComponent.RefreshManaOnTurn();
        player.ManaComponent.RefreshManaOnTurn();
    }

    public void UpdateTurnStats()
    {
        if (isBottomPlayerTurn)
        {
            TurnText.text = "Your turn";
            buttonText.text = "End your turn";
        }
        else
        {
            TurnText.text = "Opponent's turn";
            buttonText.text = "End opponent's turn";
        }

        TurnCount += 1;
    }

    public void OnClick()
    {
        if (isBottomPlayerTurn)
        {
            EventManager.PlayerTurnEnded();
        }

        UpdateTurnStats();
    }

    public void EndPlayerTurn()
    {
        isBottomPlayerTurn = false;
        EndTurn(player, ref _opponentTurnEffects);
    }

    public void EndOpponentTurn()
    {
        isBottomPlayerTurn = true;
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
