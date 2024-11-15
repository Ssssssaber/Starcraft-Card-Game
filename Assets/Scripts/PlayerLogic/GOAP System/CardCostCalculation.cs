using System.Collections.Generic;
using System.Linq;
using static GameUtilities;

namespace GOAP_System
{
    public class CardCostCalculation
    {
        private BoardAwareness _awareness = BoardAwareness.Instance; 
        public int Transfuse(PlayerManager player)
        {
            IEnumerable<CreatureCard> missingHealthCards = player.Table.GetCardsList()
                .Where(card => card.HealthComponent.Health < card.HealthComponent.MaxHealth);
            if ( missingHealthCards.Count() == 0)
            {
                return -1;
            }
            return missingHealthCards.Count() * CARD_WEIGHT;
        }
        public int Transcendence(PlayerManager player)
        {
            int freeHandSlots = MAX_HAND_CAPACITY - player.Hand._cardsList.Count;
            if (freeHandSlots == 0)
            {
                return -1;
            }
            return freeHandSlots * CARD_WEIGHT * 5;
        }

        public int WildMutation(PlayerManager player)
        {
            int cardsOnTable = player.Table.GetCount();
            if (cardsOnTable == 0)
            {
                return -1;
            }
            return cardsOnTable * CARD_WEIGHT * 5;
        }

        public int Infusion(PlayerManager player)
        {
            int cardsOnTable = player.Table.GetCount();
            if (cardsOnTable == 0)
            {
                return -1;
            }
            return cardsOnTable * CARD_WEIGHT * 5;
        }

        public int KhaydarinAmulet(PlayerManager player)
        {
            int handCount = player.Hand._cardsList.Count;
            if (handCount == 0)
            {
                return -1;
            }
            return handCount * CARD_WEIGHT * 5;
        }

		public int OrbitalStrike(PlayerManager player)
		{
			int cardsOnTable = player.OpposingPlayer.Table.GetCount();
            if (cardsOnTable == 0)
            {
                return -1;
            }
            return cardsOnTable * CARD_WEIGHT * 5;
		}
    }
}