using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.State
{
    public class EmptyState : HandState
    {
        public override void Init(HandBehaviour attachedHand)
        {
            Image background = attachedHand.GetComponent<Image>();

            if (background != null)
            {
                background.color = Color.white;
            }
        }

        public override void Handle(HandBehaviour attachedHand)
        {
            
            if (attachedHand.GetCardsCount() > 0)
            {
                attachedHand.State = new StandardState();
            }
        }
    }
}