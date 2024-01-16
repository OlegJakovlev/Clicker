using System;
using System.Collections.Generic;
using System.Linq;
using CombatSystem;
using Creatures;
using UnityEngine;

namespace UI
{
    public class EnemyUIManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyUI> enemiesUI;
        private CombatManager combatManager;

        private List<Character> currentEnemies = new List<Character>();

        public void Initialise()
        {
            combatManager = Root.Instance.CombatManager;
            combatManager.SpawnedNextWave += RepopulateAllEnemyUI;
            combatManager.RestartedWave += RepopulateAllEnemyUI;
        }

        private void RepopulateAllEnemyUI()
        {
            foreach (var enemyUI in enemiesUI)
            {
                enemyUI.Disable();
            }
            
            currentEnemies = combatManager.GetAllAvailableEnemies().ToList();
            
            for (int i = 0; i < currentEnemies.Count; i++)
            {
                var enemy = (Monster)currentEnemies[i];
                EnemyUI uiEntry = enemiesUI[i];
                
                enemy.CombatManager.Died += uiEntry.Disable;
                enemy.CombatManager.HealthChanged += uiEntry.UpdateCurrentHealth;
                enemy.CombatManager.ShieldChanged += uiEntry.UpdateCurrentShield;
                                                                                
                uiEntry.Enable();
                uiEntry.PopulateInfo(enemy);
            }
        }
    }
}