using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DontLetItFall.Variables;

namespace DontLetItFall.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public List<int> acceptedIds;
        public List<InventoryItem> currentItems;

        public Value<int> currentItemCount;

        private void FixedUpdate()
        {
            currentItemCount.value = currentItems.Count;
        }

        public float GetAmount(int id)
        {
            float amount = 0;
            foreach (InventoryItem item in currentItems)
            {
                if (item.itemId == id)
                    amount += item.GetAmount();
            }
            return amount;
        }

        public void RemoveAmount(int id,float amount)
        {
            foreach (InventoryItem item in currentItems)
            {
                if (item.itemId == id)
                    item.RemoveAmount(amount);
            }
        }
    }
}