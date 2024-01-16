using System.Collections.Generic;
using System.Linq;
using Creatures;
using UnityEngine;

namespace UpgradeSystem
{
    public class UpgradeHandler
    {
        private HashSet<Upgrade> upgrades = new HashSet<Upgrade>();
        private Character characterRef;
        
        public UpgradeHandler(Character character)
        {
            characterRef = character;
        }

        public bool HasUpgrade(Upgrade upgrade)
        {
            return upgrades.Contains(upgrade);
        }
        
        public void AddUpgrade(Upgrade upgrade)
        {
            upgrades.Add(upgrade);
            upgrade.ApplyTo(characterRef);
        }

        public void RemoveUpgrade(Upgrade upgrade)
        {
            if (!upgrades.Contains(upgrade))
            {
                Debug.LogWarning($"Trying to remove [{upgrade.data.name}], that does not exist!");
                return;
            }
            
            upgrades.Remove(upgrade);
        }
        
        public void ResetUpgrades()
        {
            upgrades.Clear();
        }
    }
}