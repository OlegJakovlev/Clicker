using InventorySystem;
using ShopSystem;

namespace Creatures
{
    public class Player : Character
    {
        public BalanceHandler BalanceHandler { get; private set; }

        public Player(CharacterData data)
        {
            this.data = data;
        }
        
        public override void Initialise()
        {
            base.Initialise();
            
            BalanceHandler = new BalanceHandler();

            CombatManager.KilledCharacter += (victim) =>
            {
                BalanceHandler.AddBalance(victim.ExperienceHandler.GetKillGoldReward());
            };

            CombatManager.Died += Reset;
        }

        protected override void InitialiseInventory()
        {
            Inventory = new InventoryHandler(this, ((PlayerData)data).maxItemsInInventory);
        }
    }
}
