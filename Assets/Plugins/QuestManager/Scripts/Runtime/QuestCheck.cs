using UnityEngine;

namespace GameLabGraz.QuestManager
{
    [RequireComponent(typeof(Quest))]
    public abstract class QuestCheck : MonoBehaviour
    {
        private SubQuest _subQuest;

        private void Start()
        {
            InitCheck();

            _subQuest = GetComponent<SubQuest>();
            _subQuest.questCheck = CheckCompliance;
        }

        protected abstract void InitCheck();
        protected abstract bool CheckCompliance();
    }
}