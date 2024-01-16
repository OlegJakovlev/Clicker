using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public interface IDatabase
    {
        void Initialise();
    }
    
    [Serializable]
    public abstract class AbstractDatabase<T, K> : MonoBehaviour, IDatabase
    {
        [SerializeField] protected List<T> allItemData = new List<T>();

        protected List<K> allItems = new List<K>();
        protected Dictionary<T, K> routeTypeTable = new Dictionary<T, K>();

        public abstract void Initialise();

        public IReadOnlyList<K> GetAllItems()
        {
            return allItems;
        }
    }
}