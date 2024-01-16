using System;
using UnityEngine;

namespace InventorySystem.ItemData
{
    [Serializable]
    [CreateAssetMenu(fileName = "Name", menuName = "Items/WeaponItem", order = 1)]
    public class WeaponItemData : ItemData
    {
        [Header("Weapon data")]
        public int baseDamage;
        
        public override void CopyFrom(ItemData data)
        {
            base.CopyFrom(data);
            baseDamage = ((WeaponItemData)data).baseDamage;
        }
    }
}