using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DontLetItFall.Variables;
using DontLetItFall.Entity.Player;

namespace DontLetItFall.Inventory
{
    public interface IInteractable {
        void Interact(EntityPlayer player);
        void OnStartCanInteract();
        void OnEndCanInteract();
    }

    public class KitchenTable : MonoBehaviour, IInteractable
    {
        public Value<float> foodLevel;
        public Value<float> maxFoodLevel;

        public Value<float> waterLevel;
        public Value<float> maxWaterLevel;

        public GameObject kitchenInfo;

        public KitchenInventory inventory;

        public void Interact(EntityPlayer player)
        {
            float neededFood = (maxFoodLevel.value - foodLevel.value);
            float neededWater = (maxWaterLevel.value - waterLevel.value);

            float food = inventory.GetAmount(1);
            float water = inventory.GetAmount(2);

            Debug.Log(maxFoodLevel.value + "  " + foodLevel.value + "  " + food);


            if(neededFood > 0)
            {
                inventory.RemoveAmount(1, Mathf.Min(food, neededFood));
                foodLevel.value += Mathf.Min(food, neededFood);

                Debug.Log("Need: " + neededFood + " Had: " + food);
                Debug.Log("Ate: " + foodLevel.value + " " + maxFoodLevel.value);
            }

            if(neededWater > 0)
            {
                inventory.RemoveAmount(2, Mathf.Min(water, neededWater));
                waterLevel.value += Mathf.Min(water, neededWater);
            }
        }

        public void OnStartCanInteract()
        {
            kitchenInfo.SetActive(true);
        }

        public void OnEndCanInteract()
        {
            kitchenInfo.SetActive(false);
        }
    }
}