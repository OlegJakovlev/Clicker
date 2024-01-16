using InventorySystem.Items;
using TMPro;
using UnityEngine;
using UpgradeSystem;

namespace UI
{
    public class DescriptionPanelUI : MonoBehaviour
    {
        [SerializeField] private GameObject PanelContainer;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;

        public void PopulateInfo(Item item)
        {
            if (item == null)
            {
                HidePanel();
                return;
            }
            
            PanelContainer.SetActive(true);
            
            itemName.text = item.data.itemName;
            itemDescription.text = item.data.itemDescription;
        }
        
        public void PopulateInfo(UpgradeData upgrade)
        {
            if (upgrade == null)
            {
                HidePanel();
                return;
            }
            
            PanelContainer.SetActive(true);
            
            itemName.text = upgrade.upgradeName;
            itemDescription.text = upgrade.description + "\n" + upgrade.GetAdditionalDescription();
        }

        public void HidePanel()
        {
            PanelContainer.SetActive(false);
        }
    }
}