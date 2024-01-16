using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility
{
    public static class RandomUtility
    {
        public static List<T> GetRandomElements<T>(List<T> list, int amount)
        {
            List<T> result = new List<T>();
            List<T> allItemsCopy = new List<T>(list);

            int randomIndex;
            for (int i = 0; i < amount && allItemsCopy.Count != 0; i++)
            {
                randomIndex = Random.Range(0, allItemsCopy.Count);
                result.Add(allItemsCopy[randomIndex]);
            }

            return result;
        }
        
        public static List<T> GetRandomElements<T>(IReadOnlyList<T> list, int amount)
        {
            List<T> result = new List<T>();
            List<T> allItemsCopy = list.ToList();

            int randomIndex;
            for (int i = 0; i < amount && allItemsCopy.Count != 0; i++)
            {
                randomIndex = Random.Range(0, allItemsCopy.Count);
                result.Add(allItemsCopy[randomIndex]);
            }

            return result;
        }
        
        public static List<T> GetRandomUniqueElements<T>(List<T> list, int amount)
        {
            List<T> result = new List<T>();
            List<T> allItemsCopy = new List<T>(list);

            int randomIndex;
            for (int i = 0; i < amount && allItemsCopy.Count != 0; i++)
            {
                randomIndex = Random.Range(0, allItemsCopy.Count);
                result.Add(allItemsCopy[randomIndex]);
                allItemsCopy.RemoveAt(randomIndex);
            }

            return result;
        }
        
        public static List<T> GetRandomUniqueElements<T>(IReadOnlyList<T> list, int amount)
        {
            List<T> result = new List<T>();
            List<T> allItemsCopy = list.ToList();

            int randomIndex;
            for (int i = 0; i < amount && allItemsCopy.Count != 0; i++)
            {
                randomIndex = Random.Range(0, allItemsCopy.Count);
                result.Add(allItemsCopy[randomIndex]);
                allItemsCopy.RemoveAt(randomIndex);
            }

            return result;
        }
    }
}