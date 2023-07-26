using UnityEngine;
using UnityEngine.Events;

namespace GameLabGraz.QuestManager
{
    public abstract class Quest : MonoBehaviour
    {
        [Header("Status Lamp")]
        [SerializeField] private GameObject _activeLamp;
        [SerializeField] private GameObject _deactiveLamp;

        [SerializeField] public GameObject FinishLine;
        
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
                if (_deactiveLamp != null)
                {
                    _deactiveLamp.SetActive(!value);
                    _deactiveLamp.GetComponent<Renderer>().enabled = !IsHidden;
                }
                if (_activeLamp != null)
                {
                    _activeLamp.SetActive(value);
                    _activeLamp.GetComponent<Renderer>().enabled = !IsHidden;
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