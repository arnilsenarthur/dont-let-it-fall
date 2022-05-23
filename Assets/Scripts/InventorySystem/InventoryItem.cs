using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DontLetItFall.Variables;
using DontLetItFall.Entity;

namespace DontLetItFall.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        public int itemId;
        private Inventory _inventory;

        private void OnDisable()
        {
            if (_inventory != null)
                _inventory.currentItems.Remove(this);
        }

        public float GetAmount()
        {
            TimedEntity timedEntity = GetComponent<TimedEntity>();

            if (timedEntity != null)
                return timedEntity.maxTime.value - timedEntity.currentTime.value;
            else
                return 1;
        }

        public void RemoveAmount(float amount)
        {
            TimedEntity timedEntity = GetComponent<TimedEntity>();
            if (timedEntity != null)
                timedEntity.currentTime.value += amount;
        }

        private void OnTriggerEnter(Collider other)
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                _inventory = inventory;
                if (inventory.acceptedIds.Contains(itemId))
                {
                    if (inventory.currentItems.Contains(this))
                        return;

                    inventory.currentItems.Add(this);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                if (inventory.currentItems.Contains(this))
                {
                    inventory.currentItems.Remove(this);
                }
            }
        }

        public void RemoveFromInventory()
        {
            if (_inventory != null)
            {
                _inventory.currentItems.Remove(this);
                _inventory = null;
            }
        }
    }
}