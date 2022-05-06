using UnityEngine;
using DontLetItFall.Variables;

namespace DontLetItFall.Data
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "DLIF/PlayerStats", order = 0)]
    public class PlayerStats : ScriptableObject
    {
        public Value<float> maxFoodLevel;
        public Value<float> currentFoodLevel;

        public Value<float> maxWaterLevel;
        public Value<float> currentWaterLevel;

        public Value<float> maxEnergyLevel;
        public Value<float> currentEnergyLevel;
    }
}