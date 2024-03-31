using UnityEngine;
using System;
using System.Reflection;
using DefaultNamespace;
using Interfaces;

public class AttackEffect : IEffects
{
    private Card _card;
    private MethodInfo _onAttack;
    private Type _onAttackType = typeof(OnAttackEffectsMethods);

    public AttackEffect(Card card)
    {
        _card = card;
        _onAttack = _onAttackType.GetMethod(card.Name);
    }
    
    public void LoadMethod(Card card)
    {
        _card = card;
        _onAttack = _onAttackType.GetMethod(card.Name);
    }

    public void ImplementMethod(IHealth target)
    {
        object effectInstance = Activator.CreateInstance(_onAttackType, null);
        _onAttack?.Invoke(effectInstance, new object[] { _card, target });
    }

    public void ImplementMethod()
    {
        throw new NotImplementedException();
    }

    public string GetDesc()
    {
        return _card.Name;
    }
}