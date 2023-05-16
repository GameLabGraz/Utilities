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
        [SerializeField] private GameObject infoLogoObject;

        [SerializeField] private GameObject UIAchievement;
        
        [SerializeField] public GameObject AdditionalInformationBody;
        
        private bool hasAdditionalInformation;
        
        public delegate bool QuestCheck();
        public QuestCheck questCheck;

        private void Start()
        {
            if (UIAchievement)
                UIAchievement.SetActive(false);
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
            infoLogoObject.SetActive(false);
            AdditionalInformationBody.SetActive(false);
            QuestData.IsCompleted = true;
            if (QuestData.QuestAchievement != null)
            {
                UIAchievement = QuestData.QuestAchievement;
            }
            QuestData.QuestAchievement?.SetActive(true);
            if (UIAchievement)
            {
                UIAchievement?.SetActive(true);
                UIAchievement?.GetComponentInChildren<ParticleSystem>()?.Play();
                Destroy(UIAchievement, 3);
            }
            onQuestFinished.Invoke();
        }

        public bool HasAdditionalInformation
        {
            get => hasAdditionalInformation;
            set
            {
                hasAdditionalInformation = value;
                if (infoLogoObject != null) infoLogoObject.SetActive(true);
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