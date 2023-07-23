using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GEAR.Localization;
using GEAR.Localization.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace GameLabGraz.QuestManager.View
{
    public class QuestViewVR : QuestView
    {
        [SerializeField] protected GameObject Handle;
        [SerializeField] private Vector3 _mQBodyiniOffset = new Vector3(0, -0.1f, 0);
        [SerializeField] private Vector3 _mQBodyOffset = new Vector3(0, -2.85f, 0);
        [SerializeField] private Vector3 _sQBodyOffset = new Vector3(0f, -2f, -0.1f);


        protected override void InitializeQuestView(List<QuestData> mainQuests)
        {
            QuestManager questManager = FindObjectOfType<QuestManager>();
            if (questManager != null)
            {
                questManager.ScrollDownVR.AddListener(HandleScrollDownView);
            }
            var mainQuestPosition = _mQBodyiniOffset;
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
                    subQuestBody.transform.localPosition = subQuestOffsetCount * _sQBodyOffset;
                    subQuestBody.GetComponentInChildren<LocalizedTMP>().Key = subQuestData.TranslationKey;
                    subQuestBody.GetComponentInChildren<TMP_Text>().text = LanguageManager.Instance.GetString(subQuestData.TranslationKey);
                    
                    var subQuestObject = subQuestBody.GetComponent<SubQuest>();
                    subQuestData.QuestBody = subQuestObject;
                    subQuestObject.QuestData = subQuestData;

                    var additionalInfoBtn = subQuestObject.GetComponentInChildren<Button>();
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
                        subQuestObject.gameObject.AddComponent(type);
                    }
                    subQuestObject.QuestData = subQuestData;

                    subQuestOffsetCount++;
                }
                mainQuestData.QuestBody = mainQuestObject;
                mainQuestPosition += _mQBodyOffset + _sQBodyOffset * subQuestOffsetCount;
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

           
            var canvas = subQuest.AdditionalInformationBody.GetComponentInChildren<Canvas>();

            if (string.IsNullOrEmpty(subQuestData.AdditionalInformation))
            {
                subQuest.AdditionalInformationBody.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                subQuest.AdditionalInformationBody.gameObject.transform.GetChild(1).transform.localPosition += new Vector3(0,0,0.003f);
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
                canvas.transform.parent.gameObject.SetActive(false);
                return;
            }

            if (subQuest.QuestData.AdditionalImageHeight <= 0 || subQuest.QuestData.AdditionalImageWidth <= 0
                || subQuest.QuestData.AdditionalImageHeight > MaxImageSize.y || subQuest.QuestData.AdditionalImageWidth > MaxImageSize.x)
            {
                var additionalImage = new Texture2D(100, 100);
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

        private void HandleScrollDownView(GameObject target, float duration)
        {
            StartCoroutine(ScrollDownView(target, duration));
        }

        public IEnumerator ScrollDownView(GameObject quest, float distance)
        {
            float scrollDuration = 2f;
            float moveAmount = distance / scrollDuration * Time.deltaTime;
            var mainRenderer = quest.GetComponentInChildren<MeshRenderer>();
            var lastSQ = quest.GetComponentsInChildren<SubQuest>().LastOrDefault().gameObject;
            var renderer = lastSQ.GetComponentInChildren<MeshRenderer>();

            if (renderer == null)
                yield return null;

            while (mainRenderer.isVisible || renderer.isVisible)
            {
                Vector3 newPosition = Handle.transform.position - new Vector3(0, moveAmount, 0);
                Handle.transform.position = newPosition;

                yield return null;
            }
        }

    }
}

