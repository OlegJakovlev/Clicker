using Creatures;
using InventorySystem.ItemData;
using UnityEngine;

namespace InventorySystem.Items
{
    public class WeaponItem : Item, IEquipable, ISellable
    {
        public WeaponItem(ItemData.ItemData data)
        {
            this.data = data;
        }
        
        public override void ScaleWithLevel(int level)
        {
            UpdateItemPrice();
        }

        public bool Equip(Character character)
        {
            bool success = character.Inventory.TryEquip(this);

            if (success)
            {
                character.CombatManager.IncreaseAttackPower(((WeaponItemData) data).baseDamage * CurrentLevel);
            }
            
            Debug.Log(success
                ? $"{character.data.name} equipped `{data.itemName}`"
                : $"{character.data.name} tried to equipped `{data.itemName}` and failed");

            return success;
        }

        public void Unequip(Character character)
        {
            character.Inventory.Unequip(this);
            character.CombatManager.DecreaseAttackPower(((WeaponItemData) data).baseDamage * CurrentLevel);
            Debug.Log($"Player unequipped {data.itemName}");
        }

        public void Sell()
        {
            Root.Instance.Player.Inventory.RemoveItem(this);
            Root.Instance.Player.BalanceHandler.AddBalance(CurrentPrice);
            Debug.Log($"Player sold `{data.itemName}` for {CurrentPrice}");
        }
    }
}