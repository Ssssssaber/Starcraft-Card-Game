using System.Collections.Generic;
using DefaultNamespace.State;
using UnityEngine;
using UnityEngine.XR;


public class HandBehaviour : MonoBehaviour
{
    public List<Card> _cardsList = new List<Card>();
    public Team Team;
    public HandState State {
        get => _handState;
        set
        {
            _handState = value;
            _handState.Init(this);
        } 
    }

    public List<Card> GetCardsList()
    {
        return _cardsList;
    }

    public int GetCount()
    {
        return _cardsList.Count;
    }

    private HandState _handState;

    private void Start()
    {
        State = new EmptyState();
        EventManager.OnBoardInitialized.AddListener(ResetHand);
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

    private void ResetHand()
    {
        foreach (var card in _cardsList)
        {
            Destroy(card.gameObject);
        }
        _cardsList = new List<Card>();
    }

    private void HandleState()
    {
        State.Handle(this);
    }
    
    public void AddCard(Card card)
    {
        HandleState();
        _cardsList.Add(card);
        if (Team == Team.Opponent)
        {
            card.IsDraggable = false;
        }
    }

    public void RemoveCard(Card card)
    {
        HandleState();
        _cardsList.Remove(card);
    }
    
    public void RefreshDragAbility()
    {
        foreach (var card in _cardsList)
        {
            card.IsDraggable = true;
        }
    }
    
    public void DisableDragAbility()
    {
        foreach (var card in _cardsList)
        {
            card.IsDraggable = false;
        }
    }

    public int GetCardsCount()
    {
        return _cardsList.Count;
    }

}