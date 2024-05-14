
using static GameUtilities;
using System;
using System.Reflection;
using GOAP_System;

public class SpellCard : Card
{
    public SpellCardDisplay SpellCardDisplay;

    public SpellCardEffects CardEffects;
    private Type _calcType = typeof(CardCostCalculation);
    private MethodInfo _onCalc;
    
    
    private void Awake()
    {
        Team = Team.Player;
        State = CardState.InDeck;
        CardEffects = GetComponent<SpellCardEffects>();
        ManaComponent = GetComponent<ManaComponent>();
        AttackComponent = GetComponent<AttackComponent>();    
        SpellCardDisplay = GetComponent<SpellCardDisplay>();
        
        
        // Debug.Log($"{this.Name} : ({_onCalc == null})");
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
        SpellCardDisplay.ApplyStats();
       
        Name = CapitalizeRemoveSpaces(Name);

        CardEffects.LoadCardEffects(Name);
    }

    public override int CalculatePlayPrioririty()
    {
        if (_onCalc != null)
        {
            object effectInstance = Activator.CreateInstance(_calcType, null);
            object ret = _calcType.InvokeMember(Name, BindingFlags.InvokeMethod,null, effectInstance, new object[] { ownerPlayer });
            // Debug.Log("return extra value");
            return (int)ret;
        }
        return 5 * (ManaComponent.Mana);
    }

    // public override void CalcPrirorityEvent()
    // { 
    //     PlayPriority = CalculatePlayPrioririty();
    // }

    public override void FaceCardDown(bool down)
    {
        SpellCardDisplay.DisplayBackImage(down);
    }


    public override void CloneCard(Card card)
    {
    
        base.CloneCard(card);
        SpellCard spell = card.GetComponent<SpellCard>();
        ManaComponent = spell.ManaComponent;
        CardEffects = spell.CardEffects;
        SpellCardDisplay.ApplyStats();
    }
}