
using System;
using System.Collections.Generic;
using UnityEngine;
public class GoalGoFace : GoalBase
{
    [SerializeField] private int Priority = 10;
    [SerializeField] private int cardWeight = 5;
    private BoardAwareness _awareness;

    private void Start()
    {
        _awareness = BoardAwareness.awareness;
    }


    public override bool CanRun()
    {
        // return !_awareness.OpponentTable.IsEmpty();
        return !player.Table.IsEmpty();
    }

    public override int CalculatePriority()
    {
        if (!CanRun())
        {
            return 0;
        }
        // int finalPrio = Priority + _awareness.CalculateOpponentCardDamage() +
        //                 (_awareness.OpponentHeroHealth() - _awareness.CalculatePlayerCardDamage()) -
        //                 cardWeight * _awareness.PlayerTableCardsCount(); 
        int finalPrio = Priority + player.CalculateCardDamage() + 
                        player.OpposingPlayer.HeroHealth() - player.OpposingPlayer.CalculateCardDamage() - 
                        cardWeight * player.OpposingPlayer.TableCardsCount();
 
        return finalPrio < 0 ? 0 : finalPrio;
    }
}