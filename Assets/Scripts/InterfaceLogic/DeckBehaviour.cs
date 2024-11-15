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
using TMPro;


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

    [SerializeField] private TMP_Text sizeText;

    public int CurrentSize
    {
        get { return _size; }
        set
        {
            _size = value;
            sizeText.text = _size.ToString();

            // if (_size > 1)
            // {
            //     _topCardStats = Deck[_size - 1];
            // }
            // else
            // {
            //     // throw new Exception("deck count below zero");
            //     Debug.Log("deck count below zero");
            //     // Destroy(gameObject, 0.1f);
            // }

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
        player = GetComponentInParent<PlayerManager>();
        EventManager.OnDatabaseCreated.AddListener(Setup);
    }

    private void Setup()
    {
		_awareness = BoardAwareness.Instance;
        attachedHand = player.Hand;
        EventManager.OnDeckInitialized.AddListener(DeckInitilize);
        if (Team == Team.Player)
        {
            EventManager.OnPlayerTurnEnd.AddListener(Draw);
        }
        else
        {
            EventManager.OnOpponentTurnEnd.AddListener(Draw);
        }
    }

    private void DeckInitilize()
    {
		if (player.Team == Team.Player && OptionStats.PlayerDeckCode.Length != 0)
		{
			if (OptionStats.PlayerDeckCode == "random")
			{
				CreateRaceDeck();
				return;
			}
			str = OptionStats.PlayerDeckCode;
            CreateDeckFromString(str);    
		}
		else if (player.Team == Team.Opponent && OptionStats.OpponentDeckCode.Length != 0)
		{
			if (OptionStats.OpponentDeckCode == "random")
			{
				CreateRaceDeck();
				return;
			}
			str = OptionStats.OpponentDeckCode;
            CreateDeckFromString(str);    	
			
			
		}
        else
        {
            CreateRaceDeck();
        }
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

    private void ResetDeck()
    {
        Deck = new List<CardStats>();
        CurrentSize = 0;
    }

    // private void AddCard(CardStats card)
    // {
    //     Deck.Add(card);
    //     CurrentSize++;
    // }

    // private void RemoveCard(CardStats card)
    // {
    //     if (Deck.Contains(card) && CurrentSize > 0)    
    //     {
    //         Deck.Remove(card);
    //         CurrentSize--;
    //     }
    //     else if (CurrentSize == 0)
    //     {
    //         player.Hero.HealthComponent.Damage(1);
    //     }
    //     else
    //     {
    //         throw new Exception("suka na remove");
    //     }
    // }

    private CardStats DrawAndGetNext()
    {
        CardStats card = null;
        try 
        {
            if (Deck.Count <= 0)
            {
                throw new Exception("kekw deletion from deck");       
            }
            card = Deck[Deck.Count - 1];
            Deck.RemoveAt(Deck.Count - 1);
            
            CurrentSize = Deck.Count;
            
        }
        catch (Exception ex)
        {   
            Debug.Log(ex.Message);
        }
        return card;
        
        
        
    }

    private CardStats AddAndGetNext(CardStats card)
    {
        Deck.Add(card);
        CurrentSize = Deck.Count;
        return Deck[Deck.Count - 1];
    }

    public void CreateRaceDeck()
    {
        ResetDeck();
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
        if (OptionStats.shuffleDecks) ShuffleDeck();
    }

    private void AddCardsFromList(List<CardStats> cardStatsList, int dublicateRate)
    {
        foreach (var cardStats in cardStatsList)
        {
            for (int i = 0; i < dublicateRate; i++)
            {
                _topCardStats = AddAndGetNext(cardStats);
            }
        }
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
        }
    }


    // This will get the hang of the draw 
    IEnumerator StartGame()
    {
        while (attachedHand._cardsList.Count < _startCardAmount)
        {
            yield return new WaitForSeconds(GameUtilities.ACTION_WAIT_TIME);
            // Debug.Log($"{Team} {Deck.Count}");
            Draw();
        }

        if (Team == Team.Player)
        {
            EventManager.PlayerTurnStarted();
        }
        else
        {
            // EventManager.OpponentTurnStarted();
        }
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
            yield return new WaitForSeconds(GameUtilities.ACTION_WAIT_TIME);
            Draw();
            // count += 1;
        }
        // Debug.Log($"Draw {count} cards");
    }

    private void Draw()
    {
        // check if can draw
        if (Deck.Count <= 0)
        {
            player.Hero.HealthComponent.Damage(1);
            return;
        }

        // get next card
        _topCardStats = DrawAndGetNext();
        if (_topCardStats == null)
        {
            Debug.Log("bezdar"); 
            return;
        }
        // check if can get card
        if (attachedHand._cardsList.Count >= GameUtilities.MAX_HAND_CAPACITY)
        {
            Debug.Log($"Card burned{_topCardStats.PrintStats()}");
            return;
        }
        // handle card draw
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
        // if (attachedHand._cardsList.Count < _maxCardAmount && Deck.Count > 0)
        // {
        //     if (_topCardStats.Type == CardType.Creature)
        //     {
        //         CreatureCard newCard = _factory.CreateCreatureCard(player);
        //         CardDealt(newCard);
        //     }
        //     else if (_topCardStats.Type == CardType.Spell)
        //     {
        //         SpellCard newCard = _factory.CreateSpellCard(player);
        //         CardDealt(newCard);
        //     }
        // }
        // else
        // {
        //     Debug.Log($"Card burned{_topCardStats.PrintStats()}");
        // }
        // // CurrentSize -= 1;
        // // Deck.Remove(_topCardStats);
        // // RemoveCard(_topCardStats);
        // _topCardStats = DrawAndGetNext();
        // if (_topCardStats == null && Deck.Count == 0)
        // {
        //     player.Hero.HealthComponent.Damage(1);
        // }
    }

    private void DrawSafe()
    {
        // check if can draw
        if (Deck.Count < 0)
        {
            player.Hero.HealthComponent.Damage(1);
            return;
        }

        // get next card
        _topCardStats = DrawAndGetNext();
        if (_topCardStats == null)
        {
            Debug.Log("bezdar"); 
            return;
        }
        // check if can get card
        if (attachedHand._cardsList.Count >= GameUtilities.MAX_HAND_CAPACITY)
        {
            Debug.Log($"Card burned{_topCardStats.PrintStats()}");
        }
        // handle card draw
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

    private void CardDealt(Card newCard)
    {
        newCard.ApplyStats(_topCardStats);
        newCard.transform.SetParent(attachedHand.transform, false);
        newCard.State = CardState.InHand;
        
        if (player.ControlType != PlayerControl.Manual)
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
        
        if (OptionStats.encoded) deckString = DecodeAsci(deckString);
        deckString = deckString.Remove(deckString.Length - 1);
        
        // if (Deck.Count != 0) Deck = new List<CardStats>();
        
        int[] ints = deckString.Split(';').Select(int.Parse).ToArray();
        
        foreach (int id in ints)
        {
            if (CardDatabase.CardsDict.ContainsKey(id))
            {
                // AddCard(CardDatabase.CardsDict[id]);
                AddAndGetNext(CardDatabase.CardsDict[id]);
                // Debug.Log(Team + " Adding");
            }
        }

        if (OptionStats.shuffleDecks) ShuffleDeck();
    }
    

}
