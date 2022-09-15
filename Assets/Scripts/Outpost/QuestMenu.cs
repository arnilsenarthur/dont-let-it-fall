using System.Collections;
using System.Collections.Generic;
using DontLetItFall.UI;
using DontLetItFall.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DontLetItFall.UI
{
    public class QuestMenu : MonoBehaviour
    {
        [Header("Data")] 
        [SerializeField] private QuestList questData;
        private QuestListEntry selectQuest;

        [Space] [Header("UI")] [SerializeField]
        private RectTransform questContent;

        [SerializeField] private GameObject questPrefab;
        [SerializeField] private Image questBanner;
        [SerializeField] private TextMeshProUGUI questTitle;

        [SerializeField] private RectTransform rewardContainer;
        [SerializeField] private GameObject rewardPrefab;
        [SerializeField] private Image weatherIcon;
        [SerializeField] private TextMeshProUGUI difficultyText;

        [SerializeField] private TextMeshProUGUI questDescription;
        [SerializeField] private RectTransform taskContent;
        [SerializeField] private GameObject taskPrefab;

        private void Start()
        {
            DestroyChildren(questContent);

            foreach (var quest in questData.entries)
            {
                var questObject = Instantiate(questPrefab, questContent).GetComponent<QuestContainer>();
                questObject.SetQuest(quest);
                questObject.questMenu = this;
            }
            
            SelectQuest(questData.entries[0]);
        }

        public void SelectQuest(QuestListEntry quest)
        {
            if (selectQuest == quest) return;
            
            DestroyChildren(rewardContainer);
            DestroyChildren(taskContent);

            questTitle.text = quest.questName;
            questDescription.text = quest.questDescription;
            difficultyText.text = quest.GetDifficulty();
            //statusIcon.sprite = quest.status.GetIcon();
            weatherIcon.sprite = quest.weatherEvent.icon;
            questBanner.sprite = quest.questBanner;
            foreach (var reward in quest.questRewards)
            {
                var rewardIcon = Instantiate(rewardPrefab, rewardContainer).transform;
                rewardIcon.GetChild(0).GetComponent<Image>().sprite = reward.icon;
            }

            foreach (var task in quest.questTask)
            {
                var taskObject = Instantiate(taskPrefab, taskContent);
                taskObject.GetComponentInChildren<TextMeshProUGUI>().text = task.taskName;
            }
            
            selectQuest = quest;
        }

        private void DestroyChildren(Transform parent)
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
