using System.Collections.Generic;
using UnityEngine.Events;


    public class EventManager
    {
        public static bool IsDatabaseCreated = false;

        public static UnityEvent OnEnvironmentChange = new UnityEvent();

        // Player Events
        public readonly static UnityEvent<Card> OnPlayerCardDealt = new UnityEvent<Card>();
        public readonly static UnityEvent<Card> OnPlayerCardPlayed = new UnityEvent<Card>();
        public readonly static UnityEvent OnPlayerTurnEnd = new UnityEvent();
        public readonly static UnityEvent OnPlayerTurnEndAfter = new UnityEvent();
        
        // random garbage
        public readonly static UnityEvent<CardStats> OnCardLoaded = new UnityEvent<CardStats>();
        public readonly static UnityEvent<List<CardStats>> OnAllCardsLoaded = new UnityEvent<List<CardStats>>();
        public readonly static UnityEvent OnDatabaseCreated = new UnityEvent();
        public readonly static UnityEvent OnGameEnded = new UnityEvent();
        // public static UnityEvent OnAnyCardPlayed = new UnityEvent();

        public readonly static UnityEvent OnPlayerTurnStart = new UnityEvent();

        public static void PlayerTurnStarted()
        {
            OnPlayerTurnStart?.Invoke();
        }
        
        public static void CardLoaded(CardStats cardStats)
        {
            OnCardLoaded?.Invoke(cardStats);
        }

 
        public static void AllCardsLoaded(List<CardStats> loadedCards)
        {
            OnAllCardsLoaded?.Invoke(loadedCards);
        }


        public static void DatabaseCreated()
        {
            IsDatabaseCreated = true;
            OnDatabaseCreated?.Invoke();
        }

        public static void GameEnded()
        {
            OnGameEnded?.Invoke();
        }
        
        // Player functions

        public static void PlayerCardDealt(Card card)
        {
            OnPlayerCardDealt?.Invoke(card);
            OnEnvironmentChange?.Invoke();
            // GetMethodNames(OnPlayerCardPlayed);
        }

        
        public static void PlayerCardPlayed(Card card)
        {
            OnPlayerCardPlayed?.Invoke(card);
            OnEnvironmentChange?.Invoke();
            // GetMethodNames(OnPlayerCardPlayed);
            // Debug.Log(cardPlayedStr);
        }
       

        public static void PlayerTurnEnded()
        {
            OnPlayerTurnEnd?.Invoke();
            OnPlayerTurnEndAfter?.Invoke();
            OnEnvironmentChange?.Invoke();
            // GetMethodNames(OnPlayerCardPlayed);
        }

        // Opponent Events
        public readonly static UnityEvent<Card> OnOpponentCardDealt = new UnityEvent<Card>();
        public readonly static UnityEvent<Card> OnOpponentCardPlayed = new UnityEvent<Card>();
        public readonly static UnityEvent<Card> OnCardRemovedFromTable = new UnityEvent<Card>();
        public readonly static UnityEvent OnOpponentTurnEnd = new UnityEvent();
        public readonly static UnityEvent OnOpponentTurnEndAfter = new UnityEvent();
        
        // Opponent functions
        public static void OpponentCardDealt(Card card)
        {
            OnOpponentCardDealt?.Invoke(card);
            OnEnvironmentChange?.Invoke();
        }

        public static void OpponentCardPlayed(Card card)
        {
            OnOpponentCardPlayed?.Invoke(card);
            OnEnvironmentChange?.Invoke();
        }

        public static void OpponentTurnEnded()
        {
            OnOpponentTurnEnd?.Invoke();
            OnOpponentTurnEndAfter?.Invoke();
            OnEnvironmentChange?.Invoke();
        }
    }