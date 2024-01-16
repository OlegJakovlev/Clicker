using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UpgradeSystem
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "UpgradeSystem/UpgradeItem", order = 1)]
    public class UpgradeData : ScriptableObject
    {
        private static Regex regex = new Regex(@"\{(?<key>\w+)\}");

        [Header("Upgrade Data")]
        public string upgradeName;
        [TextArea] public string description;
        [TextArea] public string additionalDescription; // Describe stats going to change
        public int initialCost;

        [Header("Effects")]
        [SerializeField] public List<UpgradeEffect> upgradeEffects = new List<UpgradeEffect>();

        private int currentCost;

        // TODO: Replace with params object[] parameters
        // Because we want to pass anything we want, but it requires additional work for checking what kind of data type we pass in
        // I'm too lazy
        private string ResolveAdditionalDescription(List<string> parameters)
        {
            int index = 0;
            
            return regex.Replace(additionalDescription, 
                _=>
                {
                    string result = parameters[index];
                    index++;
                    return result;
                });
        }

        public string GetAdditionalDescription()
        {
            return ResolveAdditionalDescription(upgradeEffects.Select(effect => effect.value.ToString()).ToList());
        }

        public void CopyFrom(UpgradeData data)
        {
            upgradeName = data.upgradeName;
            description = data.description;
            additionalDescription = data.additionalDescription;
            initialCost = data.initialCost;

            foreach (UpgradeEffect effect in data.upgradeEffects)
            {
                upgradeEffects.Add(new UpgradeEffect(effect));
            }
        }
    }
}