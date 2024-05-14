using System;
using System.Collections.Generic;
using UnityEngine;
using MockObjects;
// using static BoardAwareness;
public class GoalControlTable : GoalBase
{
    [SerializeField] private int _priority = 50;
    [SerializeField] private int cardWeight = 5;
    private BoardAwareness _awareness;
    
    protected List<System.Type> supportedActions = new List<Type>() {typeof(ActionTradeCards)};
    private void Start()
    {
        _awareness = BoardAwareness.awareness;
    }

    private List<System.Type> _suitableActions = new List<System.Type>() { typeof(ActionTradeCards) };


    public override bool CanRun()
    { 
        // return !(_awareness.OpponentTable.IsEmpty() && _awareness.PlayerTable.IsEmpty());
        return !(player.Table.IsEmpty() && player.OpposingPlayer.Table.IsEmpty());
    }

    public override int CalculatePriority()
    {
        /*
         VARIABLES:
         1. Player cards on table
            1) Damage of player cards (chances to stay alive)
            2) Count of player cards
         2. Opponent cards on table
         3. Player hero health
         4. Opponent hero health 
        */
        if (!CanRun())
        {
            return 0;
        }

        
        // int finalPrio = - _awareness.CalculateOpponentCardDamage() +
        //             (_awareness.CalculatePlayerCardDamage()) +
        //             cardWeight * _awareness.PlayerTableCardsCount();
        int finalPrio = - player.CalculateCardDamage() +
                        player.OpposingPlayer.CalculateCardDamage() +
                        cardWeight * player.OpposingPlayer.TableCardsCount();
        
        return finalPrio < 0 ? 0 : finalPrio;
    }

    

    public int TestCalculatePriority(MockTableBehavior table)
    {
        return cardWeight * table.CardsList.Count;
    }
}