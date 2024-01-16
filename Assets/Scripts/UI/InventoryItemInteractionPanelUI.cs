using Creatures;
using InventorySystem.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryItemInteractionPanelUI : MonoBehaviour
    {
        [SerializeField] private GameObject panelContainer;
        
        [SerializeField] private Button equipButton;
        [SerializeField] private TextMeshProUGUI equipButtonText;
        
        [SerializeField] private Button useButton;
        [SerializeField] private Button sellButton;

        private Player playerRef;
        private InventoryUI inventoryRef;
        
        public void Initialise(InventoryUI inventoryRef)
        {
            playerRef = Root.Instance.Player;
            this.inventoryRef = inventoryRef;
        }
        
        public void ShowInteractionPanel(Item item)
        {
            ToggleEquipButton(item);
            ToggleUseButton(item);
            ToggleSellButton(item);

            panelContainer.SetActive(equipButton.gameObject.activeSelf || useButton.gameObject.activeSelf || sellButton.gameObject.activeSelf);
        }

        // TODO: Call when lose user presses somewhere else
        public void HideInteractionPanel()
        {
            panelContainer.SetActive(false);
        }

        private void ToggleSellButton(Item item)
        {
            if (item is ISellable sellableItem)
            {
                sellButton.onClick.RemoveAllListeners();
                
                sellButton.gameObject.SetActive(true);
                sellButton.onClick.AddListener(() =>
                {
                    sellableItem.Sell();
                    HideInteractionPanel();
                });
            }
            else
            {
                sellButton.gameObject.SetActive(false);
            }
        }

        private void ToggleUseButton(Item item)
        {
            if (item is IUsable usableItem)
            {
                useButton.onClick.RemoveAllListeners();
                
                useButton.gameObject.SetActive(true);
                useButton.onClick.AddListener(() =>
                {
                    usableItem.Use(Root.Instance.Player);
                    HideInteractionPanel();
                });
            }
            else
            {
                useButton.gameObject.SetActive(false);
            }
        }

        private void ToggleEquipButton(Item item)
        {
            if (item is IEquipable equipableItem)
            {
                equipButton.onClick.RemoveAllListeners();
                equipButton.gameObject.SetActive(true);

                if (playerRef.Inventory.IsEquipped(equipableItem))
                {
                    equipButtonText.text = "Unequip";
                    equipButton.onClick.AddListener(() =>
                    {
                        ToggleEquipItem(equipableItem, false);
                        HideInteractionPanel();
                    });
                }
                else
                {
                    equipButtonText.text = "Equip";
                    equipButton.onClick.AddListener(() =>
                    {
                        ToggleEquipItem(equipableItem, true);
                        HideInteractionPanel();
                    });
                }
            }
            else
            {
                equipButton.gameObject.SetActive(false);
            }
        }

        private void ToggleEquipItem(IEquipable equipableItem, bool shouldEquip)
        {
            if (shouldEquip)
            {
                equipableItem.Equip(Root.Instance.Player);
            }
            else
            {
                equipableItem.Unequip(Root.Instance.Player);
            }
        }
    }
}