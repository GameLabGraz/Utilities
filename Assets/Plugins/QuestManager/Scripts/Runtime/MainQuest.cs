using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GameLabGraz.QuestManager
{
    public class MainQuest : Quest
    {
        private int _subQuestIndex = 0;
        
        private List<SubQuest> _subQuests = new List<SubQuest>();
        
        protected override bool IsDone()
        {
            return _subQuests.All(subQuest => subQuest.IsFinished);
        }

        public void ActivateNextSubQuest()
        {
            if (_subQuestIndex >= _subQuests.Count)
                return;

            var activeSubQuest = _subQuests[_subQuestIndex++];
            activeSubQuest.IsActive = true;
            activeSubQuest.onQuestFinished.AddListener(() =>
            {
                if (activeSubQuest.FinishLine != null)
                {
                    activeSubQuest.FinishLine.SetActive(true);
                    activeSubQuest.FinishLine.GetComponent<Renderer>().enabled = !IsHidden;
                }
                else
                {
                    activeSubQuest.gameObject.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                }
                activeSubQuest.IsActive = false;
                ActivateNextSubQuest();
            });
        }
        
        public IEnumerator HideSubQuests()
        {
            yield return new WaitForSeconds (3);
            foreach (var subQuest in _subQuests)
            {
                subQuest.gameObject.SetActive(!subQuest.gameObject.activeInHierarchy);
            }
        }

        public IEnumerator MoveMainQuestToBottom()
        {
            yield return new WaitForSeconds (3);
            transform.SetAsLastSibling();
        }
        
        public void AddSubQuest(SubQuest subQuest)
        {
            _subQuests.Add(subQuest);
        }

        public int GetSubQuestCount()
        {
            return _subQuests.Count;
        }
    }
}