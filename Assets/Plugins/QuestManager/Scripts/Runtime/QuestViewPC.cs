using System;
using System.Collections.Generic;
using System.IO;
using GEAR.Localization;
using GEAR.Localization.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace GameLabGraz.QuestManager
{ 
    public class QuestViewPC : QuestView
    {
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

                    var additionalInfoBtn = subQuestBody.GetComponentInChildren<UnityEngine.UI.Button>();
                    
                    if (subQuestData.AdditionalInformation != null || subQuestData.AdditionalImagePath != null)
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
            var additionalInfoBody = subQuest.AdditionalInformationBody;
            additionalInfoBody.SetActive(!additionalInfoBody.activeInHierarchy);
        }        

        protected override void SetAdditionalInformation(QuestData subQuestData)
        {
            var subQuest = (SubQuest)subQuestData.QuestBody;

           
            if (string.IsNullOrEmpty(subQuestData.AdditionalInformation))
            {
                subQuest.AdditionalInformationBody.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                subQuest.AdditionalInformationBody.GetComponentInChildren<LocalizedTMP>().Key =
                    subQuestData.AdditionalInfoTranslationKey;
                subQuest.AdditionalInformationBody.GetComponentInChildren<TMP_Text>().text =
                    LanguageManager.Instance.GetString(subQuestData.AdditionalInfoTranslationKey);
            }

            if (!File.Exists(subQuestData.AdditionalImagePath))
            {
                subQuestData.AdditionalImagePath = null;
                Destroy(subQuest.AdditionalInformationBody.GetComponentInChildren<RawImage>().gameObject);
                return;
            }

            if (subQuest.QuestData.AdditionalImageHeight <= 0 || subQuest.QuestData.AdditionalImageWidth <= 0 
                || subQuest.QuestData.AdditionalImageHeight > MaxImageSize.y || subQuest.QuestData.AdditionalImageWidth > MaxImageSize.x)
            {
                var additionalImage = new Texture2D(100,100);
                additionalImage.LoadImage(File.ReadAllBytes(subQuestData.AdditionalImagePath));
                subQuest.AdditionalInformationBody.GetComponentInChildren<RawImage>().texture = additionalImage;
                subQuest.AdditionalInformationBody.GetComponentInChildren<RawImage>().SetNativeSize();
            }
            else
            {
                var additionalImage = new Texture2D(subQuestData.AdditionalImageWidth, subQuestData.AdditionalImageHeight);
                additionalImage.LoadImage(File.ReadAllBytes(subQuestData.AdditionalImagePath));
                subQuest.AdditionalInformationBody.GetComponentInChildren<RawImage>().texture = additionalImage;
                subQuest.AdditionalInformationBody.GetComponentInChildren<RawImage>().GetComponent<RectTransform>()
                    .sizeDelta = new Vector2(subQuestData.AdditionalImageWidth, subQuestData.AdditionalImageHeight);
            }

        }


    }
}
