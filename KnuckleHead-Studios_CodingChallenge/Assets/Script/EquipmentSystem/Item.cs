using System;
using System.Collections.Generic;
using InteractableSystem;
using UnityEngine;
using UnityEngine.Events;

namespace EquipmentSystem
{
    /// <summary>
    /// Base class for all items in the game.
    /// </summary>
    public abstract class Item : MonoBehaviour
    {
        private Interactable interactableObject; // The interactable object associated with the item
        internal Rigidbody _rigidbody;
        internal List<Collider> _colliders;
        [SerializeField] private Vector3 positionOffset;

        [SerializeField]
        private Vector3 rotationOffset; // The offset of the equip rotation relative to the origin point
        
        [SerializeField] private UnityEvent onEquip; // Event that is invoked when the item is equipped
        [SerializeField] private UnityEvent onUnEquip; // Event that is invoked when the item is unequipped

        public Vector3 PositionOffset
        {
            get => positionOffset;
            private set => positionOffset = value;
        }

        public Vector3 RotationOffset
        {
            get => rotationOffset;
            private set => rotationOffset = value;
        }

        public virtual void Awake()
        {
            // Get the Interactable component from the GameObject
            interactableObject ??= GetComponent<Interactable>();
            
            _colliders = new List<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
            
            // Get all colliders in the children of the throwable object
            var childrenColliders = GetComponentsInChildren<Collider>();
            var parentCollider = GetComponents<Collider>();
            
            // Combine the parent and children colliders and store them in _colliders
            _colliders.AddRange(parentCollider);
            _colliders.AddRange(childrenColliders);
            
            // filter out the colliders that are triggers
            _colliders = _colliders.FindAll(col => !col.isTrigger);
        }

        public virtual void Equip() // Equip the item
        {
            interactableObject ??= GetComponent<Interactable>();
            interactableObject.ColliderSetActive(false);
            interactableObject.RigidbodySetDiabled(true);
            interactableObject.DisablePromptCanvas();
            SetCollidersActive(false);
            
            onEquip.Invoke();
        }

        public virtual void Unequip() // Unequip the item
        {
            interactableObject ??= GetComponent<Interactable>();
            interactableObject.ColliderSetActive(true);
            interactableObject.RigidbodySetDiabled(false);
            interactableObject.EnablePromptCanvas();
            SetCollidersActive(true);
            
            onUnEquip.Invoke();
        }
        
        private void SetCollidersActive(bool _active)
        {
            if(_colliders == null) return;
            foreach (var col in _colliders)
            {
                col.enabled = _active;
            }
        }

        public abstract void Use(); // Use the item

        public abstract void StoppedUse(); // Stop using the item

        public abstract void SecondaryUse(); // Use the item in a secondary way
    }
}