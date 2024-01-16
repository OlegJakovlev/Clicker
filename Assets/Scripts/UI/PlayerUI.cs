using Creatures;
using ShopSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider shieldSlider;
        [SerializeField] private TextMeshProUGUI coinsTextContainer;

        private Player playerRef;
        private BalanceHandler playerBalance;

        public void Initialise()
        {
            playerRef = Root.Instance.Player;
            
            playerBalance = playerRef.BalanceHandler;
            playerBalance.BalanceChanged += UpdateBalanceUI;

            playerRef.CombatManager.HealthChanged += UpdateHealthUI;
            playerRef.CombatManager.ShieldChanged += UpdateShieldUI;

            UpdateBalanceUI();
        }

        private void UpdateHealthUI()
        {
            int maxHealth = playerRef.CombatManager.CurrentMaxHealth;
            
            if (maxHealth != 0)
            {
                healthSlider.value = playerRef.CombatManager.CurrentHealth / (float) maxHealth;
            }
            else
            {
                healthSlider.value = 0;
            }
        }

        private void UpdateShieldUI()
        {
            int maxShield = playerRef.CombatManager.CurrentMaxShield;
            
            if (maxShield != 0)
            {
                shieldSlider.value = playerRef.CombatManager.CurrentShield / (float) maxShield;
            }
            else
            {
                shieldSlider.value = 0;
            }
        }

        private void UpdateBalanceUI()
        {
            coinsTextContainer.text = playerBalance.CurrentBalance.ToString();
        }
    }
}