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
        
        [SerializeField] public GameObject additionalInformationBody;

        public Transform AchievementLocation;
        
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
                additionalInformationBody.SetActive(false);

            if (!IsActive)
                return;
            if (!IsDone()) 
                return;
            
            
            IsFinished = true;
            IsActive = false;
            infoLogoObject.SetActive(false);
            QuestData.IsCompleted = true;
            if (QuestData.QuestAchievement != null)
            {
                UIAchievement = QuestData.QuestAchievement;
            }
            QuestData.QuestAchievement?.SetActive(true);
            if (UIAchievement)
            {
                UIAchievement?.SetActive(true);
                //UIAchievement.transform.position = AchievementLocation.position;
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
            Debug.Log("Show additional information");
            foreach (var renderer in additionalInformationBody.GetComponentsInChildren<Renderer>())
                renderer.enabled = !additionalInformationBody.activeInHierarchy;
            additionalInformationBody.SetActive(!additionalInformationBody.activeInHierarchy);
        }

        protected override bool IsDone()
        {
            return questCheck != null && questCheck();
        }
    }
}