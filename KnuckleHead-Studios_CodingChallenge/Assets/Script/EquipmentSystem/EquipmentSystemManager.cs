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
        [SerializeField] private GameObject leftHandOrigin;
        [SerializeField] private GameObject rightHandOrigin;
        [SerializeField] private GameObject headOrigin;

        private void Start()
        {
            // Initialize the equipment dictionary
            equippedItems = new Dictionary<EquipmentSlot, Item>();
        }

        // Equip item to a specific slot (left hand, right hand, or head)
        /// <summary>
        /// Equip an item to a specific slot.
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


        /// <summary>
        /// Unequip the item in the specified slot.
        /// </summary>
        /// <param name="_slot">Slot to unequip an item from</param>
        /// <returns>Whether the Item has succesfully Unequipped</returns>
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

        /// <summary>
        /// Unequip an item from any slot in the equipment system.
        /// Prioritizes unequipping the item that got equipped last.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Use the item in the specified slot.
        /// </summary>
        /// <param name="slot"></param>
        public void UseItem(EquipmentSlot slot)
        {
            if (equippedItems.ContainsKey(slot))
            {
                equippedItems[slot].Use();
            }
        }

        /// <summary>
        /// Swap the items in the left and right hand slots.
        /// </summary>
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

        /// <summary>
        /// Stop using the item in the specified slot. This is called when the input for using the item is released.
        /// </summary>
        /// <param name="slot"></param>
        public void StopUseItem(EquipmentSlot slot)
        {
            if (equippedItems.ContainsKey(slot))
            {
                equippedItems[slot].StoppedUse();
            }
        }

        /// <summary>
        /// Perform the secondary action of the item in the specified slot.
        /// </summary>
        /// <param name="slot"></param>
        public void SecondaryAction(EquipmentSlot slot)
        {
            if (equippedItems.ContainsKey(slot))
            {
                equippedItems[slot].SecondaryUse();
            }
        }

        /// <summary>
        /// Get the origin point for the specified equipment slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
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
        /// <returns>List of Items in all slots except the specified slot</returns>
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

        /// <summary>
        /// Get all equipped items in the equipment system. Without any slot specification.
        /// </summary>
        /// <returns>List of all Items in the equipment system</returns>
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