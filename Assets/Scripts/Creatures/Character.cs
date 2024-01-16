using CombatSystem;
using ExperienceSystem;
using InventorySystem;
using UpgradeSystem;

namespace Creatures
{
    public abstract class Character
    {
        public CharacterData data { get; protected set; }
        
        public InventoryHandler Inventory { get; protected set; }
        public CombatHandler CombatManager { get; private set; }
        public ExperienceHandler ExperienceHandler { get; private set; }
        public UpgradeHandler UpgradeHandler { get; private set; }

        public virtual void Initialise()
        {
            ExperienceHandler = new ExperienceHandler(this);
            CombatManager = new CombatHandler(this);
            UpgradeHandler = new UpgradeHandler(this);
            
            InitialiseInventory();

            CombatManager.KilledCharacter += (victim) =>
            {
                ExperienceHandler.IncreaseXP(victim.ExperienceHandler.GetKillXPReward());
            };
        }

        protected virtual void InitialiseInventory()
        {
            Inventory = new InventoryHandler(this, 5);
        }

        public virtual void TryAttack(Character target)
        {
            CombatManager.Damage(target);
        }

        public void Reset()
        {
            CombatManager.Reset();
        }
    }
}
