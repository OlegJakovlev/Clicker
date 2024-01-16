using Factories;
using InventorySystem.ItemData;
using InventorySystem.Items;

namespace Database
{
    public class ItemDatabase : AbstractDatabase<ItemData, Item>
    {
        public override void Initialise()
        {
            foreach (ItemData data in allItemData)
            {
                Item item = ItemFactory.Create(data);
                item.Initialise();
                
                allItems.Add(item);
                routeTypeTable.Add(data, item);
            }
        }
    }
}