using DefaultNamespace.Factory;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class CardDisplayPlace : MonoBehaviour
    {
        private GameObject MainCanvas;
        private AbstractFactory _playerFactory;
        private AbstractFactory _opponentFactory;
        private BoardAwareness _awareness;
        private Flyweight.Flyweight _flyweight;
        private void Start()
        {
            _awareness = BoardAwareness.Instance;
            _playerFactory = AssignFactory(_awareness.player.Race);
            _opponentFactory = AssignFactory(_awareness.opponent.Race);
            // _playerFactory = AssignFactory(aw)

            _flyweight = new Flyweight.Flyweight();
            
            MainCanvas = _awareness.MainCanvas.gameObject;
            EventManager.OnPlayerCardPlayed.AddListener(DisplayPlayedCard);
            // EventManager.AddPlayerCardPlayed(DisplayPlayedCard);
            EventManager.OnOpponentCardPlayed.AddListener(DisplayPlayedCard);
        }

        private AbstractFactory AssignFactory(Race race)
        {
            switch (race)
            {
                case (Race.Protoss):
                    return new ProtossCardFactory();
                case (Race.Zerg):
                    return new ZergCardFactory();
                case (Race.Terran):
                    // return new ZergCardFactory();
                    throw new System.Exception("card hue");
                
                default:
                    // Debug.Log("HeheHaha");
                    return null;
            }
        }
        
        // Pretty lazy way of fixing the problem, may not be optimal
        // bug: cards played by opponent do not have backimage
        public void DisplayPlayedCard(Card card)
        {
            Card tempCard = CreateCardPrefab(card);
            tempCard.transform.position = transform.position;
            // tempCard.CloneCard(card);
            
            tempCard.ApplyStats(_flyweight.GetCard(card.ID));
            
            tempCard.transform.SetParent(MainCanvas.transform);
            tempCard.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            Destroy(tempCard.gameObject, 1f);
        }

        public void DisplayPlayedCardFlyweight(Card card)
        {
            Card tempCard = CreateCardPrefab(card);
            tempCard.transform.position = transform.position;
            // tempCard.CloneCard(card);
            tempCard.ApplyStats(_flyweight.GetCard(card.ID));
            
            tempCard.transform.SetParent(MainCanvas.transform);
            tempCard.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            Destroy(tempCard.gameObject, 1f);
        }
        

        private Card CreateCardPrefab(Card card)
        {
            if (card.Team == Team.Player)
            {
                if (card.Type == CardType.Creature)
                {
                    return _playerFactory.CreateCreatureCard(_awareness.player);
                }
                else
                {
                    return _playerFactory.CreateSpellCard(_awareness.player);
                }
            }
            else 
            {
                if (card.Type == CardType.Creature)
                {
                    return _opponentFactory.CreateCreatureCard(_awareness.opponent);
                }
                else
                {
                    return _opponentFactory.CreateSpellCard(_awareness.opponent);
                }
            }
        }
    }
}