using UnityEngine;

namespace GameLabGraz.QuestManager
{
    [RequireComponent(typeof(Quest))]
    public abstract class QuestCheck : MonoBehaviour
    {
        private SubQuest _quest;

        private void Start()
        {
            InitCheck();

            _quest = GetComponent<SubQuest>();
            _quest.questCheck = CheckCompliance;
        }

        protected abstract void InitCheck();
        protected abstract bool CheckCompliance();
    }
}