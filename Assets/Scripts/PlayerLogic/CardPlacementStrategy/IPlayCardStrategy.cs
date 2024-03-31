using System.Collections.Generic;

namespace PlayerLogic.CardPlacementStrategy
{
    public interface IPlayCardStrategy
    {
        public Card Execute(List<Card> cardsList);
    }
}