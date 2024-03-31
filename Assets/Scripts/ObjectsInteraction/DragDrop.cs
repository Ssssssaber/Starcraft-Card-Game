
using UnityEngine;
using UnityEngine.Serialization;

public class DragDrop : MonoBehaviour
{
    public GameObject Canvas;
    public PlayerManaComponent playerManaComponent;
    // public PlayerManager player;
    private Card _card;
    
    [SerializeField] private bool _isDragging = false;
    
    [Header("In hand parameters")]
    private GameObject _startParent; 
    private Vector2 _startPosition;
    private GameObject _dropZone;
    private bool _isOverDropZone;
    private BoardAwareness _awareness = BoardAwareness.awareness;
   
    // Start is called before the first frame update
    private void Start()
    {
        _awareness = BoardAwareness.awareness;
        _card = GetComponent<Card>();
        Canvas = _awareness.MainCanvas.gameObject;

        // if (_card.Team == Team.Opponent)
        // {
        //     playerManaComponent = _awareness.OpponentManaComponent;
        //     _card.IsDraggable = false;
        // }
        
        // if (_card.Team == Team.Player)
        // {
        //     playerManaComponent = _awareness.PlayerManaComponent;
        // }

        playerManaComponent = _card.ownerPlayer.ManaComponent;
        if (!_card.ownerPlayer.isManuallyControlled)
        {
            _card.IsDraggable = false;
        }
    }
    
    public void StartDrag()
    {
        if (_card.State == CardState.InHand && _card.IsDraggable)
        {
            _isDragging = true;
            _startParent = transform.parent.gameObject;
            _startPosition = transform.position;
        }
    }

    public void Drag()
    {      
        if (_isDragging && _card.IsDraggable)
        {
            transform.position = Input.mousePosition;
            transform.SetParent(Canvas.transform, true);
        }
    }

    public void EndDrag()
    {   
        if (_card.IsDraggable)
        {
            _isDragging = false;

            if (_isOverDropZone && playerManaComponent.currentMana >= _card.ManaComponent.Mana)
            {
                if (_card.Type == CardType.Creature)
                {
                    transform.SetParent(_dropZone.transform, false);
                }
                else
                {
                    SpellCard card = _card.GetComponent<SpellCard>();
                    
                }

                               
                _card.IsDraggable = false;
                _card.State = CardState.OnTable;
                
                EventManager.PlayerCardPlayed(_card);
                _card.CardPlayed?.Invoke();
            }
            else
            {
                transform.position = _startPosition;
                transform.SetParent(_startParent.transform, false);
                
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isOverDropZone = true;
        _dropZone = collision.gameObject;
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        _isOverDropZone = false;
        _dropZone = null;
    }
    
}
