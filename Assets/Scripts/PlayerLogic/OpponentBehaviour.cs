using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using GOAP_System;
using Interfaces;
using PlayerLogic.CardPlacementStrategy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public enum OpponentState
{
    PlaceCards,
    ActOnTable,
}
    public class OpponentBehaviour : MonoBehaviour
    {
        public PlayerManager player;
        public PlayerManaComponent mana;
        public HandBehaviour hand;
        public TableBehaviour table;
        public TableBehaviour opponentTable;
        public HeroBehaviour opponentHero;

        public GameObject CardDisplayPlace;
        public GOAPPlanner planner;
        private Team team;
        private int randIndex;
        private bool _playerTableEmpty = true;
        private bool _opponentHandEmpty = true;
        private List<Card> _playedCards = new List<Card>();
        
        public float cardAttackDelay = 1f;
        public float cardPlaceDelay = 1f;
        
        private void Start()
        {
            player = GetComponentInParent<PlayerManager>();
            if (player.isManuallyControlled)
                return;

            CardDisplayPlace = GameObject.Find("CardDisplayPlace");

            table = player.Table;
            opponentTable = player.OpposingPlayer.Table;
            hand = player.Hand;
            mana = player.ManaComponent;
            opponentHero = player.OpposingPlayer.Hero;

            planner = GetComponent<GOAPPlanner>();
            team = player.Team;
            if (team == Team.Opponent)
            {
                EventManager.OnPlayerTurnEnd.AddListener(StartAiTurn);
            }
            else 
            {
                EventManager.OnOpponentTurnEnd.AddListener(StartAiTurn);
            }
            StartAiTurn();
        }

        public void StartAiTurn()
        {
            StartCoroutine("OpponentTurn");
        }

        private IEnumerator OpponentTurn()
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine("PlaceCards");
            // TryPlaceCardWithStrategy();
            yield return new WaitForSeconds(1f);
            ActionBase action = planner.ChooseAction();

            if (action != null)
            {
                // Debug.Log(action.GetType());
                action.OnTick();
            }
            else
            {
                Debug.Log("ti");
            }

            if (player.Team == Team.Player)
            {
                EventManager.PlayerTurnEnded();
            }
            else
            {
                EventManager.OpponentTurnEnded();
            }

            TurnSystem.instance.UpdateTurnStats();
        }

        private IEnumerator PlaceCards()
        {
            Card cardToPlay = ChooseCardToPlay();
            while (cardToPlay != null)
            {
                PlaceChosenCard(cardToPlay);
                cardToPlay = ChooseCardToPlay();
                yield return new WaitForSeconds(cardPlaceDelay);
            }

        }

        private Card ChooseCardToPlay()
        {
            _opponentHandEmpty = hand.CardsList.Count == 0;
            

            if (!_opponentHandEmpty)
            {
                // List<Card> sortedCards = new List<Card>(OpponentHand.CardsList);
                IEnumerable<string> names = hand.CardsList.Select(card => card.Name);
                // Debug.Log(names);
                IEnumerable<Card> sortedCards = hand.CardsList.OrderByDescending(card => card.PlayPriority);
                var oppCards = sortedCards.ToList();
                names = oppCards.Select(card => card.Name);
                string dbg = "";
                foreach(var card in oppCards)
                {
                    dbg += card.Name + ":  " + card.PlayPriority + "; ";
                }
                Debug.Log(dbg);
                Card bestCard = null;
                foreach (Card oppCard in oppCards)
                {
                    if (oppCard.PlayPriority == -1)
                    {
                        continue;
                    }
                    if (bestCard == null)
                    {
                        bestCard = oppCard;
                    }
                    else if (mana.currentMana >= oppCard.ManaComponent.Mana &&
                        table.GetCount() < table.maxCardsOnTable &&
                        bestCard.PlayPriority < oppCard.PlayPriority)
                    {
                        bestCard = oppCard;
                    }
                }
                return bestCard;
            }

            return null;
        }

        private void PlaceChosenCard(Card oppCard)
        {
            if (oppCard.Type == CardType.Creature)
            {
                oppCard.transform.SetParent(table.transform, false);
                CreatureCard temp = oppCard.GetComponent<CreatureCard>();
                temp.CanAttack = false;
            }
            oppCard.SetGoapGOVisible(false);
            oppCard.IsDraggable = false;
            oppCard.State = CardState.OnTable;
            oppCard.FaceCardDown(false);
            _playedCards.Add(oppCard);
            if (player.Team == Team.Player)
            {
                EventManager.PlayerCardPlayed(oppCard);
            }
            else
            {
                EventManager.OpponentCardPlayed(oppCard);
            }
            oppCard.CardPlayed?.Invoke();
            hand.RemoveCard(oppCard);
        }
        public IEnumerator GoFace()
        {
            // Debug.Log("GoFace");
            // in case any card dies after attacking (baneling, for example)
            List<CreatureCard> opponentTableCards = new List<CreatureCard>(table.GetCardsList());
            foreach (var oppCard in opponentTableCards)
            {
                if (oppCard.CanAttack)
                {
                    DamageHero(opponentHero, oppCard);
                }
            }

            yield return new WaitForSeconds(cardAttackDelay);
        }
        
        // think of cards that can attack multiple times
        public IEnumerator TradeThenGoFace()
        {
            Debug.Log("TradeThenGoFace");
            List<CreatureCard> opponentTableCards = new List<CreatureCard>(table.GetCardsList());
            
            foreach (CreatureCard oppCard in opponentTableCards)
            {
                _playerTableEmpty = opponentTable.GetCount() == 0;
                if (oppCard.CanAttack)
                {
                    if (!_playerTableEmpty)
                    {
                        List<CreatureCard> tempTable = opponentTable.GetCardsList();
                        int cardsCount = opponentTable.GetCount();
                        
                        randIndex = Random.Range(0, cardsCount);
                        if (tempTable[randIndex] == null)
                            randIndex = Random.Range(0, cardsCount);

                        CreatureCard targetCard = tempTable[randIndex];

                        if (targetCard != null)
                        {
                            IHealth targetCardHealth = targetCard.GetComponent<IHealth>();

                            if (targetCardHealth.Health > 0)
                            {
                                oppCard.AttackCard(targetCard);
                                oppCard.CardEffects.ImplementOnTargetEffect(targetCardHealth);
                                yield return new WaitForSeconds(cardAttackDelay);
                            }
                        }
                    }
                
                    else
                    {
                        DamageHero(opponentHero, oppCard);
                        yield return new WaitForSeconds(cardAttackDelay);
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

        private void DamageHero(HeroBehaviour hero, CreatureCard oppCard)
        {
            if (hero != null)
            {
                opponentHero.HealthComponent.Damage(oppCard.AttackComponent.Attack);
                // Debug.Log("damagehero");
            }
        }
    }