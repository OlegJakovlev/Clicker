using System;
using System.Collections.Generic;
using System.Linq;
using Creatures;
using Factories;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace CombatSystem
{
    public class CombatManager
    {
        public event Action SpawnedNextWave;
        public event Action RestartedWave;

        public int MinEnemies => 2;
        public int MaxEnemies => 3;

        private static int currentWave;
        private List<Monster> currentEnemies = new List<Monster>();
        private int currentEnemiesAmount;

        private static readonly List<Monster> nonBossMonsters = new();
        private static readonly List<Monster> bossMonsters = new();

        private Player playerRef;
        private bool shouldUpdateWave = false;
        
        public void Initialise()
        {
            IReadOnlyList<Monster> allEnemies = Root.Instance.EnemyCreatureDatabase.GetAllItems();

            foreach (var enemy in allEnemies)
            {
                Monster monsterClone = (Monster)CharacterFactory.Clone(enemy);
                monsterClone.Initialise();
                
                if (((MonsterData)enemy.data).isBoss)
                {
                    bossMonsters.Add(monsterClone);
                }
                else
                {
                    nonBossMonsters.Add(monsterClone);
                }
            }
        }

        public void Start()
        {
            playerRef = Root.Instance.Player;
            playerRef.CombatManager.Died += RestartWave;

            shouldUpdateWave = true;
        }

        public IEnumerable<Character> GetAvailableEnemies(int amount)
        {
            List<Monster> aliveEnemies = currentEnemies.Where(enemy => enemy.CombatManager.CurrentHealth > 0).ToList();
            return RandomUtility.GetRandomUniqueElements(aliveEnemies, amount);
        }

        public IEnumerable<Character> GetAllAvailableEnemies()
        {
            List<Monster> aliveEnemies = currentEnemies.Where(enemy => enemy.CombatManager.CurrentHealth > 0).ToList();
            return aliveEnemies;
        }

        private void RestartWave()
        {
            playerRef.Reset();
            
            foreach (Monster enemy in currentEnemies)
            {
                enemy.Reset();
            }
            
            RestartedWave?.Invoke();
        }
        
        private void SpawnNextWave()
        {
            currentWave++;
            Debug.Log($"[CombatManager] Spawning next wave: {currentWave}");
            
            if (currentWave % 100 == 0)
            {
                SpawnBossWave(3);
            }
            else if (currentWave % 50 == 0)
            {
                SpawnBossWave(2);
            }
            else if (currentWave % 10 == 0)
            {
                SpawnBossWave(1);
            }
            else
            {
                SpawnRegularWave();
            }
            
            foreach (var enemy in currentEnemies)
            {
                enemy.ScaleWithWave(currentWave);
                enemy.Reset();
            }
            
            SubscribeOnDeath();
        }

        public void Tick()
        {
            if (shouldUpdateWave)
            {
                shouldUpdateWave = false;
                UpdateWave();
            }
            
            foreach (var enemy in currentEnemies)
            {
                enemy.TryAttack(playerRef);
            }
        }

        private void UpdateWave()
        {
            if (currentEnemiesAmount > 0)
            {
                return;
            }
            
            UnsubscribeFromDeath();
            
            SpawnNextWave();
            SpawnedNextWave?.Invoke();
        }

        private void SpawnRegularWave()
        {
            int amountOfEnemies = Random.Range(MinEnemies, MaxEnemies+1);
            var currentEnemiesQuery = RandomUtility.GetRandomElements(nonBossMonsters, amountOfEnemies);
            currentEnemies.Clear();
            
            foreach (Monster enemy in currentEnemiesQuery)
            {
                Monster enemyClone = (Monster) CharacterFactory.Clone(enemy);
                enemyClone.Initialise();
                currentEnemies.Add(enemyClone);
            }
            
            currentEnemiesAmount = amountOfEnemies;
        }
        
        private void SpawnBossWave(int amountOfEnemies)
        {
            var currentEnemiesQuery = RandomUtility.GetRandomElements(bossMonsters, amountOfEnemies);
            currentEnemies.Clear();
            
            foreach (Monster enemy in currentEnemiesQuery)
            {
                Monster enemyClone = (Monster) CharacterFactory.Clone(enemy);
                enemyClone.Initialise();
                currentEnemies.Add(enemyClone);
            }
            
            currentEnemiesAmount = amountOfEnemies;
        }

        private void SubscribeOnDeath()
        {
            foreach (var enemy in currentEnemies)
            {
                enemy.CombatManager.Died += DeathCallback;
            }
        }

        private void UnsubscribeFromDeath()
        {
            foreach (var enemy in currentEnemies)
            {
                enemy.CombatManager.Died -= DeathCallback;
            }
        }

        private void DeathCallback()
        {
            currentEnemiesAmount--;
            shouldUpdateWave = true;
        }
    }
}