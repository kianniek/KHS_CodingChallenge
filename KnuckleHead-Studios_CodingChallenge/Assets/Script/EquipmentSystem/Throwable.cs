using System;
using System.Collections.Generic;
using UnityEngine;

namespace EquipmentSystem
{
    public class Throwable : Item
    {
        [SerializeField] private float throwForce = 10f;
        
        private EquipmentSystemManager _equipmentSystemManager;
        private Camera _mainCamera;
        
        
        public override void Awake()
        {
            base.Awake();
            
            _mainCamera = Camera.main;
        }

        public override void Equip()
        {
            base.Equip();
            
            Debug.Log("Rock equipped.");
        }

        public override void Unequip()
        {
            base.Unequip();
            
            Debug.Log("Rock unequipped.");
        }

        public override void Use()
        {
            Debug.Log($"Throwing {gameObject.name}.");
            
            // Early return if _equipmentSystemManager is not available
            if (_equipmentSystemManager == null)
            {
                _equipmentSystemManager = FindFirstObjectByType<EquipmentSystemManager>();
                if (_equipmentSystemManager == null) return;
            }
            
            // Unequip the ammo clip after using it
            _equipmentSystemManager.UnequipItem(this);
            
            // Early return if _rigidbody is not available
            if (_rigidbody == null) return;
            
            // Throw the rock
            var horizontalForward = _mainCamera.transform.forward;
            
            //project the camera's forward vector onto the xz plane
            horizontalForward.y = 0;
            horizontalForward.Normalize();
            _rigidbody.AddForce(horizontalForward * 10, ForceMode.Impulse);
        }
        
        

        public override void SecondaryUse()
        {
            Debug.Log("Rock secondary use.");
        }
        
        public override void StoppedUse()
        {
            
        }
    }
}