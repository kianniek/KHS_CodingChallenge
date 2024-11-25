using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace InteractableSystem
{
    public class TriggerInteractable : Interactable
    {
        [SerializeField] private UnityEvent onTriggerInteracted;

        private bool trigger = false;
        
        public override void Interact(PlayerInputHandler playerInputHandler)
        {
            base.Interact(playerInputHandler);
            trigger = true;
            StartCoroutine(InvokeWhileTriggered());
        }
        
        IEnumerator InvokeWhileTriggered()
        {
            while (trigger)
            {
                onTriggerInteracted.Invoke();
                trigger = false;
                yield return null;
            }
        }
    }
}