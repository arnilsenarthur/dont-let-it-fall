using System;
using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Utils;
using DontLetItFall.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace DontLetItFall
{
    public class QuestManager : MonoBehaviour
    {
        [Header("Ship")]
        [SerializeField] private Value<float> shipAngleX;
        [SerializeField] private Value<float> shipAngleZ;
        [SerializeField] private float angleSafetyMargin;

        [Header("World")] 
        [SerializeField] private Value<int> dayNumber;
        [SerializeField] private int taskDayEnd;
        [SerializeField] private WeatherEventManager weatherEventManager;
        public int getDayToEnd => taskDayEnd - dayNumber.value;
        
        [Header("Quest")]
        [SerializeField]
        private QuestList questList;
        
        private int currentQuestIndex => questList.selectedQuest;
        
        private QuestListEntry currentQuest => questList.entries[currentQuestIndex];

        private int currentQuestID => currentQuest.questID;
        
        private QuestTaskEntry currentQuestTasks => currentQuest.questTask[0];
        
        [SerializeField] private int taskDones;

        private bool checkTask = false;
        
        private bool surviveChecking = false;

        [SerializeField]
        private UnityEvent onQuestCompleted;
        
        private void Awake()
        {
            questList ??= Resources.Load<QuestList>("Data/Quests/QuestList");
        }

        private void Start()
        {
            questList.selectedQuest = 0;
            StartCoroutine(TimeToCheck());
        }

        private void Update()
        {
            if (!checkTask) return;
            
            if (currentQuestTasks.type == QuestTaskEntry.TaskType.Balance)
            {
                BalanceQuestCheck();
            }
            else if (currentQuestTasks.type == QuestTaskEntry.TaskType.Survive_Seconds)
            {
                if (!surviveChecking)
                {
                    surviveChecking = true;
                    StartCoroutine(SurviveQuestCheck(currentQuestTasks.amount));
                }
            }
            else if (currentQuestTasks.type == QuestTaskEntry.TaskType.Survive_Days)
            {
                if (!surviveChecking)
                {
                    surviveChecking = true;
                    taskDayEnd = dayNumber.value + (int)currentQuestTasks.amount;
                }
                else if(taskDayEnd == dayNumber.value)
                {
                    surviveChecking = false;
                    TaskDone();
                }
            }
        }
        
        private void TaskDone()
        {
            Debug.Log($"Task {currentQuestID} Done");
            
            taskDones++;
            questList.SetQuestCompleted();
            onQuestCompleted.Invoke();
            weatherEventManager.ChangeWeatherEvent(currentQuest.weatherEvent);
        }

        private void BalanceQuestCheck()
        {
            if (Mathf.Abs(shipAngleX.value) < angleSafetyMargin)
            {
                if (Mathf.Abs(shipAngleZ.value) < angleSafetyMargin)
                {
                    TaskDone();
                }
            }
        }
        
        private IEnumerator SurviveQuestCheck(float time)
        {
            yield return new WaitForSeconds(time);
            
            TaskDone();
            surviveChecking = false;
        }
        
        private IEnumerator TimeToCheck()
        {
            yield return new WaitForSeconds(1);
            
            checkTask = true;
        }
    }
}
