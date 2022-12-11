using System;
using System.Collections.Generic;
using GEAR.Localization;
using GEAR.Localization.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace GameLabGraz.QuestManager
{
    public class QuestViewVR : QuestView
    {
        [SerializeField] private Vector3 MQBodyiniOffset = new Vector3(0, -5, 0);
        [SerializeField] private Vector3 MQBodyOffset = new Vector3(0, -2.85f, 0);
        [SerializeField] private Vector3 SQBodyOffset = new Vector3(0f, -2f, -0.1f);
        [SerializeField] private GameObject Cover;

        protected override void InitializeQuestView(List<QuestData> mainQuests)
        {
            var mainQuestPosition = MQBodyiniOffset;
            foreach (var mainQuestData in mainQuests)
            {
                var mainQuestBody = Instantiate(MainQuestBody, DataObjectRoot.transform);
                mainQuestBody.name = mainQuestData.DefaultText;
                mainQuestBody.transform.localPosition = mainQuestPosition;
                mainQuestBody.GetComponentInChildren<LocalizedTMP>().Key = mainQuestData.TranslationKey;
                mainQuestBody.GetComponentInChildren<TMP_Text>().text = LanguageManager.Instance.GetString(mainQuestData.TranslationKey);

                var mainQuestObject = mainQuestBody.GetComponent<MainQuest>();
                mainQuestObject.QuestData = mainQuestData;

                var subQuestOffsetCount = 0;
                foreach (var subQuestData in mainQuestData.SubQuestData)
                {
                    var subQuestBody = Instantiate(SubQuestBody, mainQuestBody.transform);
                    subQuestBody.name = subQuestData.DefaultText;
                    subQuestBody.transform.localScale = Vector3.one;
                    subQuestBody.transform.localPosition = subQuestOffsetCount * SQBodyOffset;
                    subQuestBody.GetComponentInChildren<LocalizedTMP>().Key = subQuestData.TranslationKey;
                    subQuestBody.GetComponentInChildren<TMP_Text>().text = LanguageManager.Instance.GetString(subQuestData.TranslationKey);
                    
                    var subQuestObject = subQuestBody.GetComponent<SubQuest>();
                    subQuestData.QuestBody = subQuestObject;

                    var additionalInfoBtn = subQuestObject.GetComponentInChildren<Button>();
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
                        subQuestObject.gameObject.AddComponent(type);
                    }
                    subQuestObject.QuestData = subQuestData;

                    subQuestOffsetCount++;
                }
                mainQuestData.QuestBody = mainQuestObject;
                mainQuestPosition += MQBodyOffset + SQBodyOffset * subQuestOffsetCount;
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

            var canvas = subQuest.additionalInformationBody.GetComponentInChildren<Canvas>();

            if (!File.Exists(subQuestData.AdditionalImagePath))
            {
                canvas.transform.parent.gameObject.SetActive(false);
                return;
            }

            var additionalImage = new Texture2D(100, 100);
            additionalImage.LoadImage(File.ReadAllBytes(subQuestData.AdditionalImagePath));
            subQuest.additionalInformationBody.GetComponentInChildren<RawImage>().texture = additionalImage;
            // if (additionalImage.width == 0 || additionalImage.height == 0)
            //     subQuest.additionalInformationBody.GetComponentInChildren<RawImage>().SetNativeSize();
            // else
            //     additionalImage.Resize(subQuestData.AdditionalImageWidth, subQuestData.AdditionalImageHeight);

        }
    }
}

