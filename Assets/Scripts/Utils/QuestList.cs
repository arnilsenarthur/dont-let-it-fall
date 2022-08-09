using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace DontLetItFall.Utils
{
    [System.Serializable]
    public class QuestRewardsEntry
    {
        public QuestReward reward;
        public int amount;
        public Sprite icon => reward.rewardIcon;
    }
    
    [System.Serializable]
    public class QuestTaskEntry
    {
        public string taskName;
        public TaskType type;
        public GameObject target;
        [Tooltip("For balance is 0")]
        public float amount;
        
        public enum TaskType
        {
            Balance,
            Collect,
            Survive_Days,
            Survive_Seconds,
            Survive_ThisDay,
            Survive_ThisNight
        }
    }

    [System.Serializable]
    public class QuestListEntry
    {
        [Header("About")]
        public string questName;
        public int questID;
        public Sprite questBanner;
        [Space(1f)]
        [TextArea(0,3)]
        public string questResume;
        [TextArea(5,15)]
        public string questDescription;
        [Range(1,10)]
        public int difficulty;
        public int questStatus;
        public bool isCompleted;

        [Space] [Header("Stats")] 
        public WeatherEvent weatherEvent;
        public List<QuestTaskEntry> questTask;
        public List<QuestRewardsEntry> questRewards;
        
        public string GetDifficulty()
        {
            return ToRoman(difficulty);
        }
        
        private string ToRoman(int number)
        {
            if (number < 1) return string.Empty;
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            return string.Empty;
        }
    }
    
    [CreateAssetMenu(fileName = "QuestList", menuName = "DLIF/Quest/QuestList", order = 0)]
    public class QuestList : ScriptableObject
    {
        public int selectedQuest;
        
        public List<QuestListEntry> entries;
        
        public void SelectQuest(int quest, bool isID = true)
        {
            if (isID)
            {
                for (int i = 0; i < entries.Count; i++)
                {
                    if(entries[i].questID == quest)
                    {
                        selectedQuest = i;
                        break;
                    }
                }
            }
            else
            {
                selectedQuest = quest;
            }
        }
        
        public List<QuestRewardsEntry> GetReward(int questID)
        {
            foreach (var entry in entries)
            {
                if (entry.questID == questID)
                {
                    return entry.questRewards;
                }
            }
            return null;
        }
        
        public List<QuestRewardsEntry> GetReward()
        {
            return entries[selectedQuest].questRewards;
        }
        
        public void SetQuestCompleted(int questID)
        {
            foreach (var entry in entries)
            {
                if (entry.questID == questID)
                {
                    entry.isCompleted = true;
                }
            }
        }
        
        public void SetQuestCompleted()
        {
            //entries[selectedQuest].isCompleted = true;
            NextQuest();
        }

        public void NextQuest()
        {
            if(selectedQuest < entries.Count)
            {
                selectedQuest++;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (var qList in entries)
            {
                if(qList.weatherEvent == null)
                {
                    qList.weatherEvent = Resources.Load<WeatherEvent>("Data/Weather/Clear");
                }
                
                foreach (var tasks in qList.questTask)
                {
                    tasks.taskName = tasks.type.ToString();
                }
            }
        }
#endif
    }
}
