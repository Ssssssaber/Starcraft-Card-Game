using System.Collections.Generic;

namespace DefaultNamespace.Flyweight
{
    public class Flyweight
    {
        private Dictionary<int, CardStats> cards = new Dictionary<int, CardStats>();


        public CardStats GetCard(int id)
        {
            CardStats card = null;
            
            if (cards.ContainsKey(id))
            {
                return cards[id];
            }
            else
            {
                card = CardDatabase.CardsDict[id];
            }

            return card;
        }
    }
}