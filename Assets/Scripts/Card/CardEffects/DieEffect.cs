using UnityEngine;
using System;
using System.Reflection;
using DefaultNamespace;
using Interfaces;


public class DieEffect : IEffects
{
    private Card _card;
    private MethodInfo _onDie;
    private Type _onDieType = typeof(OnDieEffectsMethods);

    public DieEffect(Card card)
    {
        _card = card;
        _onDie = _onDieType.GetMethod(card.Name);
    }

    public void LoadMethod(Card card)
    {
        _card = card;
        _onDie = _onDieType.GetMethod(card.Name);
    }

    public void ImplementMethod()
    {
        object effectInstance = Activator.CreateInstance(_onDieType, null);
        _onDie?.Invoke(effectInstance, new object[] { _card });
    }

    public string GetDesc()
    {
        return _card.Name;
    }
}