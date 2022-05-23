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
        [TextArea]
        public string questDescription;
        public bool isCompleted;

        [Space] [Header("Stats")] 
        public WeatherEvent weatherEvent;
        public List<QuestTaskEntry> questTask;
        public List<QuestRewardsEntry> questRewards;
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
