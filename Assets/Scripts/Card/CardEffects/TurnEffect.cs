using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using DefaultNamespace;
using Interfaces;
using UnityEngine.Events;

public delegate void TurnDelegate();
public class TurnEffect : IEffects
{
    public TurnDelegate del;

    public int Duration
    {
        get;
        private set;
    }
    
    private Card _card;
    private MethodInfo _onTurn;
    private Type _onTurnType = typeof(OnTurnEffectsMethods);


    public TurnEffect(Card card)
    {
        _card = card;
        _onTurn = _onTurnType.GetMethod(card.Name);
        del = ImplementMethod;
        ChangeDuration(2);
        // Debug.Log($"{_card.Name} : {Duration}");
    }
    
    public void ChangeDuration(int duration)
    {
        Duration = duration;
    }

    public void LoadMethod(Card card)
    {
        _card = card;
        _onTurn = _onTurnType.GetMethod(card.Name);
    }

    public void ImplementMethod()
    {
        if (Duration > 0)
        {
            object effectInstance = Activator.CreateInstance(_onTurnType, null);
            _onTurn?.Invoke(effectInstance, new object[] { _card });
            Duration--;
            // Debug.Log(_onTurn.Name + " " + Duration);
        }
        // Debug.Log("Duration over");
    }

    public string GetDesc()
    {
        return _card.Name;
    }
}