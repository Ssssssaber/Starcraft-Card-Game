using UnityEngine;

namespace DefaultNamespace.State
{
    public abstract class HandState 
    {
        
        public abstract void Init(HandBehaviour attachedHand);
        public abstract void Handle(HandBehaviour attachedHand);
    }
}