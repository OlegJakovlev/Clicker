using Creatures;

namespace UpgradeSystem
{
    public class Upgrade
    {
        public UpgradeData data { get; }

        public Upgrade(UpgradeData data)
        {
            this.data = data;
        }

        public void Initialise()
        {
            
        }

        public void ApplyTo(Character character)
        {
            foreach (UpgradeEffect upgradeEffect in data.upgradeEffects)
            {
                switch (upgradeEffect.updateType)
                {
                    case UpgradeEffectType.ATTACK:
                        character.CombatManager.IncreaseAttackPower(upgradeEffect.value);
                        break;
                    
                    case UpgradeEffectType.HEALTH:
                        character.CombatManager.IncreaseMaxHealth(upgradeEffect.value);
                        break;
                    
                    case UpgradeEffectType.SHIELD:
                        character.CombatManager.IncreaseMaxShield(upgradeEffect.value);
                        break;
                }
            }
        }
    }
}