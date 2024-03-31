using System.Collections.Generic;
using DefaultNamespace.State;
using UnityEngine;
using UnityEngine.XR;


public class HandBehaviour : MonoBehaviour
{
    public List<Card> CardsList = new List<Card>();
    public Team Team;
    public HandState State {
        get => _handState;
        set
        {
            _handState = value;
            _handState.Init(this);
        } 
    }

    private HandState _handState;

    private void Start()
    {
        State = new EmptyState();
        if (Team == Team.Opponent)
        {
            EventManager.OnOpponentCardDealt.AddListener(AddCard);
            EventManager.OnOpponentCardPlayed.AddListener(RemoveCard);
        }
        else
        {
            EventManager.OnPlayerCardDealt.AddListener(AddCard);
            EventManager.OnPlayerCardPlayed.AddListener(RemoveCard);
        }
    }

    private void HandleState()
    {
        State.Handle(this);
    }
    
    public void AddCard(Card card)
    {
        HandleState();
        CardsList.Add(card);
        if (Team == Team.Opponent)
        {
            card.IsDraggable = false;
        }
    }

    public void RemoveCard(Card card)
    {
        HandleState();
        CardsList.Remove(card);
    }
    
    public void RefreshDragAbility()
    {
        foreach (var card in CardsList)
        {
            card.IsDraggable = true;
        }
    }
    
    public void DisableDragAbility()
    {
        foreach (var card in CardsList)
        {
            card.IsDraggable = false;
        }
    }

    public int GetCardsCount()
    {
        return CardsList.Count;
    }

}