using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using static GameUtilities;
using System;
using System.Reflection;
using GOAP_System;
using TMPro;

public class CreatureCard : Card
{
    public HealthComponent HealthComponent;
    public CreatureCardEffects CardEffects;
    public CreatureCardDisplay CreatureCardDisplay;
    public GameObject AttackIndicator;
    private Type _calcType = typeof(CardCostCalculation);
    private MethodInfo _onCalc;

    public bool CanAttack
    {
        get
        {
            return _canAttack;
        }
        set
        {
            _canAttack = value;
            if(AttackIndicator != null)
                AttackIndicator.SetActive(_canAttack);
        }
    }

    public bool _canAttack = false;
    private void Awake()
    {
        Team = Team.Player;
        State = CardState.InDeck;
        HealthComponent = GetComponent<HealthComponent>();
        ManaComponent = GetComponent<ManaComponent>();
        AttackComponent = GetComponent<AttackComponent>();
        
        HealthComponent.OnDie.AddListener(CardDied);
        CreatureCardDisplay = GetComponent<CreatureCardDisplay>();
        
        
        // Debug.Log(_onCalc == null);
    }

    private void Start()
    {
        _onCalc= _calcType.GetMethod(Name);
    }
    
    public override void ApplyStats(CardStats cardStats)
    {
        base.ApplyStats(cardStats);
        
        ManaComponent.SetupMana(cardStats.ManaCost);
        AttackComponent.SetupAttack(cardStats.Attack);
        HealthComponent.SetupHealth(cardStats.Health);
        CreatureCardDisplay.ApplyStats();

        Name = CapitalizeRemoveSpaces(Name);
        
        CardEffects.LoadCardEffects();
    }

    public override int CalculatePlayPrioririty()
    {
        if (_onCalc != null)
        {
            // Binder defaultBinder = Type.DefaultBinder;
            object effectInstance = Activator.CreateInstance(_calcType, null);
            // onCalc?.Invoke(effectInstance, new object[] {});
        //     object effectInstance = Activator.CreateInstance(_onDieType, null);
        // _onDie?.Invoke(effectInstance, new object[] { _card });
            object ret = _calcType.InvokeMember(Name, BindingFlags.InvokeMethod,null, effectInstance, new object[] { ownerPlayer });
            Debug.Log("return extra value");
            return (int)ret;
        }
        // Debug.Log("return normal value");
        return 5 * (HealthComponent.Health + AttackComponent.Attack);
    }

    // public override void CalcPrirorityEvent()
    // { 
    //     PlayPriority = CalculatePlayPrioririty();
    // }

    

    public override void CloneCard(Card card)
    {
        base.CloneCard(card);
        CreatureCard creature = card.GetComponent<CreatureCard>();
        
        ManaComponent = creature.ManaComponent;
        AttackComponent = creature.AttackComponent;
        HealthComponent = creature.HealthComponent;
        CardEffects = creature.CardEffects;
        CreatureCardDisplay.ApplyStats();
    }
    
    public void AttackCard(CreatureCard otherCard)
    {
        IHealth cardHealth = GetComponent<IHealth>();
        IHealth otherCardHealth = otherCard.GetComponent<IHealth>();
        if (otherCard.Team != Team && this.CanAttack)
        {
            otherCardHealth.Damage(AttackComponent.Attack);
            cardHealth.Damage(otherCard.AttackComponent.Attack);
            CanAttack = false;
        }
    }


    public void AttackHero(HeroBehaviour hero)
    {
        IHealth heroHealth = hero.gameObject.GetComponent<IHealth>();

        if (hero.Team != Team && CanAttack && heroHealth != null)
        {
            heroHealth.Damage(AttackComponent.Attack);
        }
    }

    public override void FaceCardDown(bool down)
    {
        CreatureCardDisplay.DisplayBackImage(down);
    }
    
    public override void CardDied()
    {
        CardEffects.ImplementOnDieEffect();
        EventManager.OnCardRemovedFromTable.Invoke(this);
        Destroy(this.gameObject, 1f);
    }
}