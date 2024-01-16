using System;
using System.Collections.Generic;
using InventorySystem.Items;
using ShopSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ShopUI : MonoBehaviour
    {
        [Serializable]
        private class ShopItem
        {
            [SerializeField] private GameObject coinImage;
            [SerializeField] private TextMeshProUGUI itemPrice;
            [SerializeField] private Image itemIcon;
            [SerializeField] private Button purchaseButton;
            [SerializeField] private EventTrigger eventTriggerHandler;

            public Item ItemRef { get; private set; }
            public bool IsPurchased
            {
                get => isPurchased;
                set
                {
                    if (value)
                    {
                        itemIcon.sprite = soldOutImage;
                        itemPrice.text = null;
                    }
                    
                    coinImage.SetActive(!value);
                    isPurchased = value;
                }
            }

            private bool isPurchased;
            private ShopUI shopUIRef;

            public void Initialise(ShopUI shopUI)
            {
                shopUIRef = shopUI;
            }
            
            public void PostInitialise()
            {
                SetupPointerEntryEvent();
                SetupPointerExitEvent();
                SetupPurchaseEvent();
            }

            private void SetupPurchaseEvent()
            {
                purchaseButton.onClick.AddListener(() =>
                {
                    if (IsPurchased)
                    {
                        return;
                    }
                
                    if (ItemRef == null)
                    {
                        return;
                    }
            
                    if (Root.Instance.ShopManager.BuyItem(ItemRef))
                    {
                        IsPurchased = true;
                    }
                });
            }

            public void PopulateInfo(Item item)
            {
                if (item == null)
                {
                    IsPurchased = true;
                    return;
                }

                IsPurchased = false;
                ItemRef = item;
                itemIcon.sprite = item.data.itemIcon;
                itemPrice.text = item.data.basePrice.ToString();
            }

            private void SetupPointerExitEvent()
            {
                EventTrigger.Entry pointerExitEvent = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerExit
                };
                pointerExitEvent.callback.AddListener((_) => OnPointerExit());
                eventTriggerHandler.triggers.Add(pointerExitEvent);
            }

            private void SetupPointerEntryEvent()
            {
                EventTrigger.Entry pointerEntryEvent = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerEnter
                };
                pointerEntryEvent.callback.AddListener((_) => OnPointerEnter());
                eventTriggerHandler.triggers.Add(pointerEntryEvent);
            }

            private void OnPointerEnter()
            {
                if (!isPurchased)
                {
                    shopUIRef.descriptionPanelUI.PopulateInfo(ItemRef);
                }
                else
                {
                    shopUIRef.descriptionPanelUI.HidePanel();
                }
            }
        
            private void OnPointerExit()
            {
                shopUIRef.descriptionPanelUI.HidePanel();
            }
        }

        [SerializeField] private GameObject screen;
        [SerializeField] private DescriptionPanelUI descriptionPanelUI;
        [SerializeField] private TextMeshProUGUI refreshTimerTextContainer;
        [SerializeField] private List<ShopItem> shopItemsUI;
        
        private static Sprite soldOutImage;
        private Shop shopManagerCached;

        public void Initialise()
        {
            soldOutImage = Resources.Load<Sprite>("Sprites/SoldOut");
            shopManagerCached = Root.Instance.ShopManager;
            shopManagerCached.RefreshedItems += UpdateShopItemsUI;

            InitialiseShopItemsUI();
        }

        private void Update()
        {
            refreshTimerTextContainer.text = $"Refresh in: {(int)(shopManagerCached.NextRefreshTime + 1 - Time.time)}";
        }

        public void OpenScreen()
        {
            if (Root.Instance.Player.ExperienceHandler.CurrentLevel < 2)
            {
                return;
            }
            
            screen.SetActive(true);
            UpdateShopItemsUI();
            Root.Instance.TicksActive = false;
        }

        private void InitialiseShopItemsUI()
        {
            foreach (ShopItem shopItem in shopItemsUI)
            {
                shopItem.Initialise(this);
                shopItem.PostInitialise();
            }
        }
        
        private void UpdateShopItemsUI()
        {
            var list = shopManagerCached.GetAvailableItems();
            int validItems = list.Count;
            
            for (var i = 0; i < shopManagerCached.ShownItems; i++)
            {
                if (i >= validItems)
                {
                    shopItemsUI[i].PopulateInfo(null);
                }
                else
                {
                    shopItemsUI[i].PopulateInfo(list[i]);
                }
            }
        }

        public void CloseScreen()
        {
            screen.SetActive(false);
            Root.Instance.TicksActive = true;
        }
    }
}