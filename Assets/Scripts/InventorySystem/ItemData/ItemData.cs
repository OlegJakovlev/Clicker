using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.ItemData
{
    public enum ItemType
    {
        WEAPON,
        SPELL,
    }
    
    [Serializable]
    [CreateAssetMenu(fileName = "Name", menuName = "Items/Item", order = 1)]
    public abstract class ItemData : ScriptableObject
    {
        [Header("Item Data")]
        public ItemType type;
        public Sprite itemIcon;
        public string itemName;
        public string itemDescription;
        public List<ItemRequirement> requirements = new List<ItemRequirement>();
        
        [Header("Stack settings")]
        public bool isStackable;
        public int maxInStack;

        [Header("Shop configuration")]
        public int basePrice;

        public virtual void CopyFrom(ItemData data)
        {
            type = data.type;
            itemIcon = data.itemIcon;
            itemName = data.itemName;
            itemDescription = data.itemDescription;

            foreach (ItemRequirement requirement in data.requirements)
            {
                requirements.Add(new ItemRequirement(requirement));
            }

            isStackable = data.isStackable;
            maxInStack = data.maxInStack;
            basePrice = data.basePrice;
        }
    }
}