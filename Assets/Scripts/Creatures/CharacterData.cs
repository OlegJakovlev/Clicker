using System;
using UnityEngine;

namespace Creatures
{
    [Serializable]
    [CreateAssetMenu(fileName = "Name", menuName = "Character/Character", order = 1)]
    public class CharacterData : ScriptableObject
    {
        [Header("Base data")]
        public int maxHealth;
        public int maxShield;
        public int attackPower;
        public int baseKillAward;

        public virtual void CopyFrom(CharacterData data)
        {
            maxHealth = data.maxHealth;
            maxShield = data.maxShield;
            attackPower = data.attackPower;
            baseKillAward = data.baseKillAward;
        }
    }
}