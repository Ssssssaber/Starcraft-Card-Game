using UnityEngine.UI;
using UnityEngine;

namespace DefaultNamespace.State
{
    public class StandardState : HandState
    {
        public override void Init(HandBehaviour attachedHand)
        {
            Image background = attachedHand.GetComponent<Image>();

            if (background != null)
            {
                background.color = Color.cyan;
            }
        }

        public override void Handle(HandBehaviour attachedHand)
        {
            
            if (attachedHand.GetCardsCount() > 5)
            {
                attachedHand.State = new FullState();
            }
            else if (attachedHand.GetCardsCount() == 0)
            {
                attachedHand.State = new EmptyState();
            }
        }
    }
}