using System.Reflection;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using static GameUtilities;
using TMPro;
using Unity.IO.LowLevel.Unsafe;

public abstract class Card : MonoBehaviour
{
    public int ID;
    public string Name;
    public string Desc;

    public bool LoadCardFromId = false;

    public ManaComponent ManaComponent;

    public AttackComponent AttackComponent;

    public Sprite Image;

    [SerializeField] private GameObject goapGO;
    public void SetGoapGOVisible(bool visible)
    {
        goapGO.SetActive(visible);
    }
    
    [SerializeField] private TextMeshProUGUI _goapPriority;
    public int PlayPriority
    {
        get
        {
            if (_goapPriority == null)
            {
                _goapPriority = goapGO.GetComponentInChildren<TextMeshProUGUI>();
            }
            _priority = CalculatePlayPrioririty();
            _goapPriority.text = _priority.ToString(); 
            return _priority;
        }
        set
        {
            _priority = value;
        }
    }

    private int _priority = -1;
    public PlayerManager ownerPlayer;
    [field: SerializeField] public Team Team { get; set; }
    [field: SerializeField] public CardState State { get; set; }
    
    [field: SerializeField] public CardType Type { get; private set; }
    [field: SerializeField] public PlayStyle playStyle { get; private set; }
    
    public bool IsDraggable = true;

    public UnityEvent CardPlayed = new UnityEvent();
    
    private void Awake()
    {
        State = CardState.InDeck;
    }

    private void Start()
    {
        if (LoadCardFromId && ID != 0)
        {
            ApplyStats(CardDatabase.CardsList[ID - 1]);
        }

        ManaComponent = GetComponent<ManaComponent>();
        AttackComponent = GetComponent<AttackComponent>();
        // SubscribeToPriorityUpdate();
        
    }

    public virtual void ApplyStats(CardStats cardStats)
    {
        ID = cardStats.id;
        Name = cardStats.Name;
        Desc = cardStats.Desc;
        Type = cardStats.Type;
        playStyle = cardStats.PlayStyle;
        
        var tempImage = Resources.Load<Sprite>("Images/" + Name);
        Image = tempImage != null ? tempImage : BASE_CARD_IMAGE;
    }

    public void SubscribeToPriorityUpdate()
    {
        if (Team == Team.Opponent)
        {
            EventManager.OnEnvironmentChange.AddListener(CalcPrirorityEvent);
        }
    }

    public void CalcPrirorityEvent()
    { 
        // PlayPriority = CalculatePlayPrioririty();
        int temp = PlayPriority;
    }

    
    public abstract int CalculatePlayPrioririty();

    public abstract void FaceCardDown(bool down);


    public virtual void CloneCard(Card card)
    {
        if (card.Type != this.Type)
        {
            return;
        }
        
        ID = card.ID;
        Name = card.Name;
        Desc = card.Desc;
        Image = card.Image;
    }

    public virtual void CardDied()
    {
        Destroy(this.gameObject, 0.5f);
    }
}

