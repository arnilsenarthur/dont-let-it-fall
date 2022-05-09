using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontLetItFall.Utils
{
    [System.Serializable]
    public class LootTableEntry 
    {
        public GameObject prefab;
        public int weight;
    }

    [CreateAssetMenu(fileName = "LootTable", menuName = "DLIF/LootTable", order = 0)]
    public class LootTable : ScriptableObject
    {
        public List<LootTableEntry> entries;

        private bool _isInitialized = false;
        private int _totalWeight = 0;

        public GameObject GetRandom()
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            int random = Random.Range(0, _totalWeight);

            foreach (LootTableEntry entry in entries)
            {
                if (entry.weight >= random)
                {
                    return entry.prefab;
                }

                random -= entry.weight;
            }

            return null;
        }

        private void Initialize()
        {
            int totalWeight = 0;
            foreach (var entry in entries)
            {
                totalWeight += entry.weight;
            }

            _totalWeight = totalWeight;
        }
    }
}