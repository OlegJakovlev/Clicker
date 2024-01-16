using Creatures;

namespace InventorySystem.Items
{
    public interface IUsable
    {
        public bool Use(Character character);
    }

    public interface ISellable
    {
        public void Sell();
    }

    public interface IEquipable
    {
        public bool Equip(Character character);

        public void Unequip(Character character);
    }
    
    public abstract class Item
    {
        public ItemData.ItemData data { get; protected set; }

        public int CurrentPrice { get; protected set; }
        public int CurrentLevel { get; protected set; } = 1;

        public virtual void Initialise()
        {
        }

        public virtual void ScaleWithLevel(int level)
        {
            CurrentLevel = level;
        }
        
        protected void UpdateItemPrice()
        {
            CurrentPrice = data.basePrice * CurrentLevel;
        }
    }
}