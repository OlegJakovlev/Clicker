using System;
using System.Collections.Generic;
using System.Linq;
using Creatures;
using InventorySystem.ItemData;
using InventorySystem.Items;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryHandler
    {
        public event Action<Item> ItemAdded;
        public event Action<Item> ItemRemoved;
        
        private HashSet<Item> inventory;
        private HashSet<Item> equippedItems;
        private Dictionary<ItemType, HashSet<Item>> inventoryByType = new Dictionary<ItemType, HashSet<Item>>();

        private Character characterRef;

        public int MaxInventorySize { get; }

        private InventoryHandler()
        {
        }

        public InventoryHandler(Character character, int maxInventorySize)
        {
            characterRef = character;
            MaxInventorySize = maxInventorySize;
            inventory = new HashSet<Item>(maxInventorySize);
            equippedItems = new HashSet<Item>(maxInventorySize);
        }
        
        public IEnumerable<Item> GetItemsByType(ItemType type)
        {
            if (inventoryByType.TryGetValue(type, out HashSet<Item> itemList))
            {
                foreach (Item item in itemList)
                {
                    yield return item;
                }
            }
        }

        public IReadOnlyCollection<Item> GetAllItems()
        {
            return inventory;
        }

        public Item GetFirstItemByType(ItemType type)
        {
            if (inventoryByType.TryGetValue(type, out HashSet<Item> itemList))
            {
                return itemList.Single();
            }

            return null;
        }

        public void AddItem(Item item)
        {
            inventory.Add(item);

            if (inventoryByType.TryGetValue(item.data.type, out HashSet<Item> existingItems))
            {
                existingItems.Add(item);
            }
            else
            {
                inventoryByType.Add(item.data.type, new HashSet<Item>{item});
            }
            
            ItemAdded?.Invoke(item);
        }

        public void RemoveItem(Item item)
        {
            if (!inventory.Contains(item))
            {
                Debug.LogWarning($"Trying to remove [{item.data.type}] that does not exist in inventory!");
                return;
            }
            
            inventory.Remove(item);
            inventoryByType[item.data.type].Remove(item);
            
            ItemRemoved?.Invoke(item);
        }
        
        public void ClearInventory()
        {
            inventory.Clear();
            inventoryByType.Clear();
        }

        public bool ContainsAnyOfType(ItemType type)
        {
            if (inventoryByType.TryGetValue(type, out HashSet<Item> items))
            {
                return items.Count > 0;
            }

            return false;
        }
        
        public bool ContainsEquippedAnyOfType(ItemType type)
        {
            if (inventoryByType.TryGetValue(type, out HashSet<Item> items))
            {
                foreach (Item Item in items)
                {
                    if (equippedItems.Contains(Item))
                    {
                        return true;
                    }
                }

                return false;
            }

            return false;
        }
        
        public bool TryEquip(Item item)
        {
            if (MeetRequirements(item.data.requirements))
            {
                equippedItems.Add(item);
                return true;
            }

            return false;
        }

        public bool Unequip(Item item)
        {
            if (equippedItems.Contains(item))
            {
                equippedItems.Remove(item);
            }

            return false;
        }

        public bool IsEquipped(IEquipable item)
        {
            return equippedItems.Contains((Item) item);
        }

        private bool MeetRequirements(List<ItemRequirement> requirements)
        {
            return requirements.All(requirement => requirement.IsFulfilledBy(characterRef));
        }
        
        public bool UseSpell(SpellType spellType, Character spellTarget)
        {
            List<Item> spellItems = GetItemsByType(ItemType.SPELL).ToList();

            // Refactor to have a search by O(1), not O(n)
            foreach (var item in spellItems)
            {
                SpellItem spellItem = (SpellItem)item;
                SpellItemData spellData = (SpellItemData)spellItem.data;
                
                if (spellData.spellType == spellType)
                {
                    if (spellItem.Use(spellTarget))
                    {
                        return true;
                    };
                }
            }
            
            Debug.Log($"{spellTarget.data.name} don't have any {spellType} potions");

            return false;
        }
    }
}