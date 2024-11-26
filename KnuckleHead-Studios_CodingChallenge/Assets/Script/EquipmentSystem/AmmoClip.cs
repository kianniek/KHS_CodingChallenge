using System;
using EquipmentSystem;
using UnityEngine;

namespace EquipmentSystem
{
    public class AmmoClip : Item
    {
        private EquipmentSystemManager equipmentSystemManager;
        
        [SerializeField] private int ammoCount = 30;
        [SerializeField] private bool consumeOnUse = true;
        
        public int AmmoCount
        {
            get => ammoCount;
            private set => ammoCount = value;
        }

        public override void Equip()
        {
            base.Equip();
            Debug.Log("Ammo clip equipped.");
        }

        public override void Unequip()
        {
            base.Unequip();
            Debug.Log("Ammo clip unequipped.");
        }

        public override void Use()
        {
            Debug.Log("Ammo clip used to reload gun.");

            // Early return if equipmentSystemManager is not available
            if (equipmentSystemManager == null)
            {
                equipmentSystemManager = FindFirstObjectByType<EquipmentSystemManager>();
                if (equipmentSystemManager == null) return;
            }

            var items = equipmentSystemManager.GetEquippedItems();

            foreach (var item in items)
            {
                if (item is Gun gun)
                {
                    // Reload the gun with available ammo and reset ammo count
                    ammoCount = gun.Reload(ammoCount);
                    
                    Debug.Log(ammoCount);

                    if (ammoCount > 0)
                    {
                        return;
                    }
                    
                    // Unequip the ammo clip after using it
                    equipmentSystemManager.UnequipItem(this);
            
                    if (consumeOnUse)
                    {
                        Destroy(this.gameObject);
                    }
                    break; // Stops after reloading the first gun, remove if all guns should be reloaded
                }
            }
        }

        public override void StoppedUse()
        {
        }

        public override void SecondaryUse()
        {
            Debug.Log("Ammo clip secondary use.");
        }

        /// <summary>
        /// Set the ammo count of the ammo clip.
        /// </summary>
        /// <param name="ammo">The amount to add to the ammoCount</param>
        public void SetAmmoCount(int ammo)
        {
            ammoCount = ammo;
        }
    }
}