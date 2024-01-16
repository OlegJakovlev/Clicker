using Creatures;
using ExperienceSystem;
using ShopSystem;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsGotTextContainer;
        [SerializeField] private TextMeshProUGUI damageDealtTextContainer;
        [SerializeField] private TextMeshProUGUI currentLevelTextContainer;

        private BalanceHandler playerBalance;
        private ExperienceHandler playerExperience;
        private int previousBalance;

        public void Initialise()
        {
            Player playerRef = Root.Instance.Player;
            
            playerBalance = playerRef.BalanceHandler;
            playerBalance.BalanceChanged += UpdateCoinsGotText;

            playerRef.CombatManager.DealtDamage += UpdateDamageText;

            playerExperience = playerRef.ExperienceHandler;
            playerExperience.LeveledUp += UpdateLevelText;
            
            UpdateCoinsGotText();
            UpdateDamageText();
            UpdateLevelText();
        }

        private void UpdateCoinsGotText()
        {
            int currentBalance = playerBalance.CurrentBalance;
            
            coinsGotTextContainer.text = $"Coins got: {currentBalance - previousBalance}";
            previousBalance = currentBalance;
        }

        private void UpdateDamageText(int damageDealt = 0, int enemiesAmount = 1)
        {
            damageDealtTextContainer.text = $"Damage dealt: {Mathf.Max(1, damageDealt / enemiesAmount)} (*{enemiesAmount})";
        }

        private void UpdateLevelText()
        {
            currentLevelTextContainer.text = $"Player level: {playerExperience.CurrentLevel}";
        }
    }
}