using System.Collections.Generic;

namespace PlayerLogic.CardPlacementStrategy
{
    public class PlayExpensiveCardStrategy : IPlayCardStrategy
    {
        private BoardAwareness _awareness = BoardAwareness.Instance;
        public Card Execute(List<Card> cardsList)
        {
            _awareness = BoardAwareness.Instance;
            
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