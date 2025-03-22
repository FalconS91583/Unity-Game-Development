using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BramaBadura.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression = null;
        [SerializeField] private GameObject levelUpPartileEffect = null;
        [SerializeField] private bool shouldUseModifiers = false;

        public event Action onLevelUp;

        private int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel(float XP)
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }
        private void LevelUpEffect()
        {
            Instantiate(levelUpPartileEffect, transform);
        }

        public float GetHealth(Stat stat)
        {
            return (GetBaseStat(stat) + GetModifiers(stat)) * (1 + GetProcentageModifier(stat) / 100);
        }

        private float GetProcentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetProcentageModifiers(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetHealth(stat, characterClass, GetLevel());
        }

        private float GetModifiers(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifiers in provider.GetModifier(stat))
                {
                    total += modifiers;
                }
            }
            return total;   
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float currentXP = experience.GetPoints();
            int penulimateLevel = progression.GetLevel(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penulimateLevel; level++)
            {
                float XPToLevelup = progression.GetHealth(Stat.ExperienceToLevelUp, characterClass, level);
                if(XPToLevelup > currentXP)
                {
                    return level;
                }
            }

            return penulimateLevel + 1;

        }

        public int GetLevel()
        {
            if(currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }

            return currentLevel;
        }
    }
}


