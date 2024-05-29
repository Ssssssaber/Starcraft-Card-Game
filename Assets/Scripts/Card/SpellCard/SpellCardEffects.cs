
using UnityEngine;
public class SpellCardEffects : MonoBehaviour
{
    private PlayEffect _playEffect;
    private TurnEffect _turnEffect;

    [SerializeField] private SpellCard _card;
    private void Start()
    {
        _card = GetComponent<SpellCard>();

        if (_card != null)
        {
            _card.CardPlayed.AddListener(ImplementOnPlayEffect);
            _card.CardPlayed.AddListener(ImplementOnTurnEffect);
        }
    }

    public void LoadCardEffects(string Name)
    {
        if (_card != null)
        {
            _playEffect = new PlayEffect(_card);
            _turnEffect = new TurnEffect(_card);
        }
    }

    

    public void ImplementOnPlayEffect()
    {
        _playEffect.ImplementMethod();
        DestroySpellCard();
    }
     
    public void ImplementOnTurnEffect()
    {
        if (_turnEffect != null)
        {
            TurnEffectManager manager = TurnSystem.instance.turnEffectManager;            
            if (_card.Team == Team.Opponent)
            {
                manager.AddOpponentTurnEffect(_turnEffect);
            }
            else
            {
                manager.AddOpponentTurnEffect(_turnEffect);
            }
        }
        DestroySpellCard();
    }

    private void DestroySpellCard()
    {
        Destroy(_card.gameObject);
    }

}
