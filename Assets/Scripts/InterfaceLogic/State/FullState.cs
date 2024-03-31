using DefaultNamespace.State;
using UnityEngine.UI;
using UnityEngine;
namespace DefaultNamespace.State
{
    public class FullState : HandState
    {
        public override void Init(HandBehaviour attachedHand)
        {
            Image background = attachedHand.GetComponent<Image>();

            if (background != null)
            {
                background.color = Color.red;
            }
        }

        public override void Handle(HandBehaviour attachedHand)
        {
            
            if (attachedHand.GetCardsCount() < 6)
            {
                attachedHand.State = new StandardState();
            }
        }
    }
}