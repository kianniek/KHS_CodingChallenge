using UnityEngine;
using UnityEngine.Events;

namespace InteractableSystem
{
    public class ToggleInteractable : Interactable
    {
        [SerializeField] private UnityEvent<bool> onToggleInteracted;
        
        private bool isToggled = false;
        
        public override void Interact(PlayerInputHandler playerInputHandler)
        {
            base.Interact(playerInputHandler);
            isToggled = !isToggled;
            onToggleInteracted.Invoke(isToggled);
        }
    }
}