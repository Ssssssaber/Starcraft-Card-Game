using System;
using System.Collections.Generic;
using DatabaseLoader.Proxy;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
public class BoardAwareness : MonoBehaviour
{
    public static BoardAwareness Instance
    {
        get;
        private set;
    }

	[SerializeField] bool shuffleDecks;

    public Canvas MainCanvas;
    public PlayerManager player;
    public PlayerManager opponent;
    public AttackCursor AttackCursor;

    public CreatureCard SelectedCard;
    public CreatureCard TargetCard;

	public CardDisplayPlace cardDisplayPlace;
	public SelectionSystem selectionSystem;

	public float testWaitTime = 1.0f;

    public TurnSystem TurnSystem;
	public CardStatsManagerProxy cardStatsManagerProxy;

	private bool firstTime = true;

	public void Init(BoardPreset preset)
	{
		Init(preset.playerHeroId, preset.playerGOAP, preset.opponentHeroId, preset.opponentGOAP, preset.playerDeckCode, preset.opponentDeckCode, preset.encoded, shuffleDecks);
	}
	public void Init(int playerHeroID  = 0, bool playerGOAP=true, int opponentHeroID = 0, bool opponentGOAP = true, string playerDeck = "", string opponentDeck = "", bool encodedDeckCodes = false, bool shuffleDecks = false)
	{
		
		if (firstTime)
		{
			SetSingletone();
			cardStatsManagerProxy.InitCardLoader();
			firstTime = false;
		}

		OptionStats.PlayerDeckCode = playerDeck;
		OptionStats.OpponentDeckCode = opponentDeck;
		OptionStats.PlayerHero = playerHeroID;
		OptionStats.OpponentHero = opponentHeroID;
		OptionStats.encoded = encodedDeckCodes;
		OptionStats.shuffleDecks = shuffleDecks;
		
		player.ControlType = playerGOAP ? PlayerControl.GOAP : PlayerControl.Manual;
		opponent.ControlType = opponentGOAP ? PlayerControl.GOAP : PlayerControl.Manual;
		
		
		selectionSystem.Init();
		cardDisplayPlace.Init();
		
		TurnSystem.Init();
		
	}


    private void SetSingletone()
    {
        if (Instance == null)
        {
            Instance = this;
            if (player.ControlType == PlayerControl.ML || opponent.ControlType == PlayerControl.ML)
            {
                EventManager.isML = true;
            }
        }
        else
        {
            Destroy(this);
        }
    }


    public List<float> GetPlayerTableCards(PlayerManager player)
    {
        return player.GetTableCardIds();
    }
    
    public List<float> GetPlayerHandCards(PlayerManager player)
    {
        return player.GetHandCardIds();
    }
    
    public int GetPlayerTableCardsCount(PlayerManager player)
    {
        return player.GetTableCardsCount();
    }
    
    public int GetPlayerHandCardsCount(PlayerManager player)
    {
        return player.GetHandCardsCount();
    }

    public int GetPlayerManaPoints(PlayerManager player)
    {
        return player.GetManaPoints();
    }

}