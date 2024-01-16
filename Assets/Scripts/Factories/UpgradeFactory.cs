using UnityEngine;
using UpgradeSystem;

namespace Factories
{
    public static class UpgradeFactory
    {
        public static Upgrade Create(UpgradeData data)
        {
            return new Upgrade(data);
        }
        
        public static Upgrade Clone(Upgrade upgrade)
        {
            UpgradeData clonedData = null;
            
            clonedData = ScriptableObject.CreateInstance<UpgradeData>();
            clonedData.CopyFrom(upgrade.data);

            return Create(clonedData);
        }
    }
}