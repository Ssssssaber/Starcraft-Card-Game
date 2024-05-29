using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Menu;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerManager : MonoBehaviour
{   
    [Header("Pre game stats")]
    public Team Team;
    public Race Race;
    public PlayerControl ControlType;
    public HeroStats HeroStats;

    [Header("In game components")]
    public HeroBehaviour Hero;
    public PlayerManaComponent ManaComponent;
    public TableBehaviour Table;
    public HandBehaviour Hand;
    public DeckBehaviour Deck;
    public OpponentBehaviour AI;
    public PlayerManager OpposingPlayer;

    // public bool isManuallyControlled = true;
    private void Awake()
    {
        AI = GetComponentInChildren<OpponentBehaviour>();
        // if (AI == null)
        // {
        //     isManuallyControlled = false;
        // }
        // else
        // {
        //     isManuallyControlled = true;
        // }

        Hero = GetComponentInChildren<HeroBehaviour>();
        ManaComponent = GetComponentInChildren<PlayerManaComponent>();
        Table = GetComponentInChildren<TableBehaviour>();
        Hand = GetComponentInChildren<HandBehaviour>();
        Deck = GetComponentInChildren<DeckBehaviour>();
    }

    private void Start()
    {
        EventManager.OnDatabaseCreated.AddListener(SetupHero);
    }


    public void SetupHero()
    {
        Race = HeroStats.Race;
    }
    public int TableCardsCount()
    {
        return Table.GetCount();
    }
    

    public int CalculateCardDamage()
    {
        int damage = 0;

        foreach (var card in Table.GetCardsList())
        {
            damage += card.AttackComponent.Attack;
        }

        return damage;
    }

    public int HeroHealth()
    {
        return Hero.HealthComponent.Health;
    }
    
    public int CurrentMana()
    {
        return ManaComponent.currentMana;
    }

    public List<float> GetTableCardIds()
    {   
        
        float[] cardIds = new float[GameUtilities.MAX_TABLE_CAPACITY];
        List<CreatureCard> cards = Table.GetCardsList();
        int i = 0;
        try {
            for (i = 0; i < cards.Count; i++)
            {                
                cardIds[i] = cards[i].ID;
            }
        }
        catch (Exception e) 
        {
            throw new Exception($"bezdarge: {e.Message}");
        }  
        
        return cardIds.ToList();
    }

    public List<float> GetHandCardIds()
    {
        float[] cardIds = new float[GameUtilities.MAX_HAND_CAPACITY];
        List<Card> cards = Hand.GetCardsList();
        try 
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cardIds[i] = cards[i].ID;
            }
        }
        catch (Exception e) 
        {
            throw new Exception($"bezdarge: {e.Message}");
        } 

        return cardIds.ToList();
    }

    public int GetTableCardsCount()
    {
        return Table.GetCardsList().Count;
    }

    public int GetHandCardsCount()
    {
        return Hand.GetCardsList().Count;
    }

    public int GetManaPoints()
    {
        return ManaComponent.currentMana;
    }

    private bool TryGetTableCardByPosition(int cardPos, out CreatureCard card)
    {
        if (cardPos < 0 || cardPos >= Table.GetCount())
        {
            card = null;
            return false;
        }
        else
        {
            card = Table.GetCardsList()[cardPos];
            return true;
        }
    }

    private bool TryGetHandCardByPosition(int cardPos, out Card card)
    {
        if (cardPos < 0 || cardPos >= Hand.GetCount())
        {
            card = null;
            return false;
        }
        else
        {
            card = Hand.GetCardsList()[cardPos];
            return true;
        }
    }

    // public bool CanPlayCardByPositonInHand(int cardPosition)
    // {
    //     if (TryGetHandCardByPosition(cardPosition, out Card card))
    //     {
    //         return 
    //         (card.ManaComponent.Mana < ManaComponent.currentMana) &&
    //         (Table.GetCount() < GameUtilities.MAX_TABLE_CAPACITY)&&
    //         (cardPosition < Hand.GetCount());
    //     }
    //     else 
    //     {
    //         return false;
    //     }
    // }

    // public void PlayCardByPositionInHand(int cardPosition)
    // {
    //     Card card = Hand.GetCardsList()[cardPosition];
    //     if (card.Type == CardType.Creature)
    //     {
    //         card.transform.SetParent(Table.transform, false);
    //         CreatureCard temp = card.GetComponent<CreatureCard>();
    //         temp.CanAttack = false;
    //     }
    //     card.SetGoapGOVisible(false);
    //     card.IsDraggable = false;
    //     card.State = CardState.OnTable;
    //     card.FaceCardDown(false);
    //     if (Team == Team.Player)
    //     {
    //         EventManager.PlayerCardPlayed(card);
    //     }
    //     else
    //     {
    //         EventManager.OpponentCardPlayed(card);
    //     }

    //     card.CardPlayed?.Invoke();
    //     Hand.RemoveCard(card);
    // }

    public bool TryPlayCardById(int cardPosition)
    {
        if (TryGetHandCardByPosition(cardPosition, out Card card))
        {
            bool isValid = (card.ManaComponent.Mana < ManaComponent.currentMana) &&
            (Table.GetCount() < GameUtilities.MAX_TABLE_CAPACITY)&&
            (cardPosition < Hand.GetCount());
            
            if (isValid)
            {
                PlayCard(card);
            }

            return isValid;
        }
        else 
        {
            return false;
        }
    }

    private void PlayCard(Card card)
    {
        if (card.Type == CardType.Creature)
        {
            card.transform.SetParent(Table.transform, false);
            CreatureCard temp = card.GetComponent<CreatureCard>();
            temp.CanAttack = false;
        }
        card.SetGoapGOVisible(false);
        card.IsDraggable = false;
        card.State = CardState.OnTable;
        card.FaceCardDown(false);
        if (Team == Team.Player)
        {
            EventManager.PlayerCardPlayed(card);
        }
        else
        {
            EventManager.OpponentCardPlayed(card);
        }

        card.CardPlayed?.Invoke();
        // Hand.RemoveCard(card);
    }

    public bool TryAttackTarget(int cardId, int targetId)
    {
        if (TryGetTableCardByPosition(cardId, out CreatureCard card))
        {
            if (!card.CanAttack)
            {
                return false;
            }
            bool isValid = false;
            switch(targetId) 
            {
                case (8):
                    isValid = TryCardAttackHero(OpposingPlayer.Hero, card);
                    return isValid;
                case (9):
                    return true;
                default:
                    // try get card
                    isValid = TryGetTableCardByPosition(targetId, out CreatureCard targetCard);
                    // try attack card
                    isValid = isValid && TryCardAttackCard(card, targetCard);
                    return isValid;
            }
        }
        else 
        {
            return false;
        }
    }

    private bool TryCardAttackHero(HeroBehaviour hero, CreatureCard oppCard)
    {
        bool isValid = hero != null && hero.isAlive;
        if (isValid)
        {
            hero.HealthComponent.Damage(oppCard.AttackComponent.Attack);
        }

        return isValid;
    }

    private bool TryCardAttackCard(CreatureCard attacker, CreatureCard target)
    {
        if (attacker == null)
        {
            throw new Exception("suka player");
        }
        if (target == null)
        {
            throw new Exception("keke player");
        }
        IHealth targetCardHealth = target.GetComponent<IHealth>();
        bool isValid = targetCardHealth.Health > 0;
        if (isValid)
        {
            attacker.AttackCard(target);
            attacker.CardEffects.ImplementOnTargetEffect(targetCardHealth);
        }
        return isValid;
    }
}