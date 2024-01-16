using System;
using UnityEngine;

namespace UpgradeSystem
{
    public enum UpgradeEffectType
    {
        HEALTH = 1 << 0,
        SHIELD = 1 << 1,
        ATTACK = 1 << 2,
    }
    
    [Serializable]
    public class UpgradeEffect
    {
        public UpgradeEffectType updateType;
        public int value;

        public UpgradeEffect(UpgradeEffect effect)
        {
            this.updateType = effect.updateType;
            this.value = effect.value;
        }
    }
}