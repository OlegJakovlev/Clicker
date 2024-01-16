using System;
using Creatures;
using UnityEngine;

namespace ExperienceSystem
{
    public class ExperienceHandler
    {
        public event Action LeveledUp;
        
        private Character characterRef;

        public int CurrentLevel { get; private set; }
        public int TotalSkillPoints { get; private set; }
        public int AvailableSkillPoints { get; private set; }

        private int currentXP;

        public ExperienceHandler(Character character)
        {
            characterRef = character;
        }
        
        public int GetKillXPReward()
        {
            return characterRef.data.baseKillAward * Mathf.Max(1, CurrentLevel);
        }

        public int GetKillGoldReward()
        {
            return characterRef.data.baseKillAward * Mathf.Max(1, CurrentLevel);
        }
        
        public void IncreaseXP(int xp)
        {
            if (xp < 0)
            {
                return;
            }

            currentXP += xp;

            while (currentXP > CurrentLevel * 2)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            currentXP -= CurrentLevel * 2;
            CurrentLevel+=1;
            AcquireSkillPoints(1);
            LeveledUp?.Invoke();
        }

        public void SpendSkillPoint(int skillPoints)
        {
            if (skillPoints < 0)
            {
                return;
            }
            
            TotalSkillPoints-=skillPoints;
            AvailableSkillPoints-=skillPoints;
        }

        private void AcquireSkillPoints(int skillPoints)
        {
            if (skillPoints < 0)
            {
                return;
            }
            
            TotalSkillPoints+=skillPoints;
            AvailableSkillPoints+=skillPoints;
        }
    }
}