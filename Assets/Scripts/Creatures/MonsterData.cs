using System;
using UnityEngine;

namespace Creatures
{
    [Serializable]
    [CreateAssetMenu(fileName = "Name", menuName = "Character/Monster", order = 2)]
    public class MonsterData : CharacterData
    {
        [Header("Visuals")]
        public Sprite icon;
        
        [Header("Attack data")]
        public float attackDelay;

        [Header("Special flags")]
        public bool isBoss;

        public override void CopyFrom(CharacterData data)
        {
            base.CopyFrom(data);
            
            MonsterData castedData = (MonsterData)data;
            icon = castedData.icon;
            attackDelay = castedData.attackDelay;
            isBoss = castedData.isBoss;
        }
    }
}