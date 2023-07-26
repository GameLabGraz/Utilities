using System.Collections.Generic;
using UnityEngine;

namespace GameLabGraz.QuestManager.View
{
    [RequireComponent(typeof(QuestManager))]
    public abstract class QuestView : MonoBehaviour
    {
        [SerializeField] protected GameObject MainQuestBody;
        [SerializeField] protected GameObject SubQuestBody;
        [SerializeField] protected GameObject DataObjectRoot;
        [SerializeField] protected Vector2 MaxImageSize = new Vector2(800,500);

        private QuestManager _questManager;

        protected abstract void InitializeQuestView(List<QuestData> mainQuests);
        protected abstract void ShowAdditionalInformation(SubQuest subQuest);
        protected abstract void SetAdditionalInformation(QuestData subQuestData);

        private void Start()
        {
            _questManager = GetComponent<QuestManager>();

            QuestManager.OnQuestsRead.AddListener(() =>
            {
                InitializeQuestView(_questManager.MainQuests);
                _questManager.ActivateNextMainQuest();
            });
        }


    }

}
