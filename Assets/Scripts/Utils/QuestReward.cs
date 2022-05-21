using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DontLetItFall.Utils
{
    [CreateAssetMenu(fileName = "Reward", menuName = "DLIF/Quest/Reward", order = 0)]
    public class QuestReward : ScriptableObject
    {
        public Rewards reward;
        public Sprite rewardIcon;
        public GameObject rewardPrefab;
        public UnityEvent rewardEvent;
        
        public enum Rewards
        {
            Coin,
            Meat,
            Fish,
            Water,
            Coal,
            Life,
            Wood,
            Cloth,
            Metal
        }
    }
}
