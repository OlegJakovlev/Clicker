using System;
using System.Collections.Generic;
using Creatures;
using UnityEngine;

namespace CombatSystem
{
    public class CombatHandler
    {
        public event Action<Character> KilledCharacter;
        public event Action<int, int> DealtDamage;
        public event Action Died;
        public event Action HealthChanged;
        public event Action ShieldChanged;


        private Character characterRef;

        public int CurrentAttackPower { get; private set; }
        public int CurrentShield { get; private set; }
        public int CurrentMaxShield { get; private set; }
        public int CurrentHealth { get; private set; }
        public int CurrentMaxHealth { get; private set; }

        public CombatHandler(Character character)
        {
            characterRef = character;
            CurrentMaxHealth = characterRef.data.maxHealth;
            CurrentMaxShield = characterRef.data.maxShield;
            CurrentAttackPower = characterRef.data.attackPower;
        }

        public void Reset()
        {
            CurrentHealth = CurrentMaxHealth;
            CurrentShield = CurrentMaxShield;

            HealthChanged?.Invoke();
            ShieldChanged?.Invoke();
        }

        public void IncreaseAttackPower(int value)
        {
            if (value < 0)
            {
                return;
            }
            
            CurrentAttackPower += value;
        }

        public void DecreaseAttackPower(int value)
        {
            if (value < 0)
            {
                return;
            }
            
            CurrentAttackPower -= value;
        }
        
        public void IncreaseMaxHealth(int value)
        {
            if (value < 0)
            {
                return;
            }
            
            CurrentMaxHealth += value;
            HealthChanged?.Invoke();
        }

        public void DecreaseMaxHealth(int value)
        {
            if (value < 0)
            {
                return;
            }
            
            CurrentMaxHealth -= value;
            HealthChanged?.Invoke();
        }
        
        public void IncreaseMaxShield(int value)
        {
            if (value < 0)
            {
                return;
            }
            
            CurrentMaxShield += value;
            ShieldChanged?.Invoke();
        }

        public void DecreaseMaxShield(int value)
        {
            if (value < 0)
            {
                return;
            }
            
            CurrentMaxShield -= value;
            ShieldChanged?.Invoke();
        }

        public void Damage(Character target)
        {
            Damage(target, CurrentAttackPower);
            DealtDamage?.Invoke(CurrentAttackPower, 1);
        }
        
        public void DamageSplit(List<Character> targets)
        {
            if (targets.Count == 0)
            {
                return;
            }
            
            int damageForEntity = Mathf.Max(1, CurrentAttackPower / targets.Count);
            foreach (var target in targets)
            {
                Damage(target, damageForEntity);
            }
            
            DealtDamage?.Invoke(damageForEntity, targets.Count);
        }
        
        private void Damage(Character target, int damage)
        {
            int damageLeft = damage;

            int targetShield = target.CombatManager.CurrentShield;
        
            if (targetShield > 0)
            {
                if (targetShield - damageLeft >= 0)
                {
                    target.CombatManager.CurrentShield -= damageLeft;
                    target.CombatManager.ShieldChanged?.Invoke();
                    return;
                }

                damageLeft -= targetShield;
                target.CombatManager.CurrentShield = 0;
                target.CombatManager.ShieldChanged?.Invoke();
            }
        
            target.CombatManager.CurrentHealth -= damageLeft;
            target.CombatManager.HealthChanged?.Invoke();

            if (target.CombatManager.CurrentHealth <= 0)
            {
                Kill(target);
            }
        }
        
        public void Heal(int amount)
        {
            if (amount < 0)
            {
                return;
            }
            
            CurrentHealth = Mathf.Min(CurrentHealth + amount, characterRef.CombatManager.CurrentMaxHealth);
            HealthChanged?.Invoke();
        }

        public void AddShield(int amount)
        {
            if (amount < 0)
            {
                return;
            }

            CurrentShield = Mathf.Min(CurrentShield + amount, CurrentMaxShield);
            ShieldChanged?.Invoke();
        }

        private void Kill(Character victim)
        {
            victim.CombatManager.Die();
            KilledCharacter?.Invoke(victim);
        }

        private void Die()
        {
            Died?.Invoke();
        }
    }
}