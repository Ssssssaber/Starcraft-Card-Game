using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DefaultNamespace;
using DefaultNamespace.Factory;
using Menu;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using static GameUtilities;


public class DeckBehaviour : MonoBehaviour
{
    public bool OpponentCardsVisible = false;
    [SerializeField] private int _deckSize;
    public List<CardStats> Deck = new List<CardStats>();

    public Team Team;
    private PlayerManager player;

    private HandBehaviour attachedHand;

    [SerializeField] private GameObject[] DeckIndicators;

    [SerializeField] private CardStats _topCardStats;
    [SerializeField] private CreatureCard _creatureCardPrefab;
    [SerializeField] private SpellCard _spellCardPrefab;

    private AbstractFactory _factory;
    
    public string deckString;
    public string str;
    public bool CreateStringDeck;

    private BoardAwareness _awareness;

    private int _maxCardAmount = MAX_HAND_CAPACITY;
    private int _startCardAmount = START_CARD_COUNT;

    public int CurrentSize
    {
        get { return _size; }
        set
        {
            _size = value;

            if (_size > 1)
            {
                _topCardStats = Deck[_size - 1];
            }
            else
            {
                Destroy(gameObject, 0.1f);
            }

            switch (_size)
            {
                case (> 15):
                    DeckIndicators[3].SetActive(false);
                    break;
                case (> 10):
                    DeckIndicators[2].SetActive(false);
                    break;
                case (> 5):
                    DeckIndicators[1].SetActive(false);
                    break;
                case (> 0):
                    DeckIndicators[0].SetActive(false);
                    break;

            }
        }
    }

    private int _size;

    public void Awake()
    {
        _awareness = BoardAwareness.Instance;
        player = GetComponentInParent<PlayerManager>();
        EventManager.OnDatabaseCreated.AddListener(CreateDeck);
    }

    private void CreateDeck()
    {
        attachedHand = player.Hand;
        if (Team == Team.Player)
        {
            EventManager.OnPlayerTurnEnd.AddListener(Draw);
        }
        else
        {
            EventManager.OnOpponentTurnEnd.AddListener(Draw);
        }
        
        if (player.isManuallyControlled && OptionStats.UseDeckCode && OptionStats.DeckCode.Length != 0)
        {
            str = OptionStats.DeckCode;
            CreateDeckFromString(str);    
        }
        else
        {
            CreateRaceDeck();
        }
        // if (Team == Team.Player)
        // {
        //     EventManager.OnPlayerTurnEnd.AddListener(Draw);
        //     attachedHand = _awareness.PlayerHand;

        //     if (OptionStats.UseDeckCode && OptionStats.DeckCode.Length != 0)
        //     {
        //         str = OptionStats.DeckCode;
        //         CreateDeckFromString(str);
        //     }
        //     else
        //     {
        //         CreateRaceDeck();
        //     }
        // }
        // else
        // {
        //     EventManager.OnOpponentTurnEnd.AddListener(Draw);
        //     attachedHand = _awareness.OpponentHand;
        //     CreateRaceDeck();
        // }
        
        // Debug.Log($"{Team} {Deck.Count}");
        StartCoroutine("StartGame");
    }

    public List<CardStats> CreateRngDeck()
    {
        List<CardStats> tempDeck = new List<CardStats>();
        
        for (int i = 0; i < _deckSize; i++)
        {
            int randIndex = Random.Range(0, CardDatabase.CardsList.Count);
            // if (CardDatabase.CardsList[randIndex].Race == Race)
            tempDeck.Add(CardDatabase.CardsList[randIndex]);
        }

        return tempDeck;
    }

    public void CreateRaceDeck()
    {   
        // Задание типа фабрики
        switch (player.Race)
        {
            case (Race.Protoss):
                AddCardsFromList(CardDatabase.ProtossCards, 2);
                _factory = new ProtossCardFactory();
                break;
            case (Race.Zerg):
                AddCardsFromList(CardDatabase.ZergCards, 2);
                _factory = new ZergCardFactory();
                break;
            case (Race.Terran):
                Debug.Log("hue");
                throw new Exception("hue");
        }
        ShuffleDeck();
    }

    private void AddCardsFromList(List<CardStats> cardStatsList, int dublicateRate)
    {
        foreach (var cardStats in cardStatsList)
        {
            
            // Debug.Log(Team + " Adding");
            for (int i = 0; i < dublicateRate; i++)
                Deck.Add(cardStats);
        }
        
        CurrentSize = Deck.Count;
    }

    public void ShuffleDeck()
    {
        CardStats container;
        // Debug.Log((int)System.DateTime.Now.Ticks);
        for (int i = 0; i < CurrentSize; i++)
        {
            container = Deck[i];
            int randIndex = Random.Range(i, CurrentSize);
            Deck[i] = Deck[randIndex];
            Deck[randIndex] = container;
            // checkArr = Deck.Select(i => i.id).ToArray();
            // Debug.Log(PrintArray(checkArr));
        }
    }


    // This will get the hang of the draw 
    IEnumerator StartGame()
    {
        while (attachedHand.CardsList.Count < _startCardAmount)
        {
            yield return new WaitForSeconds(0.3f);
            // Debug.Log($"{Team} {Deck.Count}");
            Draw();
        }

        EventManager.PlayerTurnStarted();
    }

    public void DrawCards(int amount)
    {
        StartCoroutine(DrawCardRoutine(amount));
    }

    private IEnumerator DrawCardRoutine(int amount)
    {
        // int count = 0;
        
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(0.3f);
            Draw();
            // count += 1;
        }
        // Debug.Log($"Draw {count} cards");
    }

    private void Draw()
    {
        if (attachedHand.CardsList.Count < _maxCardAmount)
        {
            if (_topCardStats.Type == CardType.Creature)
            {
                CreatureCard newCard = _factory.CreateCreatureCard(player);
                CardDealt(newCard);
            }
            else if (_topCardStats.Type == CardType.Spell)
            {
                SpellCard newCard = _factory.CreateSpellCard(player);
                CardDealt(newCard);
            }
        }
        else
        {
            Debug.Log($"Card burned{_topCardStats.PrintStats()}");
        }
        CurrentSize -= 1;
        Deck.Remove(_topCardStats);
    }

    private void CardDealt(Card newCard)
    {
        newCard.ApplyStats(_topCardStats);
        newCard.transform.SetParent(attachedHand.transform, false);
        newCard.State = CardState.InHand;
        
        if (!player.isManuallyControlled)
        {
            newCard.IsDraggable = false;
            newCard.SubscribeToPriorityUpdate();
            newCard.SetGoapGOVisible(true);
        }
        else
        {
            newCard.IsDraggable = true;
        }

        if (Team == Team.Opponent)
        {
            newCard.FaceCardDown(OpponentCardsVisible);
            newCard.Team = Team.Opponent;
            EventManager.OpponentCardDealt(newCard);

        }
        else if (Team == Team.Player)
        {
            newCard.Team = Team.Player;
            EventManager.PlayerCardDealt(newCard);
        }

        // if (Team == Team.Opponent)
        // {
        //     newCard.FaceCardDown(OpponentCardsVisible);
        //     newCard.IsDraggable = false;
        //     newCard.Team = Team.Opponent;
        //     newCard.SubscribeToPriorityUpdate();
        //     newCard.SetGoapGOVisible(true);
        //     EventManager.OpponentCardDealt(newCard);

        // }
        // else if (Team == Team.Player)
        // {
        //     newCard.Team = Team.Player;
        //     EventManager.PlayerCardDealt(newCard);
        // }
    }


    public void CreateDeckFromString(String deckString)
    {
         switch (player.Race)
        {
            case (Race.Protoss):
                _factory = new ProtossCardFactory();
                break;
            case (Race.Zerg):
                _factory = new ZergCardFactory();
                break;
            case (Race.Terran):
                Debug.Log("hue");
                throw new Exception("hue");
        }
        
        deckString = DecodeAsci(deckString);
        deckString = deckString.Remove(deckString.Length - 1);
        
        // if (Deck.Count != 0) Deck = new List<CardStats>();
        
        int[] ints = deckString.Split(';').Select(int.Parse).ToArray();
        
        foreach (int id in ints)
        {
            if (CardDatabase.CardsDict.ContainsKey(id))
            {
                Deck.Add(CardDatabase.CardsDict[id]);
                // Debug.Log(Team + " Adding");
            }
        }
        CurrentSize = Deck.Count;

        ShuffleDeck();
    }
    

}
