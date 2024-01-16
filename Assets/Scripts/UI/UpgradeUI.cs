using System;
using System.Collections.Generic;
using System.Linq;
using Creatures;
using Factories;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UpgradeSystem;
using Utility;

namespace UI
{
    [Serializable]
    public class UpgradeItemUI
    {
        private static Sprite nullUpgradeImage;
        
        [SerializeField] private Image icon;
        [SerializeField] private GameObject priceAndCoinImageContainer;
        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private EventTrigger eventTriggerHandler;

        private UpgradeUI upgradeUIRef;
        public Upgrade upgradeRef;

        public void Initialise(UpgradeUI upgradeUIRef)
        {
            if (nullUpgradeImage == null)
            {
                nullUpgradeImage = Resources.Load<Sprite>("Sprites/EmptyInventorySlot");
            }
            
            this.upgradeUIRef = upgradeUIRef;
        }

        public void PostInitialise()
        {
            SetupPointerEntryEvent();
            SetupPointerExitEvent();

            upgradeButton.onClick.AddListener(() =>
            {
                if (upgradeUIRef.PurchaseUpgrade(upgradeRef))
                {
                    upgradeUIRef.RefreshUpgradeItem(this);
                }
            });
        }
        
        public void PopulateInfo(Upgrade upgrade)
        {
            if (upgrade == null)
            {
                upgradeRef = null;
                icon.sprite = nullUpgradeImage;
                priceAndCoinImageContainer.SetActive(false);
                return;
            }
            
            upgradeRef = upgrade;
            price.text = upgradeRef.data.initialCost.ToString();
            priceAndCoinImageContainer.SetActive(true);
        }

        private void SetupPointerExitEvent()
        {
            EventTrigger.Entry pointerExitEvent = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            pointerExitEvent.callback.AddListener((data) => OnPointerExit((PointerEventData)data));
            eventTriggerHandler.triggers.Add(pointerExitEvent);
        }

        private void SetupPointerEntryEvent()
        {
            EventTrigger.Entry pointerEntryEvent = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            pointerEntryEvent.callback.AddListener((data) => OnPointerEnter((PointerEventData)data));
            eventTriggerHandler.triggers.Add(pointerEntryEvent);
        }

        private void OnPointerEnter(PointerEventData arg0)
        {
            upgradeUIRef.descriptionPanelUI.PopulateInfo(upgradeRef?.data);
        }
        
        private void OnPointerExit(PointerEventData arg0)
        {
            upgradeUIRef.descriptionPanelUI.HidePanel();
        }
    }
    
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField] private GameObject screen;
        
        [SerializeField] private TextMeshProUGUI availableSkillPointsText;
        [SerializeField] private List<UpgradeItemUI> upgradesUI;
        public DescriptionPanelUI descriptionPanelUI;

        private Player playerRef;
        
        public void Initialise()
        {
            playerRef = Root.Instance.Player;
            playerRef.ExperienceHandler.LeveledUp += UpdateSkillPointText;
            
            InitialiseUpgradeItemsUI();
        }

        public void OpenScreen()
        {
            screen.SetActive(true);
            Root.Instance.TicksActive = false;
        }
        
        public void CloseScreen()
        {
            descriptionPanelUI.HidePanel();
            screen.SetActive(false);
            Root.Instance.TicksActive = true;
        }

        private void InitialiseUpgradeItemsUI()
        {
            List<Upgrade> availableUpgrades = Root.Instance.UpgradeDatabase.GetAllItems()
                .Where(upgrade => !playerRef.UpgradeHandler.HasUpgrade(upgrade)).ToList();

            int upgradesAmount = upgradesUI.Count;
            availableUpgrades = RandomUtility.GetRandomUniqueElements(availableUpgrades, upgradesAmount);

            for (var i = 0; i < upgradesAmount; i++)
            {
                upgradesUI[i].Initialise(this);
                upgradesUI[i].PostInitialise();

                if (i < availableUpgrades.Count)
                {
                    upgradesUI[i].PopulateInfo(availableUpgrades[i]);
                }
                else
                {
                    upgradesUI[i].PopulateInfo(null);
                }
            }
        }
        
        private void UpdateSkillPointText()
        {
            availableSkillPointsText.text = $"Available skill points: {playerRef.ExperienceHandler.AvailableSkillPoints}";
        }

        public bool PurchaseUpgrade(Upgrade upgradeRef)
        {
            if (upgradeRef == null)
            {
                return false;
            }
            
            if (playerRef.ExperienceHandler.AvailableSkillPoints < upgradeRef.data.initialCost)
            {
                return false;
            }
            
            Debug.Log($"Player acquired upgrade `{upgradeRef.data.upgradeName}` for {upgradeRef.data.initialCost}");
            
            playerRef.ExperienceHandler.SpendSkillPoint(upgradeRef.data.initialCost);
            playerRef.UpgradeHandler.AddUpgrade(upgradeRef);
            UpdateSkillPointText();

            return true;
        }

        public void RefreshUpgradeItem(UpgradeItemUI upgradeItemUI)
        {
            List<Upgrade> availableUpgradesQuery = Root.Instance.UpgradeDatabase.GetAllItems()
                .Where(upgrade => !playerRef.UpgradeHandler.HasUpgrade(upgrade)).ToList();

            List<Upgrade> possibleUpgrades = new List<Upgrade>();
            foreach (var upgrade in availableUpgradesQuery)
            {
                bool isShownInUpgradeShop = upgradesUI.Any(upgradeUI => upgrade != null && upgradeUI.upgradeRef.data.upgradeName == upgrade.data.upgradeName);

                if (!isShownInUpgradeShop)
                {
                    possibleUpgrades.Add(upgrade);
                }
            }

            // We purchased all upgrades
            if (possibleUpgrades.Count == 0)
            {
                upgradeItemUI.PopulateInfo(null);
                return;
            }

            Upgrade selectedUpgrade = possibleUpgrades[UnityEngine.Random.Range(0, possibleUpgrades.Count)];
            
            Upgrade selectedUpgradeClone = UpgradeFactory.Clone(selectedUpgrade);
            selectedUpgradeClone.Initialise();
            
            upgradeItemUI.PopulateInfo(selectedUpgradeClone);
        }
    }
}