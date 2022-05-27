using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Data;
using DontLetItFall.Variables;
using UnityEngine;

namespace DontLetItFall
{
    public class PlayerManager : MonoBehaviour
    {
        #region PlayerStats
        
        [Space(1f)]
        [Header("Player Stats")]

        [SerializeField]
        private PlayerStats playerStats;
        [SerializeField]
        private StatsUI[] playerStatsImage;

        [SerializeField] 
        private float lifeDecayRate = 1f;

        private float LifeDecay
        {
            get
            {
                int i = 4;
                if (playerEnergy <= 0) i--;
                if (playerFood <= 0) i--;
                if (playerWater <= 0) i--;
                return lifeDecayRate / i;
            }    
        }
        
        [SerializeField] 
        private float energyDecayRate = 1f;
        [SerializeField] 
        private float foodDecayRate = 1f;
        [SerializeField] 
        private float waterDecayRate = 1f;
        
        [SerializeField]
        private bool energyRegeneration = true;

        #region StatsVariables
        
        #region CurrentStats
        private float playerLife
        {
            get => playerStats.currentLifeLevel.value; 
            set => playerStats.currentLifeLevel.value = value;
        }
        private float playerEnergy
        {
            get => playerStats.currentEnergyLevel.value; 
            set => playerStats.currentEnergyLevel.value = value;
        }
        private float playerFood
        {
            get => playerStats.currentFoodLevel.value; 
            set => playerStats.currentFoodLevel.value = value;
        }
        private float playerWater
        {
            get => playerStats.currentWaterLevel.value; 
            set => playerStats.currentWaterLevel.value = value;
        }
        #endregion
        
        #region MaxStats
        private float playerLifeMax => playerStats.maxLifeLevel.value;
        private float playerEnergyMax => playerStats.maxEnergyLevel.value;
        private float playerFoodMax => playerStats.maxFoodLevel.value;
        private float playerWaterMax => playerStats.maxWaterLevel.value;
        #endregion
        
        #endregion
        
        #endregion

        #region ShipStats
        
        [Space(1f)]
        [Header("Ship Stats")]
        [SerializeField]
        private Value<float> currentFuel;        
        [SerializeField]
        private Value<float> maxFuel;
        [SerializeField]
        private StatsUI[] shipStatsImage;
        [SerializeField] 
        private float fuelDecayRate = 1f;
        
        #endregion
        
        [Space]
        [Header("Player UI")]
        [SerializeField]
        private HUDScript hud;
        
        private void Start()
        {
            ResetPlayerStats();
        }

        private void ResetPlayerStats()
        {
            playerStats.currentLifeLevel.value = playerLifeMax;
            playerStats.currentEnergyLevel.value = playerEnergyMax;
            playerStats.currentFoodLevel.value = playerFoodMax;
            playerStats.currentWaterLevel.value = playerWaterMax;
            
            currentFuel.value = maxFuel.value;
        }

        void Update()
        {
            PlayerStatsUpdate();

            ShipStatsUpdate();
            
            if(playerLife <= 0)
            {
                hud.DeathScreen();
            }
        }
        
        private void PlayerStatsUpdate()
        {
            if (playerLife > 0 && playerEnergy <= 0 && (playerFood <= 0 || playerWater <= 0))
            {
                playerLife -= LifeDecay * Time.deltaTime;
            }

            if (energyRegeneration)
            {
                if(playerEnergy < playerEnergyMax)
                    playerEnergy += energyDecayRate * Time.deltaTime;
            }
            else if (playerEnergy > 0)
            {
                playerEnergy -= energyDecayRate * Time.deltaTime;
            }

            if (playerFood > 0)
            {
                if (energyRegeneration) 
                    playerFood -= (foodDecayRate * 1.25f) * Time.deltaTime;
                else
                    playerFood -= foodDecayRate * Time.deltaTime;
            }
            
            if (playerWater > 0)
            {
                if (energyRegeneration)
                    playerWater -= (waterDecayRate * 1.25f) * Time.deltaTime;
                else
                    playerWater -= waterDecayRate * Time.deltaTime;
            }

            if (playerWater <= 0 && playerFood <= 0)
            {
                energyRegeneration = false;
            }
        }
        
        private void ShipStatsUpdate()
        {
            if (currentFuel.value > 0)
            {
                currentFuel.value -= fuelDecayRate * Time.deltaTime;
            }
        }
        
        public void AddFuel(float value)
        {
            currentFuel.value += value;
            
            if (currentFuel.value > maxFuel.value)
                currentFuel.value = maxFuel.value;
        }
    }
}
