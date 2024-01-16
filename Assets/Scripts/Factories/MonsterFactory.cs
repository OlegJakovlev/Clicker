using System;
using System.Collections.Generic;
using Creatures;
using UnityEngine;

namespace Factories
{
    public static class CharacterFactory
    {
        private enum CharacterDataType
        {
            MonsterData = 1,
            PlayerData = 2,
        }
        
        private static readonly IReadOnlyDictionary<Type, CharacterDataType> CharacterDataTypes = new Dictionary<Type, CharacterDataType>()
        {
            { typeof(MonsterData), CharacterDataType.MonsterData },
            { typeof(PlayerData), CharacterDataType.PlayerData }
        };

        public static Character Create(CharacterData data)
        {
            switch (data)
            {
                case MonsterData monsterData:
                    return new Monster(monsterData);
                
                case PlayerData playerData:
                    return new Player(playerData);
                
                default:
                    Debug.LogError($"Invalid item created! Type: {data.GetType()}");
                    return null;
            }
        }
        
        public static Character Clone(Character character)
        {
            CharacterDataType characterDataType = CharacterDataTypes[character.data.GetType()];
            CharacterData clonedData = null;
            
            switch (characterDataType)
            {
                case CharacterDataType.MonsterData:
                    clonedData = ScriptableObject.CreateInstance<MonsterData>();
                    clonedData.CopyFrom(character.data);
                    break;
                
                case CharacterDataType.PlayerData:
                    clonedData = ScriptableObject.CreateInstance<PlayerData>();
                    clonedData.CopyFrom(character.data);
                    break;
            }
            
            return Create(clonedData);
        }
    }
}