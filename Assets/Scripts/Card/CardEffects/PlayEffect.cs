using UnityEngine;
using System;
using System.Reflection;
using DefaultNamespace;
using Interfaces;

public class PlayEffect : IEffects
{
    private Card _card;
    private MethodInfo _onPlay;
    private Type _onPlayType = typeof(OnPlayEffectsMethods);

    public PlayEffect(Card card)
    {
        _card = card;
        _onPlay = _onPlayType.GetMethod(card.Name);
    }
    
    public void LoadMethod(Card card)
    {
        _card = card;
        _onPlay = _onPlayType.GetMethod(card.Name);
    }

    public void ImplementMethod()
    {
        object effectInstance = Activator.CreateInstance(_onPlayType, null);
        _onPlay?.Invoke(effectInstance, new object[] { _card });
    }

    public string GetDesc()
    {
        return _card.Name;
    }
}