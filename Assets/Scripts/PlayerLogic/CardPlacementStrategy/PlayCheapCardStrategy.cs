using System.Collections.Generic;
using System.Linq;

// using static BoardAwareness;

namespace PlayerLogic.CardPlacementStrategy
{
    public class PlayCheapCardStrategy : IPlayCardStrategy
    {
        private BoardAwareness _awareness;
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