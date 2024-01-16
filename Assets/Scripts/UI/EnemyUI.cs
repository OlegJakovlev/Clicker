using Creatures;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider shieldSlider;
        [SerializeField] private Image spriteHolder;

        private Monster monsterRef;
        
        public void Enable()
        {
            container.SetActive(true);
        }

        public void Disable()
        {
            if (monsterRef != null)
            {
                monsterRef.CombatManager.Died -= Disable;
                monsterRef.CombatManager.HealthChanged -= UpdateCurrentHealth;
                monsterRef.CombatManager.ShieldChanged -= UpdateCurrentShield;
            }

            container.SetActive(false);
        }
        
        public void PopulateInfo(Monster monster)
        {
            monsterRef = monster;
            spriteHolder.sprite = ((MonsterData)monster.data).icon;
            UpdateCurrentHealth();
            UpdateCurrentShield();
        }

        public void UpdateCurrentHealth()
        {
            int maxHealth = monsterRef.CombatManager.CurrentMaxHealth;
            
            if (maxHealth != 0)
            {
                healthSlider.value = monsterRef.CombatManager.CurrentHealth / (float) maxHealth;
            }
            else
            {
                healthSlider.value = 0;
            }
        }
        
        public void UpdateCurrentShield()
        {
            int maxShield = monsterRef.CombatManager.CurrentMaxShield;
            
            if (maxShield != 0)
            {
                shieldSlider.value = monsterRef.CombatManager.CurrentShield / (float) maxShield;
            }
            else
            {
                shieldSlider.value = 0;
            }
        }
    }
}