using System;
using UnityEngine;
using UnityEngine.Events;

namespace EquipmentSystem
{
    public class Flashlight : Item
    {
        private bool isOn = false;
        
        [SerializeField] private UnityEvent<bool> flashlightEvent;

        public override void Awake()
        {
            base.Awake();
            flashlightEvent.Invoke(isOn);
        }

        public override void Equip()
        {
            base.Equip();
            Debug.Log("Flashlight equipped.");
        }

        public override void Unequip()
        {
            base.Unequip();
            Debug.Log("Flashlight unequipped.");
        }

        public override void Use()
        {
            isOn = !isOn;
            flashlightEvent.Invoke(isOn);
            Debug.Log(isOn ? "Flashlight turned on." : "Flashlight turned off.");
        }
        
        public override void StoppedUse()
        {
            
        }

        public override void SecondaryUse()
        {
            Debug.Log("Flashlight secondary use.");
        }
    }
}