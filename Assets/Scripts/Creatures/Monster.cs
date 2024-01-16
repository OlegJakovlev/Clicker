using UnityEngine;

namespace Creatures
{
    public class Monster : Character
    {
        private float lastAttackTime;
        
        public Monster(MonsterData data)
        {
            this.data = data;
        }

        public override void Initialise()
        {
            base.Initialise();
            CombatManager.Reset();
        }

        public override void TryAttack(Character target)
        {
            float currentTime = Time.time;
            if (currentTime >= lastAttackTime + ((MonsterData)data).attackDelay)
            {
                CombatManager.Damage(target);
                lastAttackTime = currentTime;
            }
        }

        public void ScaleWithWave(int currentWave)
        {
            CombatManager.IncreaseMaxHealth(Mathf.RoundToInt(Mathf.Pow(Mathf.Log10(CombatManager.CurrentHealth * currentWave), 5.5f)));
            CombatManager.IncreaseMaxShield(Mathf.RoundToInt(Mathf.Pow(Mathf.Log10(CombatManager.CurrentShield * currentWave), 5.5f)));
            CombatManager.IncreaseAttackPower(Mathf.RoundToInt(Mathf.Log(0.1f * currentWave)));
        }
    }
}
