using System;
using Creatures;
using UnityEngine;

namespace InventorySystem
{
    public enum RequirementType
    {
        LEVEL,
    }
    
    [Serializable]
    public class ItemRequirement
    {
        [SerializeField] private RequirementType type;
        [SerializeField] private float value;
        
        // For something more complex than the level requirement (remove HideInInspector when going to use it)
        [HideInInspector][SerializeField] private string stringValue;

        public ItemRequirement(ItemRequirement data)
        {
            this.type = data.type;
            this.value = data.value;
        }
        
        public bool IsFulfilledBy(Character character)
        {
            switch (type)
            {
                case RequirementType.LEVEL:
                    return character.ExperienceHandler.CurrentLevel >= value;
            }

            return true;
        }
    }
}