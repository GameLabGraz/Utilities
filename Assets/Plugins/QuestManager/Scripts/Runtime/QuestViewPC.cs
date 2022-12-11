using System;
using System.Collections.Generic;
using System.IO;
using GEAR.Localization;
using GEAR.Localization.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLabGraz.QuestManager
{ 
    public class QuestViewPC : QuestView
    {
        [SerializeField] protected Transform AchievementLocation;
        protected override void InitializeQuestView(List<QuestData> mainQuests)
        {
            foreach (var mainQuestData in mainQuests)
            {
                DataObjectRoot.SetActive(true);
                
                var mainQuestBody = Instantiate(MainQuestBody, DataObjectRoot.transform);
                mainQuestBody.name = mainQuestData.DefaultText;
                mainQuestBody.GetComponentInChildren<LocalizedTMP>().Key = mainQuestData.TranslationKey;
                mainQuestBody.GetComponentInChildren<TMP_Text>().text = LanguageManager.Instance.GetString(mainQuestData.TranslationKey);
                
                var mainQuestObject = mainQuestBody.GetComponent<MainQuest>();
                mainQuestObject.QuestData = mainQuestData;

                foreach (var subQuestData in mainQuestData.SubQuestData)
                {
                    var subQuestBody = Instantiate(SubQuestBody, mainQuestBody.transform);
                    subQuestBody.name = subQuestData.DefaultText;
                    subQuestBody.GetComponentInChildren<LocalizedTMP>().Key = subQuestData.TranslationKey;
                    subQuestBody.GetComponentInChildren<TMP_Text>().text = LanguageManager.Instance.GetString(subQuestData.TranslationKey);

                    var subQuestObject = subQuestBody.GetComponent<SubQuest>();
                    subQuestData.QuestBody = subQuestObject;
                    subQuestObject.QuestData = subQuestData;
                    subQuestObject.AchievementLocation = AchievementLocation;

                    var additionalInfoBtn = subQuestBody.GetComponentInChildren<Button>();
                    
                    if (subQuestData.AdditionalInformation != null)
                    {
                        subQuestObject.HasAdditionalInformation = true;
                        additionalInfoBtn.gameObject.SetActive(true);
                        additionalInfoBtn.onClick.AddListener(() => ShowAdditionalInformation(subQuestObject));
                        SetAdditionalInformation(subQuestData);
                    }
                    else
                        additionalInfoBtn.gameObject.SetActive(false);
                    
                    mainQuestObject.AddSubQuest(subQuestObject);
                    
                    if (subQuestData.Script != null)
                    {
                        var type = Type.GetType(subQuestData.Script) ?? Type.GetType($"{subQuestData.Script}, Assembly-CSharp");
                        subQuestBody.gameObject.AddComponent(type);
                    }
                    
                }
                mainQuestData.QuestBody = mainQuestObject;
            }
        }

        protected override void ShowAdditionalInformation(SubQuest subQuest)
        {
            var additionalInfoBody = subQuest.additionalInformationBody;
            additionalInfoBody.SetActive(!additionalInfoBody.activeInHierarchy);
        }
        
        protected override void SetAdditionalInformation(QuestData subQuestData)
        {
            var subQuest = (SubQuest)subQuestData.QuestBody;

            subQuest.additionalInformationBody.GetComponentInChildren<LocalizedTMP>().Key =
                subQuestData.AdditionalInfoTranslationKey;
            subQuest.additionalInformationBody.GetComponentInChildren<TMP_Text>().text =
                LanguageManager.Instance.GetString(subQuestData.AdditionalInfoTranslationKey);


            if (!File.Exists(subQuestData.AdditionalImagePath))
            {
                subQuestData.AdditionalImagePath = null;
                Destroy(subQuest.additionalInformationBody.GetComponentInChildren<RawImage>().gameObject);
                return;
            }

            if (subQuest.QuestData.AdditionalImageHeight == 0 || subQuest.QuestData.AdditionalImageWidth == 0)
            {
                var additionalImage =
                    new Texture2D(100,100);
                additionalImage.LoadImage(File.ReadAllBytes(subQuestData.AdditionalImagePath));
                subQuest.additionalInformationBody.GetComponentInChildren<RawImage>().texture = additionalImage;
                subQuest.additionalInformationBody.GetComponentInChildren<RawImage>().SetNativeSize();
            }
            else
            {
                var additionalImage = new Texture2D(subQuestData.AdditionalImageWidth, subQuestData.AdditionalImageHeight);
                additionalImage.LoadImage(File.ReadAllBytes(subQuestData.AdditionalImagePath));
                subQuest.additionalInformationBody.GetComponentInChildren<RawImage>().texture = additionalImage;
            }
        }


    }
}
