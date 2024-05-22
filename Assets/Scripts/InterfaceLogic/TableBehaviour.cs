﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;


public class TableBehaviour : MonoBehaviour
{
    private List<CreatureCard> _cardsList = new List<CreatureCard>();
    public Team Team;
    
    public readonly int maxCardsOnTable = GameUtilities.MAX_TABLE_CAPACITY;
    

    private void SetupCardsList()
    {
        foreach (Transform child in transform)
        {
            CreatureCard card = child.gameObject.GetComponent<CreatureCard>();

            if (card != null)
            {
                _cardsList.Add(card);
                card.State = CardState.OnTable;
            }
        }
    }
    
    private void Start()
    {
        SetupCardsList();
        EventManager.OnCardRemovedFromTable.AddListener(RemoveCard);
        if (Team == Team.Opponent)
        {
            EventManager.OnOpponentCardPlayed.AddListener(AddCard);
        }
        else
        {
            EventManager.OnPlayerCardPlayed.AddListener(AddCard);
            // EventManager.AddPlayerCardPlayed(AddCard);
        }
    }

    public bool IsEmpty()
    {
        return _cardsList.Count == 0;
    }

    public List<CreatureCard> GetCardsList()
    {
        return _cardsList;
    }

    public int GetCount()
    {
        return _cardsList.Count;
    }

    public void AddCard(Card card)
    {
        CreatureCard creature = card.GetComponent<CreatureCard>();
        
        if (creature != null)
        {
            creature.CardEffects.ImplementOnPlayEffect();
            creature.CanAttack = false;
            _cardsList.Add(creature);
            creature.State = CardState.OnTable;
        }
    }
    public void RemoveCard(Card card)
    {
        CreatureCard creature = card.GetComponent<CreatureCard>();

        if (creature != null && _cardsList.Contains(creature))
        {
            _cardsList.Remove(creature);
        }
    }
    
    public void RefreshAttackAbility()
    {
        foreach (var card in _cardsList)
        {
            card.CanAttack = true;
        }
    }
    
    public void DisableAttackAbility()
    {
        foreach (var card in _cardsList)
        {
            card.CanAttack = false;
        }
    }
}