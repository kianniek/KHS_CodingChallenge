using System.Collections.Generic;
using UnityEngine;

namespace EquipmentSystem
{
    public class HeadWearable : Item
    {
        public override void Awake()
        {
            base.Awake();
            _rigidbody = GetComponent<Rigidbody>();
            _colliders = new List<Collider>();
            
            // Get all colliders in the children of the throwable object
            var childrenColliders = GetComponentsInChildren<Collider>();
            var parentCollider = GetComponents<Collider>();
            
            // Combine the parent and children colliders and store them in _colliders
            _colliders.AddRange(parentCollider);
            _colliders.AddRange(childrenColliders);
            
            // filter out the colliders that are triggers
            _colliders = _colliders.FindAll(col => !col.isTrigger);
        }
        public override void Equip()
        {
            base.Equip();
            SetCollidersActive(false);
            Debug.Log("Hat equipped on head.");
        }

        public override void Unequip()
        {
            base.Unequip();
            SetCollidersActive(true);
            Debug.Log("Hat unequipped.");
        }

        public override void Use()
        {
            Debug.Log("Hat cannot be used.");
        }

        public override void SecondaryUse()
        {
            Debug.Log("Hat cannot be used in a secondary way.");
        }
        
        public override void StoppedUse()
        {
            
        }
        
        private void SetCollidersActive(bool _active)
        {
            foreach (var col in _colliders)
            {
                col.enabled = _active;
            }
        }
    }
}