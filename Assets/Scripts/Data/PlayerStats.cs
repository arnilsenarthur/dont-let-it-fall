using UnityEngine;
using DontLetItFall.Variables;

namespace DontLetItFall.Data
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "DLIF/PlayerStats", order = 0)]
    public class PlayerStats : ScriptableObject
    {
        public float health;
        public float maxHealth;

        public float foodLevel;
        public float maxFoodLevel;

        public float waterLevel;
        public float maxWaterLevel;

        public float energyLevel;
        public float maxEnergyLevel;

        public AnimationCurve walkSpeedMultiplierDependingOnEnergy;
        public AnimationCurve balanceMultiplierDependingOnEnergy;
    }
}