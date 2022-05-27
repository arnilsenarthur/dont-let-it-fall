using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Utils;
using TMPro;
using UnityEngine;

namespace DontLetItFall.UI
{
    public class QuestUI : MonoBehaviour
    {
        [Header("Quest")]
        [SerializeField]
        private QuestList questList;
        
        [SerializeField]
        private QuestManager questManager;
        
        private int currentQuestIndex => questList.selectedQuest;

        private int currentQuestID => questList.entries[currentQuestIndex].questID;
        
        private QuestListEntry currentQuest => questList.entries[currentQuestIndex];
        
        
        [Header("UI")]
        [SerializeField]
        private TextMeshProUGUI questText;

        private Animator _animator;
        
        private string _questText;
        private int _questTimer;
        
        private void Awake()
        {
            questList ??= Resources.Load<QuestList>("Data/Quests/QuestList");
            _animator = GetComponent<Animator>();
            UpdateQuest();
        }

        public void UpdateQuest()
        {
            _animator.Play("NewQuest");
            _questText = currentQuest.questDescription;
            
            StartCoroutine(ChangeText());
        }

        public void UpdateQuestDays()
        {
            if (!_questText.Contains("{d}")) return;
            
            var replace = _questText.Replace("{d}", questManager.getDayToEnd.ToString());
            questText.text = replace;
        }

        private IEnumerator ChangeText()
        {
            yield return new WaitForSeconds(1f);
            
            questText.text = $"{currentQuest.questName}\n" +
                             $"   {_questText}";
            
            if (_questText.Contains("{s}"))
            {
                _questTimer = (int)currentQuest.questTask[0].amount;
                StartCoroutine(UpdateQuestTimer());
                Debug.Log("s");
            }
            else if (_questText.Contains("{d}"))
            {
                _questText = questText.text;
                UpdateQuestDays();
                Debug.Log("d");
            }
        }
        
        private IEnumerator UpdateQuestTimer()
        {
            _questText = questText.text;
            while (_questTimer > 0)
            {
                _questTimer--;
                var replace = _questText.Replace("{s}", _questTimer.ToString());
                questText.text = replace;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
