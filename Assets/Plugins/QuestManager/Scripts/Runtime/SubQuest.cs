using System;
using Antares.Evaluation.LearningContent;
using GEAR.Localization;
using GEAR.Localization.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameLabGraz.QuestManager
{
    public class SubQuest : Quest
    {
        [SerializeField] private GameObject _infoLogoObject;

        [SerializeField] private GameObject _uIAchievement;
        
        [SerializeField] public GameObject AdditionalInformationBody;
        
        private bool _hasAdditionalInformation;
        
        public delegate bool QuestCheck();
        public QuestCheck questCheck;

        private void Start()
        {
            if (_uIAchievement)
                _uIAchievement.SetActive(false);
        }

        private new void Update()
        {
            if (IsHidden)
                AdditionalInformationBody.SetActive(false);

            if (!IsActive)
                return;
            if (!IsDone()) 
                return;
            
            
            IsFinished = true;
            IsActive = false;
            _infoLogoObject.SetActive(false);
            AdditionalInformationBody.SetActive(false);
            QuestData.IsCompleted = true;
            if (QuestData.QuestAchievement != null)
            {
                _uIAchievement = QuestData.QuestAchievement;
            }
            QuestData.QuestAchievement?.SetActive(true);
            if (_uIAchievement)
            {
                _uIAchievement?.SetActive(true);
                _uIAchievement?.GetComponentInChildren<ParticleSystem>()?.Play();
                Destroy(_uIAchievement, 3);
            }
            onQuestFinished.Invoke();
        }

        public bool HasAdditionalInformation
        {
            get => _hasAdditionalInformation;
            set
            {
                _hasAdditionalInformation = value;
                if (_infoLogoObject != null) _infoLogoObject.SetActive(true);
            }
        }
        
        public void ShowAdditionalInformation()
        {
            foreach (var renderer in AdditionalInformationBody.GetComponentsInChildren<Renderer>())
                renderer.enabled = !AdditionalInformationBody.activeInHierarchy;
            AdditionalInformationBody.SetActive(!AdditionalInformationBody.activeInHierarchy);
        }

        protected override bool IsDone()
        {
            return questCheck != null && questCheck();
        }
    }
}