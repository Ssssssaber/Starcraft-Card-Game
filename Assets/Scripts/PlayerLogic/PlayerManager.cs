using System;
using Menu;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerManager : MonoBehaviour
{   
    [Header("Pre game stats")]
    public Team Team;
    public Race Race;
    public HeroStats HeroStats;

    [Header("In game components")]
    public HeroBehaviour Hero;
    public PlayerManaComponent ManaComponent;
    public TableBehaviour Table;
    public HandBehaviour Hand;
    public DeckBehaviour Deck;
    public OpponentBehaviour AI;
    public PlayerManager OpposingPlayer;
    public bool isManuallyControlled = true;
    private void Awake()
    {
        
        AI = GetComponentInChildren<OpponentBehaviour>();
        if (AI != null)
        {
            isManuallyControlled = false;
        }
        else
        {
            isManuallyControlled = true;
        }

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

    
    

}