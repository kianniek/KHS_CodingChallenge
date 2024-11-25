using System.Linq;
using EquipmentSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractableSystem
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] InteractionGlobalSettings interactionGlobalSettings;
        private EquipmentSystemManager equipmentSystem;
        private InputSystem_Actions inputActions;

        private bool isLeftHandUseHeld = false;
        private bool isRightHandUseHeld = false;

        void Awake()
        {
            equipmentSystem = GetComponent<EquipmentSystemManager>();
            inputActions = new InputSystem_Actions(); // The generated class from the Input Actions asset
        }

        void OnEnable()
        {
            inputActions.Enable();
        }

        void OnDisable()
        {
            inputActions.Disable();
        }

        void Start()
        {
            // Bind actions to methods
            inputActions.Player.UseLeftHand.performed += ctx => UseItem(EquipmentSlot.LeftHand);
            inputActions.Player.UseLeftHand.canceled += ctx => StopUseItem(EquipmentSlot.LeftHand);  // To handle release
            inputActions.Player.UseRightHand.performed += ctx => UseItem(EquipmentSlot.RightHand);
            inputActions.Player.UseRightHand.canceled += ctx => StopUseItem(EquipmentSlot.RightHand);  // To handle release

            inputActions.Player.Interact.performed += ctx => Interact();
            inputActions.Player.Crouch.performed += ctx => UnequipItem(EquipmentSlot.Head);
            inputActions.Player.UnEquip.performed += ctx => UnequipItem(EquipmentSlot.RightHand);
            inputActions.Player.SecondaryUseRightHand.performed += ctx => SecondaryUse(EquipmentSlot.RightHand);
        }

        private void Update()
        {
            // Check if buttons are being held down
            if (inputActions.Player.UseLeftHand.ReadValue<float>() > 0)
            {
                if (!isLeftHandUseHeld)
                {
                    // Handle the start of holding down the button (if needed)
                    isLeftHandUseHeld = true;
                }
            }
            else
            {
                if (isLeftHandUseHeld)
                {
                    // Handle the release of holding the button
                    isLeftHandUseHeld = false;
                }
            }

            if (inputActions.Player.UseRightHand.ReadValue<float>() > 0)
            {
                if (!isRightHandUseHeld)
                {
                    // Handle the start of holding down the button (if needed)
                    isRightHandUseHeld = true;
                }
            }
            else
            {
                if (isRightHandUseHeld)
                {
                    // Handle the release of holding the button
                    isRightHandUseHeld = false;
                }
            }
        }

        // This method is called when an interactable object is picked up and equipped
        public bool EquipInteractableItem(EquipmentSlot slot, GameObject interactableObject)
        {
            return equipmentSystem.EquipItem(slot, interactableObject);
        }

        public bool UnequipItem(EquipmentSlot slot)
        {
            return equipmentSystem.UnequipItem(slot);
        }

        private void UseItem(EquipmentSlot slot)
        {
            equipmentSystem.UseItem(slot);
        }
        
        private void SwapPrimarySecondaryHands()
        {
            equipmentSystem.SwapPrimarySecondaryHands();
        }

        private void StopUseItem(EquipmentSlot slot)
        {
            // Handle what happens when the use button is released
            Debug.Log($"{slot} use action stopped.");
            equipmentSystem.StopUseItem(slot);
        }

        private void SecondaryUse(EquipmentSlot slot)
        {
            equipmentSystem.SecondaryAction(slot);
        }

        private void Interact()
        {
            Debug.Log("Interacting with object...");
            Interactable interactable = GetClosestInteractable();
            if (interactable != null)
            {
                interactable.Interact(this);
            }
        }

        private Interactable GetClosestInteractable()
        {
            RaycastHit hit;
            Collider[] colliders =
                Physics.OverlapSphere(transform.position, interactionGlobalSettings.interactionDistance);

            // Sort the colliders by distance to the player
            colliders = colliders.OrderBy(_interactable => (_interactable.transform.position - transform.position).sqrMagnitude).ToArray();

            foreach (Collider collider in colliders)
            {
                Interactable interactable = collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    return interactable;
                }
            }

            return null;
        }
    }
}
