using Creatures;
using Factories;

namespace Database
{
    public class EnemyCreatureDatabase : AbstractDatabase<MonsterData, Monster>
    {
        public override void Initialise()
        {
            foreach (MonsterData data in allItemData)
            {
                Monster monster = (Monster)CharacterFactory.Create(data);
                monster.Initialise();
                
                allItems.Add(monster);
                routeTypeTable.Add(data, monster);
            }
        }
    }
}