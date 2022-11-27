using UnityEngine;
using UnityEngine.Events;

namespace GameLabGraz.QuestManager
{
    public abstract class Quest : MonoBehaviour
    {
        [Header("Status Lamp")]
        [SerializeField] private GameObject activeLamp;
        [SerializeField] private GameObject deactiveLamp;

        [SerializeField] public GameObject finishLine;
        
        public UnityEvent onQuestFinished = new UnityEvent();

        public QuestData QuestData{ get; set; }
        public bool IsHidden { get; set; }
        public bool IsFinished { get; set; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                QuestData?.QuestHint?.SetActive(value);

               _isActive = value;
                if (deactiveLamp != null)
                {
                    deactiveLamp.SetActive(!value);
                    deactiveLamp.GetComponent<Renderer>().enabled = !IsHidden;
                }
                if (activeLamp != null)
                {
                    activeLamp.SetActive(value);
                    activeLamp.GetComponent<Renderer>().enabled = !IsHidden;
                }
            }
        }
        protected abstract bool IsDone();

        protected virtual void OnEnable()
        {
            IsActive = _isActive;
        }

        protected virtual void Update()
        {
            if (!IsActive)
                return;
            
            if (!IsDone()) 
                return;
            
            IsFinished = true;
            IsActive = false;
            QuestData.QuestAchievement?.SetActive(true);
            onQuestFinished.Invoke();
        }
        
    }
}