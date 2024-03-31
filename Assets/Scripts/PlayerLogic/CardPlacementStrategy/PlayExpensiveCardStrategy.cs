using System.Collections.Generic;

namespace PlayerLogic.CardPlacementStrategy
{
    public class PlayExpensiveCardStrategy : IPlayCardStrategy
    {
        private BoardAwareness _awareness = BoardAwareness.awareness;
        public Card Execute(List<Card> cardsList)
        {
            _awareness = BoardAwareness.awareness;
            
            Card resultCard = cardsList[0];
            foreach (Card card in cardsList)
            {
                if (card.ManaComponent.Mana < resultCard.ManaComponent.Mana)
                {
                    resultCard = card;
                }
            }

            return resultCard;
        }
    }
}