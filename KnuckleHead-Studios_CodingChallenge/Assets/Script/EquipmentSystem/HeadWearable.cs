using System.Collections.Generic;
using UnityEngine;

namespace EquipmentSystem
{
    public class HeadWearable : Item
    {
        public override void Equip()
        {
            base.Equip();
            Debug.Log("Hat equipped on head.");
        }

        public override void Unequip()
        {
            base.Unequip();
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
    }
}