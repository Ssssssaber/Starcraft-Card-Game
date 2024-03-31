using UnityEngine;
using System;
using Interfaces;
using System.Reflection;
using DefaultNamespace;
using Unity.VisualScripting;

public class CreatureCardEffects : MonoBehaviour
{
    private PlayEffect _playEffect;
    private DieEffect _dieEffect;
    private AttackEffect _attackEffect;

    [SerializeField] private CreatureCard _card;
    private void Start()
    {
        _card = GetComponent<CreatureCard>();

        if (_card != null)
        {
            _card.CardPlayed.AddListener(ImplementOnPlayEffect);
        }
    }

    public void LoadCardEffects()
    {
        
        if (_card != null)
        {
            _playEffect = new PlayEffect(_card);
            _dieEffect = new DieEffect(_card);
            _attackEffect = new AttackEffect(_card);
        }
    }

    
    public void ImplementOnTargetEffect(IHealth target)
    {
        _attackEffect.ImplementMethod(target);
    }

    public void ImplementOnPlayEffect()
    {
        _playEffect.ImplementMethod();
    }
    public void ImplementOnDieEffect()
    {
        _dieEffect.ImplementMethod();
    }
}