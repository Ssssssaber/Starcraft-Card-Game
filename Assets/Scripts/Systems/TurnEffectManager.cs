using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEffectManager
{

    private List<TurnEffect> _playerTurnEffects = new List<TurnEffect>();
    private List<TurnEffect> _opponentTurnEffects = new List<TurnEffect>();

    public void ResetTurnEffects()
    {
        _playerTurnEffects = new List<TurnEffect>();
        _opponentTurnEffects = new List<TurnEffect>();
    }

    public void AddOpponentTurnEffect(TurnEffect effect)
    {
        _opponentTurnEffects.Add(effect);
    }

    public void AddPlayerTurnEffect(TurnEffect effect)
    {
        _playerTurnEffects.Add(effect);
    }

    public void ImplementTurnEffects(Team team)
    {
        if (team == Team.Player)
        {
            ImplementTurnEffects(ref _playerTurnEffects);
        }
        else 
        {
            ImplementTurnEffects(ref _opponentTurnEffects);
        }
    }
    private void ImplementTurnEffects(ref List<TurnEffect> effects)
    {
        if (effects.Count == 0) return;
        
        List<TurnEffect> expiredEffects = new List<TurnEffect>();
        foreach (var effect in effects)
        {
            switch (effect.Duration)
            {
                case (1):
                    effect.ImplementMethod();
                    Debug.Log($"EffectName: {effect.GetDesc()} : {effect.Duration}");
                    expiredEffects.Add(effect);
                    break;
                case (0):
                    Debug.Log($"EffectName: {effect.GetDesc()} : {effect.Duration}");
                    break;
                default:
                    effect.ImplementMethod();
                    break;
            }
        }

        foreach (var effect in expiredEffects)
        {
            effects.Remove(effect);
        }
    }
    public void PrintTurnEffectsInfo(List<TurnEffect> effects)
    {
        string dbg = $"EFFECTS: ";
        foreach (var effect in effects)
        {
            dbg += $"{effect.GetDesc()} : {effect.Duration}; ";
        }
        Debug.Log(dbg);
    }
}
