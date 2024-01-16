using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem;
using InventorySystem.Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [Serializable]
        private class InventoryItemUI
        {
            [SerializeField] private Button inventoryItemButton;
            [SerializeField] private Image spriteContainer;

            private int indexId;
            private InventoryUI inventoryRef;

            public Item ItemRef { get; private set; }
            
            public void Initialise(int indexId, InventoryUI inventoryUI)
            {
                this.indexId = indexId;
                inventoryRef = inventoryUI;
            }

            public void PostInitialise()
            {
                inventoryItemButton.onClick.AddListener(() =>
                {
                    inventoryRef.OpenInteractionPanel(indexId);
                    inventoryRef.descriptionPanel.PopulateInfo(ItemRef);
                });
            }

            public void SetItem(Item item)
            {
                ItemRef = item;
                spriteContainer.sprite = item == null ? nullItemIcon : ItemRef.data.itemIcon;
            }
        }
        
        [SerializeField] private List<InventoryItemUI> itemsUI;
        [SerializeField] private GameObject screen;
        [SerializeField] private DescriptionPanelUI descriptionPanel;
        [SerializeField] private InventoryItemInteractionPanelUI itemInteractionPanelUI;
        [SerializeField] private GridLayoutGroup layout;

        private static Sprite nullItemIcon;
        private InventoryHandler playerInventoryRef;
        
        public void Initialise()
        {
            nullItemIcon = Resources.Load<Sprite>("Sprites/EmptyInventorySlot");
            
            for (var index = 0; index < itemsUI.Count; index++)
            {
                var itemUI = itemsUI[index];
                itemUI.Initialise(index, this);
                itemUI.PostInitialise();
            }
            
            playerInventoryRef = Root.Instance.Player.Inventory;

            playerInventoryRef.ItemAdded += (_) => RefreshInventoryUI();
            playerInventoryRef.ItemRemoved += (_) => RefreshInventoryUI();
            
            itemInteractionPanelUI.Initialise(this);
        }

        public void OpenScreen()
        {
            screen.SetActive(true);
            RefreshInventoryUI();
            Root.Instance.TicksActive = false;
        }

        private void RefreshInventoryUI()
        {
            List<Item> allItems = playerInventoryRef.GetAllItems().ToList();

            for (int i = 0; i < playerInventoryRef.MaxInventorySize; i++)
            {
                if (i >= allItems.Count)
                {
                    itemsUI[i].SetItem(null);
                }
                else
                {
                    itemsUI[i].SetItem(allItems[i]);
                }
            }
            
            descriptionPanel.HidePanel();
        }

        public void CloseScreen()
        {
            descriptionPanel.HidePanel();
            itemInteractionPanelUI.HideInteractionPanel();
            screen.SetActive(false);
            Root.Instance.TicksActive = true;
        }

        private void OpenInteractionPanel(int cellIndex)
        {
            MoveInteractionPanelToCell(cellIndex);
            itemInteractionPanelUI.ShowInteractionPanel(itemsUI[cellIndex].ItemRef);
        }

        private void MoveInteractionPanelToCell(int cellIndex)
        {
            // TODO: FIX ME, I'M WRONG
            
            float x = layout.cellSize.x;
            float y = layout.cellSize.y;

            int xIndex = 0;
            int yIndex = 0;
            float xOffset = 0;
            float yOffset = 0;

            switch (layout.constraint)
            {
                case GridLayoutGroup.Constraint.FixedColumnCount:
                    xIndex = cellIndex % layout.constraintCount;
                    yIndex = cellIndex / layout.constraintCount;
                    break;

                case GridLayoutGroup.Constraint.FixedRowCount:
                    xIndex = cellIndex / layout.constraintCount;
                    yIndex = cellIndex % layout.constraintCount;
                    break;
            }

            x *= xIndex;
            y *= yIndex;

            xOffset = xIndex * layout.spacing.x + layout.cellSize.x / 4 - gameObject.transform.localScale.x * 0.5f;
            yOffset = yIndex * layout.spacing.y + layout.cellSize.y / 4 - gameObject.transform.localScale.y * 0.5f;

            itemInteractionPanelUI.gameObject.transform.localPosition = new Vector2(x + xOffset, y + yOffset);
        }
    }
}