using System;
using UnityEngine;

namespace InventorySystem.ItemData
{
    [Serializable]
    public enum SpellType
    {
        HEAL,
        SHIELD,
    }
    
    [Serializable]
    [CreateAssetMenu(fileName = "Name", menuName = "Items/SpellItem", order = 1)]
    public class SpellItemData : ItemData
    {
        [Header("Spell Item Special")]
        public SpellType spellType;
        public float spellValue;

        public void Awake()
        {
            type = ItemType.SPELL;
        }

        public override void CopyFrom(ItemData data)
        {
            base.CopyFrom(data);
            
            SpellItemData castedData = (SpellItemData) data;
            spellType = castedData.spellType;
            spellValue = castedData.spellValue;
        }
    }
}