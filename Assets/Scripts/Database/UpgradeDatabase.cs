using Factories;
using UpgradeSystem;

namespace Database
{
    public class UpgradeDatabase : AbstractDatabase<UpgradeData, Upgrade>
    {
        public override void Initialise()
        {
            foreach (UpgradeData data in allItemData)
            {
                Upgrade upgrade = UpgradeFactory.Create(data);
                upgrade.Initialise();
                
                allItems.Add(upgrade);
                routeTypeTable.Add(data, upgrade);
            }
        }
    }
}