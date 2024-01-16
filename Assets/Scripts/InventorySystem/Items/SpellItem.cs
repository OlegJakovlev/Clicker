using Creatures;
using InventorySystem.ItemData;
using UnityEngine;

namespace InventorySystem.Items
{
    public class SpellItem : Item, IUsable, ISellable
    {
        public SpellItem(ItemData.ItemData data)
        {
            this.data = data;
        }

        public void Sell()
        {
            Root.Instance.Player.Inventory.RemoveItem(this);
            Root.Instance.Player.BalanceHandler.AddBalance(CurrentPrice);
            Debug.Log($"Player sold `{data.itemName}`({((SpellItemData)data).spellType}) for {CurrentPrice}");
        }

        public bool Use(Character character)
        {
            SpellItemData spellData = ((SpellItemData)data);
            
            switch (spellData.spellType)
            {
                case SpellType.HEAL:
                    character.CombatManager.Heal((int) spellData.spellValue);
                    break;
                
                case SpellType.SHIELD:
                    // Don't let character use potion when max shield is 0
                    if (character.CombatManager.CurrentMaxShield == 0)
                    {
                        Debug.Log("Can not use shield potion, max shield level is 0");
                        return false;
                    }
            
                    // Honestly, this is horrible game design
                    // If player wants to have 100% of shield all time, let him do it
                    if (character.CombatManager.CurrentShield > 0)
                    {
                        Debug.Log("Can not use shield potion, as current shield is more than 0");
                        return false;
                    }
                    
                    character.CombatManager.AddShield((int)spellData.spellValue);
                    break;
            }
            
            Debug.Log($"{character.data.name} used spell of `{spellData.itemName}`({spellData.spellType})");

            character.Inventory.RemoveItem(this);
            return true;
        }
    }
}