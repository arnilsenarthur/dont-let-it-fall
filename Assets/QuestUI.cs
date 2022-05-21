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
        private int currentQuestIndex => questList.selectedQuest;

        private int currentQuestID => questList.entries[currentQuestIndex].questID;
        
        private QuestListEntry currentQuest => questList.entries[currentQuestIndex];
        
        [Header("UI")]
        [SerializeField]
        private TextMeshProUGUI questText;

        private Animator _animator;
        
        private void Awake()
        {
            questList ??= Resources.Load<QuestList>("Data/Quests/QuestList");
            _animator = GetComponent<Animator>();
            UpdateQuest();
        }

        public void UpdateQuest()
        {
            _animator.Play("NewQuest");
            StartCoroutine(ChangeText());
        }

        private IEnumerator ChangeText()
        {
            yield return new WaitForSeconds(1f);
            
            questText.text = $"{currentQuest.questName}\n" +
                             $"   {currentQuest.questDescription}";
        }
    }
}
