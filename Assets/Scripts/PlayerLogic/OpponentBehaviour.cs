using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GOAP_System;
using Interfaces;
using UnityEngine;
using RSG;
using System;
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
        private IPromiseTimer promiseTimer;

        private void Awake()
        {
            promiseTimer = new PromiseTimer();
        }

        private void Start()
        {
            player = GetComponentInParent<PlayerManager>();
            if (player.ControlType != PlayerControl.GOAP)
                return;

            CardDisplayPlace = GameObject.Find("CardDisplayPlace");

            table = player.Table;
            opponentTable = player.OpposingPlayer.Table;
            hand = player.Hand;
            mana = player.ManaComponent;
            opponentHero = player.OpposingPlayer.Hero;

            planner = GetComponent<GOAPPlanner>();
            team = player.Team;
            if (team == Team.Player)
            {
                EventManager.OnOpponentTurnEndAfter.AddListener(StartAiTurn);
                StartAiTurn();
            }
            else 
            {
                EventManager.OnPlayerTurnEndAfter.AddListener(StartAiTurn);
            }
            
        }

        void Update()
        {
            // deltaTime is equal to the time since the last MainLoop
            // promiseTimer.Update(Time.deltaTime);

            // Process your other logic here
        }

        public void StartAiTurn()
        {
            StartCoroutine("OpponentTurn");
        }

        private IEnumerator OpponentTurn()
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine("PlaceCards");
            yield return new WaitForSeconds(1f);
            ActionBase action = planner.ChooseAction();

            if (action != null)
            {
                action.OnTick();
            }
            else
            {
                Debug.Log("ti");
            }

            TurnSystem.instance.SwitchTurn();
        }

        private void OpponentTurnPromise()
        {
            StartCoroutine("PlaceCards");
            ActionBase action = planner.ChooseAction();

            if (action != null)
            {
                action.OnTick();
            }
            else
            {
                Debug.Log("ti");
            }

            TurnSystem.instance.SwitchTurn();
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


        private void CardsLoop()
        {
            
        }

        private Card ChooseCardToPlay()
        {
            _opponentHandEmpty = hand._cardsList.Count == 0;

            if (!_opponentHandEmpty)
            {
                IEnumerable<string> names = hand._cardsList.Select(card => card.Name);
                IEnumerable<Card> sortedCards = hand._cardsList.OrderByDescending(card => card.PlayPriority);
                var handCards = sortedCards.ToList();
                names = handCards.Select(card => card.Name);
                string dbg = "";
                foreach(var card in handCards)
                {
                    dbg += card.Name + ":  " + card.PlayPriority + "; ";
                }
                Debug.Log($"{player.Team}: {dbg}");
                Card bestCard = null;
                foreach (Card card in handCards)
                {
                    // if no reason to play this card
                    if (card.PlayPriority == -1)
                    {
                        continue;
                    }
                    // if cannot play this card
                    if (!(mana.currentMana >= card.ManaComponent.Mana && table.GetCount() < table.maxCardsOnTable))
                    {
                        continue;
                    }

                    if (bestCard == null)
                    {
                        bestCard = card;
                    }

                    else if (bestCard.PlayPriority < card.PlayPriority)
                    {
                        bestCard = card;
                    }
                }
                return bestCard;
            }

            return null;
        }

        private void PlaceChosenCard(Card card)
        {
            if (card.Type == CardType.Creature)
            {
                card.transform.SetParent(table.transform, false);
                CreatureCard temp = card.GetComponent<CreatureCard>();
                temp.CanAttack = false;
            }
            card.SetGoapGOVisible(false);
            card.IsDraggable = false;
            card.State = CardState.OnTable;
            card.FaceCardDown(false);
            _playedCards.Add(card);
            if (player.Team == Team.Player)
            {
                EventManager.PlayerCardPlayed(card);
            }
            else
            {
                EventManager.OpponentCardPlayed(card);
            }

            Notify($"played {card.Name}");

            card.CardPlayed?.Invoke();
            hand.RemoveCard(card);
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
            Notify("TradeThenGoFace");
            List<CreatureCard> tableCards = new List<CreatureCard>(table.GetCardsList());
            
            foreach (CreatureCard card in tableCards)
            {
                if (card == null)
                {
                    table.GetCardsList().Remove(card);
                    continue;
                }
                _playerTableEmpty = opponentTable.GetCount() == 0;
                if (card.CanAttack)
                {
                    if (!_playerTableEmpty)
                    {
                        if (card == null)
                        {
                            throw new Exception("suka player 3");
                        }

                        List<CreatureCard> tempTable = opponentTable.GetCardsList();
                        int cardsCount = opponentTable.GetCount();

                        if (card == null)
                        {
                            throw new Exception("suka player 2");
                        }

                        
                        randIndex = UnityEngine.Random.Range(0, cardsCount);
                        if (tempTable[randIndex] == null)
                            randIndex = UnityEngine.Random.Range(0, cardsCount);

                        CreatureCard targetCard = tempTable[randIndex];
                        if (card == null)
                        {
                            throw new Exception("suka player 1");
                        }

                        if (targetCard != null)
                        {
                            IHealth targetCardHealth = targetCard.GetComponent<IHealth>();

                            if (targetCardHealth.Health > 0)
                            {
                                if (card == null)
                                {
                                    throw new Exception("suka player");
                                }
                                card.AttackCard(targetCard);
                                card.CardEffects.ImplementOnTargetEffect(targetCardHealth);
                                
                                Notify($"{card.Name} attacks {targetCard.Name}");
                                
                                yield return new WaitForSeconds(cardAttackDelay);
                            }
                        }
                    }
                
                    else
                    {
                        DamageHero(opponentHero, card);
                        Notify($"{card.Name} attacks {opponentHero.Team}");
                        yield return new WaitForSeconds(cardAttackDelay);
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

        private void DamageHero(HeroBehaviour hero, CreatureCard oppCard)
        {
            if (hero != null && hero.isAlive)
            {
                hero.HealthComponent.Damage(oppCard.AttackComponent.Attack);
                // Debug.Log("damagehero");
            }
        }


        private void Notify(string message)
        {
            // Debug.Log($"{player.Team} : {message}");
        }
    }