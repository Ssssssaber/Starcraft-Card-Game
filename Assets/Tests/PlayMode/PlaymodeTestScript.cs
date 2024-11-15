using System.Collections;
using System.Collections.Generic;
using Interfaces;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlaymodeTestScript
{

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
	BoardPresetManager manager;
	BoardAwareness boardAwareness;
	PlayerManager player;
	PlayerManager opponent;
	TurnSystem ts;
	bool isInitialized = false;
	private float waitTime;
	[OneTimeSetUp]
	public void Setup()
	{
		SceneManager.LoadScene("Assets/Scenes/TestBoard.unity");
		Debug.Log("Scene loaded");
	}

	public void InitBoard()
	{
		GameObject startup = GameObject.Find("startup");
		manager = startup.GetComponent<BoardPresetManager>();
		manager.LoadJsonConfigs();
		
		GameObject boardGO = GameObject.Find("board");
		boardAwareness = boardGO.GetComponent<BoardAwareness>();

		waitTime = boardAwareness.testWaitTime;

		ts = boardAwareness.TurnSystem;
		
		player = boardAwareness.player;
		opponent = boardAwareness.opponent;

		GameUtilities.setActionWaitTime(0);
	}

	[UnityTest]
    public IEnumerator _0DeckTest()
	{
		string deckString = "2;2;2;2;5;5;5;7;7;10;12;12;12;12;";
		List<string> testNames = new List<string>();
		string[] resultNames = {"Zergling", "Zergling", "Zergling", "Hydralisk", "Hydralisk", "Hydralisk", "Hydralisk", "Mutalisk", "Mutalisk", "Mutalisk", "Roach", "Roach", "Ultralisk", "Zergling"};
        
        /*
        int playerHeroID  = 0, bool playerGOAP=true, int opponentHeroID = 0, 
        bool opponentGOAP = true, string playerDeck = "", string opponentDeck = "", 
        bool encodedDeckCodes = false, bool shuffleDecks = false
        */
        InitBoard();
		boardAwareness.Init(0, false, 0, false, deckString, deckString, false, false);
		EventManager.GameStarted();

		yield return new WaitForSeconds(3f);

        HandBehaviour hand = player.Hand;
        foreach (var card in hand.GetCardsList())
        {
            testNames.Add(card.Name);
        }

        DeckBehaviour deck = player.Deck;
        foreach (var card in deck.Deck)
        {
            testNames.Add(card.Name);
        }

        Debug.Log(GameUtilities.ArrayToString(testNames));
        Assert.AreEqual(resultNames, testNames);
        
		yield return new WaitForSeconds(3f);
	}


    [UnityTest]
    public IEnumerator _1BasicTest()
    {
		InitBoard();
		boardAwareness.Init(manager.GetPresetByName("earlyStart"));
		EventManager.GameStarted();

		yield return new WaitForSeconds(2);

		yield return new WaitForSeconds(waitTime);
		player.TryPlayCardById(0);

		var ts = boardAwareness.TurnSystem;
		ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);

		opponent.TryPlayCardById(0);

		yield return new WaitForSeconds(waitTime);
		Assert.AreEqual(player.Table.GetCardsList().Count, opponent.Table.GetCardsList().Count, "cards were not played");

		ts.EndOpponentTurn();
		yield return new WaitForSeconds(waitTime);
		
		player.TryPlayCardById(0);
		player.TryAttackTarget(0, 0);

		ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);
		opponent.TryPlayCardById(0);
		opponent.TryAttackTarget(0, 0);

		yield return new WaitForSeconds(waitTime);
		Debug.Log("skipping turns... 3");
		Assert.AreEqual(player.Table.GetCardsList().Count == (opponent.Table.GetCardsList().Count), true, "attack was not done");
		EventManager.GameEnded();
		yield return new WaitForSeconds(waitTime);
    }

	[UnityTest]
	public IEnumerator _2OnPlayEffectTest()
	{
		InitBoard();
		
		boardAwareness.Init(manager.GetPresetByName("playEffectPreset"));
		EventManager.GameStarted();

		var ts = boardAwareness.TurnSystem;

		yield return new WaitForSeconds(2);
		// play 3 zerglins
		player.TryPlayCardById(1);
		ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);

		opponent.TryPlayCardById(0);
		ts.EndOpponentTurn();
		yield return new WaitForSeconds(waitTime);

		player.TryPlayCardById(1);
		player.TryPlayCardById(1);
		ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);

		ts.EndOpponentTurn();
		yield return new WaitForSeconds(waitTime);

		player.TryPlayCardById(0);
		yield return new WaitForSeconds(waitTime);
		player.TryAttackTarget(0, 0);
		yield return new WaitForSeconds(waitTime);

		Assert.AreEqual(player.TableCardsCount() == 3, true);
		Assert.AreEqual(opponent.TableCardsCount() == 0, true);
		

		yield return null;
	}

	[UnityTest]
	public IEnumerator _3AttackEffectTest()
	{
		InitBoard();
		
		boardAwareness.Init(manager.GetPresetByName("attackEffectPreset"));
		EventManager.GameStarted();

		yield return new WaitForSeconds(2);

		var ts = boardAwareness.TurnSystem;
		for (int i = 0; i < 5; i++)
		{
			ts.EndPlayerTurn();
			yield return new WaitForSeconds(waitTime);
			ts.EndOpponentTurn();
			yield return new WaitForSeconds(waitTime);
		}

		Debug.Log("skipping turns... ");

		player.TryPlayCardById(0);
		Debug.Log("playing mutalisk");
		ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);
		

		opponent.TryPlayCardById(0);
		Debug.Log("playing hydralisk");
		ts.EndOpponentTurn();
		yield return new WaitForSeconds(waitTime);
		player.TryAttackTarget(0, 0);
		yield return new WaitForSeconds(waitTime);
		
		Debug.Log("check if hydralisk is dead");
		
		Assert.AreEqual(opponent.Table.GetCardsList().Count == 0, true);

	}

	[UnityTest]
	public IEnumerator _4TurnEffectTest()
	{
		InitBoard();
		
		boardAwareness.Init(manager.GetPresetByName("turnEffectPreset"));
		EventManager.GameStarted();

		var ts = boardAwareness.TurnSystem;
		for (int i = 0; i < 5; i++)
		{
			ts.EndPlayerTurn();
			yield return new WaitForSeconds(waitTime);
			ts.EndOpponentTurn();
			yield return new WaitForSeconds(waitTime);
		}

		for (int i = 0; i < 4; i++)
		{
			player.TryPlayCardById(1);
		}

		ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);
		
		opponent.TryPlayCardById(0);
		ts.EndOpponentTurn();
		yield return new WaitForSeconds(waitTime);
		
		player.TryPlayCardById(0);
		ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);

		ts.EndOpponentTurn();
		yield return new WaitForSeconds(waitTime);
		yield return new WaitForSeconds(1);
		bool pass = true;
		foreach (CreatureCard card in player.Table.GetCardsList())
		{
			IHealth cardHealth = card.GetComponent<IHealth>();
			if (cardHealth.Health != 4)
			{
				pass = false;
			}
		}
		Assert.AreEqual(true, pass);
	}

	[UnityTest]
	public IEnumerator _5OnDieEffectTest()
	{
		InitBoard();		
		boardAwareness.Init(manager.GetPresetByName("onDieEffectPreset"));
		EventManager.GameStarted();

		yield return new WaitForSeconds(2);

		ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);

		opponent.TryPlayCardById(0);
		ts.EndOpponentTurn();
		yield return new WaitForSeconds(waitTime);

		player.TryPlayCardById(0);
		ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);

		opponent.TryPlayCardById(0);
		opponent.TryPlayCardById(0);
		opponent.TryPlayCardById(0);
		ts.EndOpponentTurn();
		yield return new WaitForSeconds(waitTime);

		player.TryAttackTarget(0, 0);
		ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);

		Assert.AreEqual(player.TableCardsCount() == 0, true);
		Assert.AreEqual(opponent.TableCardsCount() == 0, true);
		
		yield return new WaitForSeconds(waitTime);
	}

	[UnityTest]
	public IEnumerator _6GoapCalcTest()
	{
		string pHand = "50; 5; 75";
		string oHand = "100; 50; 55";
		
		InitBoard();
		var s = manager.GetPresetByName("calcGoapPreset");

		player.AI.isAutomatic = false;
		opponent.AI.isAutomatic = false;

		boardAwareness.Init(s);
		EventManager.GameStarted();

        yield return new WaitForSeconds(2);
		
        player.TryPlayCardById(2);
		ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);
        EventManager.OnEnvironmentChange.Invoke();

		opponent.TryPlayCardById(2);
		ts.EndOpponentTurn();
		yield return new WaitForSeconds(waitTime);
        EventManager.OnEnvironmentChange.Invoke();

		player.TryPlayCardById(2);
		ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);
        EventManager.OnEnvironmentChange.Invoke();

		opponent.TryPlayCardById(2);
		opponent.TryAttackTarget(0, 1);
		ts.EndOpponentTurn();
		yield return new WaitForSeconds(waitTime);
        EventManager.OnEnvironmentChange.Invoke();

		EventManager.OnEnvironmentChange.Invoke();
		
		string testPHand = GameUtilities.ArrayToString(GetPrioritiesHand(player));
		string testOHand = GameUtilities.ArrayToString(GetPrioritiesHand(opponent));
		
		Debug.Log("Player Hand: " + testPHand);		
		Debug.Log("Opponent Hand: " + testOHand);

		Assert.AreEqual(pHand, testPHand);
		Assert.AreEqual(oHand, testOHand);

		yield return new WaitForSeconds(1f);
	}

	[UnityTest]
	public IEnumerator _7AttackTestGoap()
	{
		InitBoard();
		var s = manager.GetPresetByName("goapDecisionPreset");
        
		player.AI.isAutomatic = false;
		opponent.AI.isAutomatic = false;

        boardAwareness.Init(s);
		EventManager.GameStarted();

        yield return new WaitForSeconds(2);

        player.TryPlayCardById(0);
        ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);

        opponent.TryPlayCardById(0);
        ts.EndOpponentTurn();
		yield return new WaitForSeconds(waitTime);

        player.TryPlayCardById(0);
        player.TryPlayCardById(0);
        ts.EndPlayerTurn();
        yield return new WaitForSeconds(waitTime);
		
        opponent.TryPlayCardById(1);
        ts.EndOpponentTurn();

        player.TryPlayCardById(0);
        ts.EndPlayerTurn();
        yield return new WaitForSeconds(waitTime);

        opponent.TryPlayCardById(1);
        ts.EndOpponentTurn();
        yield return new WaitForSeconds(waitTime);

        player.TryPlayCardById(0);
        bool face = player.AI.ChooseRoundAction();

        Debug.Log(opponent.HeroHealth());

        Assert.AreEqual(opponent.HeroHealth(), 24);
        
        Assert.AreEqual(true, face);

        yield return new WaitForSeconds(3f);
	}

	[UnityTest]
	public IEnumerator _8DefenceTestGoap()
	{
		InitBoard();
		var s = manager.GetPresetByName("goapDefencePreset");
        
		player.AI.isAutomatic = false;
		opponent.AI.isAutomatic = false;

        boardAwareness.Init(s);
		EventManager.GameStarted();

        yield return new WaitForSeconds(2);

        player.TryPlayCardById(0);
        ts.EndPlayerTurn();
		yield return new WaitForSeconds(waitTime);

        opponent.TryPlayCardById(0);
        ts.EndOpponentTurn();
		yield return new WaitForSeconds(waitTime);

        player.TryPlayCardById(0);
        // player.TryPlayCardById(0);
        ts.EndPlayerTurn();
        yield return new WaitForSeconds(waitTime);
		
        opponent.TryPlayCardById(0);
        ts.EndOpponentTurn();

        // player.TryPlayCardById(0);
        ts.EndPlayerTurn();
        yield return new WaitForSeconds(waitTime);

        opponent.TryPlayCardById(0);
        ts.EndOpponentTurn();
        yield return new WaitForSeconds(waitTime);

        // player.TryPlayCardById(0);
        bool face = player.AI.ChooseRoundAction();

        // Debug.Log(opponent.HeroHealth());

        Assert.AreEqual(false, face);

        yield return new WaitForSeconds(3f);
	}

	[UnityTest]
	public IEnumerator _9TestGoap()
	{
        /*
        1
        Player (False): 10; 10
        Opponent (False): + -1; -1
        2
        Player (True): -1; -1
        Opponent (False): + -1; 100; 100
        3
        Player (False): -1; -1; 10; 10
        Opponent (False): + -1; 100; 10; 10
        4
        Player (True): -1; -1; 75; 75
        Opponent (True): + 30; 30
        5
        Player (False): -1; -1; 75; 30; 30
        Opponent (True): + 10; 10
        6
        Player (False): 5; 5; 30; 30; 25; 25
        Opponent (True): + 10; 10
        7
        Player (True): 10; 10; 25; 40; 40
        Opponent (False): + 55; 55
        */
        UnityEngine.Random.InitState(42);
        (bool, bool)[] desicionsPlayerOpponent = {
            (false, false), (true, false), 
            (false, false), (true, true), 
            (true, true), (false, true), 
            (false, false),
        };
		(string, string)[] prioritiesPlayerOpponent = {
            ("10; 10", "-1; -1"),
            ("-1; -1", "-1; 100; 100"),
            ("-1; -1; 10; 10", "-1; 100; 10; 10"),
            ("-1; -1; 75; 75", "30; 30"),
            ("-1; -1; 75; 30; 30", "10; 10"),
            ("5; 5; 30; 30; 25; 25", "10; 10"),
            ("10; 10; 25; 40; 40", "55; 55")
        };
        InitBoard();
		var s = manager.GetPresetByName("goapUsual");

		player.AI.isAutomatic = false;
		opponent.AI.isAutomatic = false;

		boardAwareness.Init(s);
		EventManager.GameStarted();

        yield return new WaitForSeconds(2);

		for (int i = 0; i < 7; i++)
		{
			player.AI.PlaceCards();
			yield return new WaitForSeconds(waitTime);
			bool face = player.AI.ChooseRoundAction();
			ts.EndPlayerTurn();
			yield return new WaitForSeconds(waitTime);
            string testHand = GameUtilities.ArrayToString(GetPrioritiesHand(player));
            Debug.Log($"Player ({face}): {testHand}");
            Assert.AreEqual(desicionsPlayerOpponent[i].Item1, face);
            Assert.AreEqual(prioritiesPlayerOpponent[i].Item1, testHand);

			opponent.AI.PlaceCards();
			yield return new WaitForSeconds(waitTime);
			face = opponent.AI.ChooseRoundAction();
			ts.EndOpponentTurn();
			yield return new WaitForSeconds(waitTime);
            testHand = GameUtilities.ArrayToString(GetPrioritiesHand(opponent));
            Debug.Log($"Opponent ({face}): + {testHand}");
            Assert.AreEqual(desicionsPlayerOpponent[i].Item2, face);
            Assert.AreEqual(prioritiesPlayerOpponent[i].Item2, testHand);
        }
		
		Assert.AreEqual(true, true);
		yield return new WaitForSeconds(1f);
	}

	private List<int> GetPrioritiesHand(PlayerManager manager)
	{
		List<int> res = new List<int>();

		foreach (Card card in manager.Hand.GetCardsList())
		{
			res.Add(card.PlayPriority);
		}

		return res;
	}
}
