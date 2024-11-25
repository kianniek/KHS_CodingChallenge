using System.Collections.Generic;
using InteractableSystem;
using UnityEditor;
using UnityEngine;

namespace EquipmentSystem
{
    /// <summary>
    /// Represents the different equipment slots available in the game.
    /// </summary>
    public enum EquipmentSlot
    {
        Undefined,
        LeftHand,
        RightHand,
        Head
    }

    /// <summary>
    /// Manages the equipment system in the game.
    /// </summary>
    public class EquipmentSystemManager : MonoBehaviour
    {
        private Dictionary<EquipmentSlot, Item> equippedItems;

        // Define origin points for each equipment slot
        public GameObject leftHandOrigin;
        public GameObject rightHandOrigin;
        public GameObject headOrigin;

        void Start()
        {
            // Initialize the equipment dictionary
            equippedItems = new Dictionary<EquipmentSlot, Item>();
        }

        // Equip item to a specific slot (left hand, right hand, or head)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="itemObject"></param>
        /// <returns>Whether the Item is equipped</returns>
        public bool EquipItem(EquipmentSlot slot, GameObject itemObject)
        {
            if (itemObject == null || slot == EquipmentSlot.Undefined)
            {
                Debug.LogError("Invalid item or slot.");
                return false;
            }

            Item item = itemObject.GetComponent<Item>();
            if (item == null)
            {
                Debug.LogError("Item component not found.");
                return false;
            }
            
            Debug.Log($"Equipping {item.name} to {slot}");

            // Unequip the existing item in the target slot if necessary
            if (equippedItems.ContainsKey(slot))
            {
                Debug.Log($"Slot {slot} is already occupied. Unequipping current item.");
                UnequipItem(slot);
            }

            // Equip the item to the specified slot
            equippedItems[slot] = item;

            // Attach the item to the correct origin point
            Transform slotOrigin = GetSlotOrigin(slot);
            if (slotOrigin != null)
            {
                itemObject.transform.SetParent(slotOrigin);
                itemObject.transform.localPosition = item.PositionOffset;
                itemObject.transform.localRotation = Quaternion.Euler(item.RotationOffset);
            }

            // Trigger item-specific equip logic
            item.Equip();

            Debug.Log($"Equipped {item.name} to {slot}");
            return true;
        }


        // Unequip item from a specific slot
        public bool UnequipItem(EquipmentSlot _slot)
        {
            if (!equippedItems.ContainsKey(_slot))
            {
                //if the right hand is empty, unequip the item in the left hand
                if (_slot == EquipmentSlot.RightHand)
                {
                    UnequipItem(EquipmentSlot.LeftHand);
                }

                return false;
            }

            if (_slot == EquipmentSlot.Head)
            {
                var isLeftHandEmpty = !equippedItems.ContainsKey(EquipmentSlot.LeftHand);
                var isRightHandEmpty = !equippedItems.ContainsKey(EquipmentSlot.RightHand);

                //if one of the hands is empty, equip the item in the head slot to the empty hand
                if (isLeftHandEmpty || isRightHandEmpty)
                {
                    EquipmentSlot emptyHandSlot =
                        isLeftHandEmpty ? EquipmentSlot.LeftHand : EquipmentSlot.RightHand;

                    Item headEquippedItem = equippedItems[_slot];

                    equippedItems[_slot].Unequip();
                    equippedItems[_slot].gameObject.transform.SetParent(null); // Unparent the item
                    equippedItems.Remove(_slot);

                    EquipItem(emptyHandSlot, headEquippedItem.gameObject);
                    return true;
                }
            }

            equippedItems[_slot].Unequip();
            equippedItems[_slot].gameObject.transform.SetParent(null); // Unparent the item
            equippedItems.Remove(_slot);

            return true;
        }

        public bool UnequipItem(Item item)
        {
            //find if the item is equipped and in which slot
            EquipmentSlot _slot = EquipmentSlot.Undefined;
            foreach (var equippedItem in equippedItems)
            {
                if (equippedItem.Value == item)
                {
                    _slot = equippedItem.Key;
                    break;
                }
            }

            if (_slot == EquipmentSlot.Undefined)
                return false;

            equippedItems[_slot].Unequip();
            equippedItems[_slot].gameObject.transform.SetParent(null); // Unparent the item
            equippedItems.Remove(_slot);
            return true;
        }

        // Use the item in the specified slot (e.g., pressing a button to use it)
        public void UseItem(EquipmentSlot slot)
        {
            if (equippedItems.ContainsKey(slot))
            {
                equippedItems[slot].Use();
            }
        }

        public void SwapPrimarySecondaryHands()
        {
            var isLeftHandEmpty = !equippedItems.ContainsKey(EquipmentSlot.LeftHand);
            var isRightHandEmpty = !equippedItems.ContainsKey(EquipmentSlot.RightHand);

            if (isLeftHandEmpty || isRightHandEmpty)
            {
                return;
            }

            var leftHandItem = equippedItems[EquipmentSlot.LeftHand];
            var rightHandItem = equippedItems[EquipmentSlot.RightHand];

            // Unequip the items
            UnequipItem(EquipmentSlot.LeftHand);
            UnequipItem(EquipmentSlot.RightHand);

            // Equip the items in the opposite slots
            EquipItem(EquipmentSlot.LeftHand, rightHandItem.gameObject);
            EquipItem(EquipmentSlot.RightHand, leftHandItem.gameObject);
        }

        // Stop using the item in the specified slot
        public void StopUseItem(EquipmentSlot slot)
        {
            if (equippedItems.ContainsKey(slot))
            {
                equippedItems[slot].StoppedUse();
            }
        }

        // Perform a secondary action with the item in the specified slot
        public void SecondaryAction(EquipmentSlot slot)
        {
            if (equippedItems.ContainsKey(slot))
            {
                equippedItems[slot].SecondaryUse();
            }
        }

        // Helper method to get the origin point for each slot
        private Transform GetSlotOrigin(EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentSlot.LeftHand:
                    return leftHandOrigin.transform;
                case EquipmentSlot.RightHand:
                    return rightHandOrigin.transform;
                case EquipmentSlot.Head:
                    return headOrigin.transform;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Get the item in all slots except the specified slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public Item[] GetItemInSlotsExclude(EquipmentSlot slot)
        {
            List<Item> items = new List<Item>();
            foreach (var item in equippedItems)
            {
                if (item.Key != slot)
                {
                    items.Add(item.Value);
                }
            }

            return items.ToArray();
        }

        public Item[] GetEquippedItems()
        {
            List<Item> items = new List<Item>();
            foreach (var item in equippedItems)
            {
                items.Add(item.Value);
            }

            return items.ToArray();
        }
    }
}