using System;
using System.Collections.Generic;
using InventorySystem.ItemData;
using InventorySystem.Items;
using UnityEngine;

namespace Factories
{
    public static class ItemFactory
    {
        private enum ItemDataType
        {
            SpellItemData = 1,
            WeaponItemData = 2,
        }
        
        private static readonly IReadOnlyDictionary<Type, ItemDataType> ItemDataTypes = new Dictionary<Type, ItemDataType>()
        {
            { typeof(SpellItemData), ItemDataType.SpellItemData },
            { typeof(WeaponItemData), ItemDataType.WeaponItemData },
        };

        public static Item Create(ItemData data)
        {
            switch (data.type)
            {
                case ItemType.WEAPON:
                    return new WeaponItem(data);
                
                case ItemType.SPELL:
                    return new SpellItem(data);
                
                default:
                    Debug.LogError($"Invalid item created! Type: {data.type}");
                    return null;
            }
        }

        public static Item Clone(Item item)
        {
            ItemDataType itemDataType = ItemDataTypes[item.data.GetType()];
            ItemData clonedData = null;
            
            switch (itemDataType)
            {
                case ItemDataType.SpellItemData:
                    clonedData = ScriptableObject.CreateInstance<SpellItemData>();
                    break;
                
                case ItemDataType.WeaponItemData:
                    clonedData = ScriptableObject.CreateInstance<WeaponItemData>();
                    break;
                
                default:
                    Debug.LogError($"Can not create item data of type {itemDataType}!");
                    return null;
            }
            
            clonedData.CopyFrom(item.data);
            return Create(clonedData);
        }
    }
}