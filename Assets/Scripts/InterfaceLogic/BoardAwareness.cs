using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class BoardAwareness : MonoBehaviour
{
    public static BoardAwareness Instance
    {
        get;
        private set;
    }

    public Canvas MainCanvas;
    public PlayerManager player;
    public PlayerManager opponent;
    public AttackCursor AttackCursor;

    public CreatureCard SelectedCard;
    public CreatureCard TargetCard;

    public TurnSystem TurnSystem;

    private void Awake()
    {
        
        SetSingletone();
    }

    private void Start()
    {
        // EventManager.OnGameStarted.AddListener(StartGame);
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