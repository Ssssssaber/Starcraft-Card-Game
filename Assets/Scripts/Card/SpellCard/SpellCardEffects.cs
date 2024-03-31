
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
            if (_card.Team == Team.Opponent)
            {
                TurnSystem.instance.AddOpponentTurnEffect(_turnEffect);
            }
            else
            {
                TurnSystem.instance.AddPlayerTurnEffect(_turnEffect);
            }
        }
        DestroySpellCard();
    }

    private void DestroySpellCard()
    {
        Destroy(_card.gameObject);
    }

}
