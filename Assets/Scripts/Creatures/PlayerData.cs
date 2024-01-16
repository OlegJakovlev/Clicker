using System;
using UnityEngine;

namespace Creatures
{
    [Serializable]
    [CreateAssetMenu(fileName = "Name", menuName = "Character/Player", order = 1)]
    public class PlayerData : CharacterData
    {
        [Header("Inventory Capacity")]
        public int maxItemsInInventory;

        public override void CopyFrom(CharacterData data)
        {
            base.CopyFrom(data);
            maxItemsInInventory = ((PlayerData)data).maxItemsInInventory;
        }
    }
}