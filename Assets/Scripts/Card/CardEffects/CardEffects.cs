using System.Collections.Generic;
using System.Collections;
using System;
using DefaultNamespace;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using static CardEffectsBaseMethods;

public static class CardEffectsBaseMethods
{
    private static BoardAwareness _awareness = BoardAwareness.awareness;
    public static void AOESpell(int interactionAmount, PlayerManager player, Action<CreatureCard, int> action)
    {
        foreach (var card in new List<CreatureCard>(player.Table.GetCardsList()))
        {
            action(card, interactionAmount);
        }
    }
    

    public static void AOESpell(int spellDamage, Action<CreatureCard, int> action)
    {
        AOESpell(spellDamage, _awareness.player, action);
        AOESpell(spellDamage, _awareness.opponent, action);
    }

    public static void AOEHandSpell(int interactionAmount, PlayerManager player, Action<Card, int> action)
    {
        // HandBehaviour targetHand;

        // if (targetTeam == Team.Opponent)
        // {
        //     targetHand = _awareness.OpponentHand;
        // }
        // else
        // {
        //     targetHand = _awareness.PlayerHand;
        // }

        foreach (var card in new List<Card>(player.Hand.CardsList))
        {
            action(card, interactionAmount);
        }
    }
    public static void CardSpell(int spellDamage, Team targetTeam, Action<CreatureCard, int> action)
    {
        
    }

    public static void HeroSpell(int spellDamage, PlayerManager targetPlayer, Action<HeroBehaviour, int> action)
    {
        action(targetPlayer.Hero, spellDamage);
        // HeroBehaviour hero;

        // if (targetTeam == Team.Opponent)
        // {
        //     hero = _awareness.PlayerHero;
        // }
        // else
        // {
        //     hero = _awareness.OpponentHero;
        // }

        // action(hero, spellDamage);
    }

    public static void DamageCard(CreatureCard card, int amount)
    {
        IHealth cardHealth = card.GetComponent<IHealth>();

        if (cardHealth != null)
        {
            // Debug.Log("hahaha");
            cardHealth.Damage(amount);
        }
    }
    
    public static void DamageTarget(IHealth target, int amount)
    {
        if (target != null)
        {
            target.Damage(amount);
        }
    }

    public static void HealCard(CreatureCard card, int amount)
    {
        IHealth cardHealth = card.GetComponent<IHealth>();

        if (cardHealth != null)
        {
            cardHealth.Heal(amount);
            // Debug.Log("hahaha");
        }
    }

    public static void BuffCardHealth(CreatureCard card, int amount)
    {
        IHealth cardHealth = card.GetComponent<IHealth>();

        if (cardHealth != null)
        {
            cardHealth.IncreaseHealth(amount);
        }
    }

    public static void DeBuffCardHealth(CreatureCard card, int amount)
    {
        IHealth cardHealth = card.GetComponent<IHealth>();

        if (cardHealth != null && cardHealth.Health == 1)
        {
            cardHealth.DecreaseHealth(amount);
        }
    }

    
    public static void HealTarget(IHealth target, int amount)
    {
        if (target != null)
        {
            target.Heal(amount);
        }
    }

    public static void IncreaseCardAttack(CreatureCard card, int amount)
    {
        IAttack cardAttack = card.GetComponent<IAttack>();

        if (cardAttack != null)
        {
            cardAttack.IncreaseAttack(amount);
        }
    }

    public static void DecreaseCardAttack(CreatureCard card, int amount)
    {
        IAttack cardAttack = card.GetComponent<IAttack>();

        if (cardAttack != null)
        {
            cardAttack.DecreaseAttack(amount);
        }
    }

    public static void IncreaseCardManaCost(Card card, int amount)
    {
        IMana cardMana = card.GetComponent<IMana>();

        if (cardMana != null)
        {
            cardMana.IncreaseManaCost(amount);
        }
    }

    public static void DecreaseCardManaCost(Card card, int amount)
    {
        IMana cardAttack = card.GetComponent<IMana>();

        if (cardAttack != null)
        {
            cardAttack.DecreaseManaCost(amount);
        }
    }

    public static void AddPlayerMana(PlayerManager player, int amount)
    {
        player.ManaComponent.AddMana(amount);
        // if (team == Team.Opponent)
        // {
        //     _awareness.OpponentDeck.DrawCards(amount);
        // }
        // else
        // {
        //     _awareness.PlayerDeck.DrawCards(amount);
        // }
    }

    public static void LosePlayerMana(PlayerManager player, int amount)
    {
        player.ManaComponent.LoseMana(amount);
        // if (team == Team.Opponent)
        // {
        //     _awareness.OpponentManaComponent.LoseMana(amount);
        // }
        // else
        // {
        //     _awareness.PlayerManaComponent.LoseMana(amount);
        // }
    }

    public static void DrawCard(PlayerManager player, int amount)
    {
        player.Deck.DrawCards(amount);
        // if (team == Team.Opponent)
        // {
        //     _awareness.OpponentDeck.DrawCards(amount);
        // }
        // else
        // {
        //     _awareness.PlayerDeck.DrawCards(amount);
        // }
    }

    public static bool ListContainsIndex(List<CreatureCard> cardList, int index)
    {
        try
        {
            CreatureCard card = cardList[index];
            return true;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }

    public static Team ReverseTeam(Team team)
    {
        if (team == Team.Opponent)
        {
            return Team.Player;
        }
        else
        {
            return Team.Opponent;
        }
    }
}


public class OnPlayEffectsMethods
{
    public void DarkTemplar(CreatureCard attachedCard)
    {
        Debug.Log("heaherahehafeha");
        // (attachedCard.GetComponent<IHealth>(), 2);
    }

    public void WildMutation(SpellCard attachedCard)
    {
        AOESpell(attachedCard.AttackComponent.Attack, attachedCard.ownerPlayer, BuffCardHealth);
        AOESpell(attachedCard.AttackComponent.Attack, attachedCard.ownerPlayer, IncreaseCardAttack);
    }

    public void Transcendence(SpellCard attachedCard)
    {
        AddPlayerMana(attachedCard.ownerPlayer, 1);
        DrawCard(attachedCard.ownerPlayer, 1);
    }

    public void KhaydarinAmulet(SpellCard attachedCard)
    {
        AOEHandSpell(attachedCard.AttackComponent.Attack, attachedCard.ownerPlayer, DecreaseCardManaCost);
    }

    
    public void Infusion(SpellCard attachedCard)
    {
        // AddPlayerMana(attachedCard.Team, 1);
        DrawCard(attachedCard.ownerPlayer, 2);
    }
}

public class OnDieEffectsMethods
{
    public void Queen(Card attachedCard)
    {
        Debug.Log(attachedCard);
    }
}

public class OnAttackEffectsMethods
{
    public Action<IHealth, int> CardAction;
    
    public void Mutalisk(Card attachedCard, IHealth target)
    {
        DamageTarget(target, 1);
    }
}

public class OnTurnEffectsMethods
{
    public delegate void TurnDelegate(Card attachedCard);
    
    public void Storm(Card attachedCard)
    {
        AOESpell(attachedCard.AttackComponent.Attack, attachedCard.ownerPlayer.OpposingPlayer, DamageCard);
    }

    public void Transfuse(Card attachedCard)
    {
        AOESpell(attachedCard.AttackComponent.Attack, attachedCard.ownerPlayer, HealCard);
    }
}