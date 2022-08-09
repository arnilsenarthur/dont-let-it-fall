using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DontLetItFall.UI
{
    public class QuestContainer : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI questTitle;
        [SerializeField] 
        private TextMeshProUGUI questDescription;

        [SerializeField] 
        private RectTransform rewardContainer;
        [SerializeField] 
        private GameObject rewardPrefab;
        [SerializeField] 
        private Image weatherIcon;
        [SerializeField] 
        private TextMeshProUGUI difficultyText;
        [SerializeField] 
        private Image statusIcon;

        [SerializeField] 
        private QuestListEntry questEntry;
        
        public QuestMenu questMenu;

        public void SetQuest(QuestListEntry quest)
        {
            foreach (Transform child in rewardContainer)
            {
                Destroy(child.gameObject);
            }
            
            questEntry = quest;

            questTitle.text = questEntry.questName;
            questDescription.text = questEntry.questDescription;
            difficultyText.text = questEntry.GetDifficulty();
            //statusIcon.sprite = quest.status.GetIcon();
            weatherIcon.sprite = questEntry.weatherEvent.icon;
            foreach (var reward in questEntry.questRewards)
            {
                var rewardIcon = Instantiate(rewardPrefab, rewardContainer).transform;
                rewardIcon.GetChild(0).GetComponent<Image>().sprite = reward.icon;
            }
        }

        public void SelectQuestMenu()
        {
            questMenu.SelectQuest(questEntry);
        }
    }
}
