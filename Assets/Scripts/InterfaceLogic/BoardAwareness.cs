using UnityEngine;
using UnityEngine.Serialization;
public class BoardAwareness : MonoBehaviour
{
    public static BoardAwareness awareness
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
    
    private void Awake()
    {
        SetSingletone();
    }

    private void SetSingletone()
    {
        if (awareness == null)
        {
            awareness = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
}