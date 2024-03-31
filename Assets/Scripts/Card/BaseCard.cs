// using DefaultNamespace;
// using Interfaces;
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.Serialization;
// using static GameConstants;
//
// public class BaseCard : MonoBehaviour
// {
//     [SerializeField] private CardStats _cardStats;
//     
//     public string Name;
//     public string Desc;
//
//     public HealthComponent HealthComponent;
//     public ManaComponent ManaComponent;
//     public AttackComponent AttackComponent;
//
//     public Sprite Image;
//     public GameObject backImage;
//     public GameObject AttackIndicator;
//
//     public Team Team { get; set; }
//     public CardState State { get; set; }
//     public CardType Type { get; set; }
//     
//     public bool CanAttack
//     {
//         get
//         {
//             return _canAttack;
//         }
//         set
//         {
//             _canAttack = value;
//             if(AttackIndicator != null)
//                 AttackIndicator.SetActive(_canAttack);
//         }
//     }
//
//     [FormerlySerializedAs("canAttack")] public bool _canAttack = true;
//     public bool IsDraggable = true;
//
//     public CardDisplay CardDisplay;
//
//     public void AttackCard(Card otherCard)
//     {
//         IHealth cardHealth = GetComponent<IHealth>();
//         IHealth otherCardHealth = otherCard.GetComponent<IHealth>();
//         
//         if (otherCard.Team != Team && CanAttack)
//         {
//             otherCardHealth.Damage(AttackComponent.Attack);
//             cardHealth.Damage(otherCard.AttackComponent.Attack);
//             CanAttack = false;
//         }
//     }
//
//     public void AttackHero(HeroBehaviour hero)
//     {
//         IHealth heroHealth = hero.gameObject.GetComponent<IHealth>();
//         if (hero.Team != Team && CanAttack && heroHealth != null)
//         {
//             heroHealth.Damage(AttackComponent.Attack);
//         }
//     }
//
//     private void Awake()
//     {
//         Team = Team.Player;
//         State = CardState.InDeck;
//     }
//
//     private void Start()
//     {
//         HealthComponent = GetComponent<HealthComponent>();
//         ManaComponent = GetComponent<ManaComponent>();
//         AttackComponent = GetComponent<AttackComponent>();
//         
//         HealthComponent.OnDie.AddListener(RemoveFromTable);
//         HealthComponent.OnDie.AddListener(CardDied);
//         CardDisplay = GetComponent<CardDisplay>();
//     }
//
//     public void ApplyStats(CardStats cardStats)
//     {
//         Name = cardStats.Name;
//         Desc = cardStats.Desc;
//         Type = cardStats.Type;
//         var tempImage = Resources.Load<Sprite>("Images/" + Name);
//         Image = tempImage != null ? tempImage : BASE_CARD_IMAGE;
//
//         ManaComponent.SetupMana(cardStats.ManaCost);
//         AttackComponent.SetupAttack(cardStats.Attack);
//         HealthComponent.SetupHealth(cardStats.Health);
//         CardDisplay.ApplyStats();
//     }
//
//     public void RemoveFromTable()
//     {
//         EventManager.OnCardRemovedFromTable.Invoke(this);
//     }
//
//     public void CardDied()
//     {
//         Destroy(this.gameObject, 0.5f);
//     }
// }
//
