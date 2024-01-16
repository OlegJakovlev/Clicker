using System;
using System.Collections.Generic;
using Creatures;
using Factories;
using InventorySystem;
using InventorySystem.Items;
using UnityEngine;
using Utility;

namespace ShopSystem
{
    public class Shop
    {
        public event Action RefreshedItems;

        public int ShownItems { get; }
        public float RefreshDelayTime { get; }
        public float NextRefreshTime { get; private set; }

        private Player playerRef;
        private BalanceHandler playerBalanceHandler;
        private List<Item> availableItems = new List<Item>();

        public Shop(int shownItems, float refreshDelayTime)
        {
            this.ShownItems = shownItems;
            this.RefreshDelayTime = refreshDelayTime;
        }
        
        public void Initialise()
        {
            playerRef = Root.Instance.Player;
            playerBalanceHandler = playerRef.BalanceHandler;
            PopulateAvailableItems(ShownItems);
        }

        public void Update()
        {
            var currentTime = Time.time;
            if (NextRefreshTime == 0)
            {
                NextRefreshTime = currentTime + RefreshDelayTime;
                return;
            }

            if (currentTime >= NextRefreshTime)
            {
                NextRefreshTime = currentTime + RefreshDelayTime;
                RefreshAllItems();
            }
        }

        public IReadOnlyList<Item> GetAvailableItems()
        {
            return availableItems;
        }

        private void PopulateAvailableItems(int amountOfItemsToPopulate)
        {
            IReadOnlyList<Item> databaseResponse = RandomUtility.GetRandomUniqueElements(Root.Instance.ItemDatabase.GetAllItems(), amountOfItemsToPopulate);

            foreach (Item databaseItem in databaseResponse)
            {
                Item item = ItemFactory.Clone(databaseItem);
                item.Initialise();
                
                item.ScaleWithLevel(playerRef.ExperienceHandler.CurrentLevel);
                availableItems.Add(item);
            }
        }

        private void RefreshAllItems()
        {
            availableItems.Clear();
            PopulateAvailableItems(ShownItems);
            RefreshedItems?.Invoke();
        }

        public bool BuyItem(Item item)
        {
            int price = item.data.basePrice;

            if (playerBalanceHandler.CurrentBalance < price)
            {
                return false;
            }
            
            playerBalanceHandler.SubtractBalance(price);
            playerRef.Inventory.AddItem(item);
            availableItems.Remove(item);
            
            Debug.Log($"Purchased {item.data.itemName} for {price}");
            
            return true;
        }
    }
}