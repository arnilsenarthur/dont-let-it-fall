using System.Collections;
using System.Collections.Generic;
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
        private string _taskName = "S";
        public enum TaskType
        {
            Balance,
            Collect,
            Survive_Days,
            Survive_Seconds,
            Survive_ThisDay,
            Survive_ThisNight
        }
        public TaskType type;
        public GameObject target;
        [Tooltip("For balance is 0")]
        public float amount;
    }

    [System.Serializable]
    public class QuestListEntry
    {
        public string questName;
        public int questID;
        [Multiline]
        public string questDescription;
        public QuestTaskEntry questTask;
        public bool isCompleted;
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
            entries[selectedQuest].isCompleted = true;
            NextQuest();
        }

        public void NextQuest()
        {
            if(selectedQuest < entries.Count)
            {
                selectedQuest++;
            }
        }
    }
}
