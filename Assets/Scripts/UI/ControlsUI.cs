using System;
using System.Collections.Generic;
using System.Linq;
using CombatSystem;
using Creatures;
using InventorySystem.ItemData;
using UnityEngine;

namespace UI
{
    public class ControlsUI : MonoBehaviour
    {
        [Serializable]
        private class ButtonWithLevelRequirement
        {
            public GameObject button;
            public int levelRequirement;

            public bool isUnlocked = false;
        }
        
        [SerializeField] private UpgradeUI upgradeScreen;
        [SerializeField] private InventoryUI inventoryScreen;
        [SerializeField] private ShopUI shopScreen;

        [Header("Control buttons with unlock")]
        [SerializeField] private List<ButtonWithLevelRequirement> buttonsWithLevelRequirements = new List<ButtonWithLevelRequirement>();

        private Player playerRef;
        private CombatManager combatManagerRef;

        public void Initialise()
        {
            playerRef = Root.Instance.Player;
            combatManagerRef = Root.Instance.CombatManager;
            
            upgradeScreen.Initialise();
            shopScreen.Initialise();
            inventoryScreen.Initialise();

            playerRef.ExperienceHandler.LeveledUp += CheckForUnlocks;
        }

        private void CheckForUnlocks()
        {
            int currentLevel = playerRef.ExperienceHandler.CurrentLevel;
            
            foreach (var button in buttonsWithLevelRequirements)
            {
                if (button.isUnlocked)
                {
                    continue;
                }

                if (button.levelRequirement <= currentLevel)
                {
                    button.button.SetActive(true);
                }
            }
        }

        public void SingleAttack()
        {
            List<Character> possibleTargets = combatManagerRef.GetAvailableEnemies(1).ToList();

            if (possibleTargets.Count == 0)
            {
                Debug.LogWarning("No targets available!");
                return;
            }
            
            playerRef.TryAttack(possibleTargets[0]);
        }

        public void AoEAttack()
        {
            List<Character> possibleTargets = combatManagerRef.GetAllAvailableEnemies().ToList();

            if (possibleTargets.Count == 0)
            {
                Debug.LogWarning("No targets available!");
                return;
            }
            
            playerRef.CombatManager.DamageSplit(possibleTargets);
        }

        public void UseHeal()
        {
            playerRef.Inventory.UseSpell(SpellType.HEAL, playerRef);
        }

        public void UseShield()
        {
            playerRef.Inventory.UseSpell(SpellType.SHIELD, playerRef);
        }

        public void OpenUpgradeScreen()
        {
            upgradeScreen.OpenScreen();
        }

        public void OpenInventory()
        {
            inventoryScreen.OpenScreen();
        }

        public void OpenShop()
        {
            shopScreen.OpenScreen();
        }
    }
}