using System;
using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Data;
using DontLetItFall.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DontLetItFall
{
    public class HUDScript : MonoBehaviour
    {
        [Header("Clock")]
        [SerializeField]
        private VariableTime time;
        [SerializeField]
        private VariableString weekDay;
        [SerializeField] 
        private Image timeClock;
        [SerializeField] 
        private TextMeshProUGUI weekDayText;
        private float timeValue => time.Value;
        
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
        
        
        [Space(1f)]
        [Header("Ship Stats")]
        [SerializeField]
        private float shipStats;
        [SerializeField]
        private StatsUI[] shipStatsImage;

        private void Start()
        {
            playerStatsImage[0].MaxValue = playerLifeMax;
            playerStatsImage[1].MaxValue = playerEnergyMax;
            playerStatsImage[2].MaxValue = playerFoodMax;
            playerStatsImage[3].MaxValue = playerWaterMax;
        }

        private void Update()
        {
            ClockUpdate();

            PlayerStatsUpdate();
        }

        private void PlayerStatsUpdate()
        {
            if (playerLife > 0 && (playerEnergy <= 0 || playerFood <= 0 || playerWater <= 0))
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


            playerStatsImage[0].CurrentValue = playerLife;
            playerStatsImage[1].CurrentValue = playerEnergy;
            playerStatsImage[2].CurrentValue = playerFood;
            playerStatsImage[3].CurrentValue = playerWater;
        }

        private void ClockUpdate()
        {
            if (timeValue >= .5f)
            {
                if (timeClock.fillClockwise)
                    timeClock.fillClockwise = false;

                timeClock.fillAmount = 1 - ((timeValue - .5f) / .5f);
            }
            else
            {
                if (!timeClock.fillClockwise)
                    timeClock.fillClockwise = true;

                timeClock.fillAmount = (timeValue) / .5f;
            }
            
            weekDayText.text = weekDay.ToString();
        }
    }
}
