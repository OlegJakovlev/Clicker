using Creatures;
using UnityEngine;

namespace InventorySystem.Items
{
    public class ArmorItem : Item, ISellable, IEquipable
    {
        public ArmorItem(ItemData.ItemData data)
        {
            this.data = data;
        }
        
        public override void ScaleWithLevel(int level)
        {
            CurrentLevel = Random.Range(Mathf.Max(0, level - 5), level + 5 + 1);
            UpdateItemPrice();
            
            // Adjust stats of the armor item
        }

        public void Sell()
        {
            Root.Instance.Player.Inventory.RemoveItem(this);
            Root.Instance.Player.BalanceHandler.AddBalance(CurrentPrice);
            Debug.Log($"Player sold `{data.itemName}` for {CurrentPrice}");
        }

        public bool Equip(Character character)
        {
            bool success = character.Inventory.TryEquip(this);

            Debug.Log(success
                ? $"{character.data.name} equipped `{data.itemName}`"
                : $"{character.data.name} tried to equipped `{data.itemName}` and failed");

            return success;
        }
        
        public void Unequip(Character character)
        {
            character.Inventory.Unequip(this);
            Debug.Log($"{character.data.name} unequipped {data.itemName}");
        }
    }
}