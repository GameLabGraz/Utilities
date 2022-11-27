using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameLabGraz.QuestManager
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private TextAsset QuestFile;

        public bool hideFinishedQuests = false;

        private int _currentQuestIndex = 0;
        private int _starScore = 0;
        private int _completedQuests = 0;
        
        private const string XmlSchemaFile = "qmSchema";
        private readonly XmlSchemaSet _xmlSchemaSet = new XmlSchemaSet();
        
        public readonly List<QuestData> MainQuests = new List<QuestData>();
        public readonly List<QuestData> SubQuests = new List<QuestData>();
        
        public static readonly UnityEvent OnQuestsRead = new UnityEvent();
        public static readonly UnityEvent OnStarEarned = new UnityEvent();
        
        private void Start()
        {
            _currentQuestIndex = 0; 
            _xmlSchemaSet.Add("", XmlReader.Create(
                new MemoryStream(Resources.Load<TextAsset>(XmlSchemaFile).bytes)));
            ReadQuestXml();
        }

        private void Update()
        {
            _completedQuests = SubQuests.Count(quest => quest.IsCompleted);
        }

        private void ReadQuestXml()
        {
            MainQuests.Clear();
            if (!QuestFile)
            {
                Debug.LogError($"QuestManager::ReadQuestXML: Unable to load quest file {QuestFile.name}");
                return;
            }

            var validationError = false;
            var doc = XDocument.Load(new MemoryStream(QuestFile.bytes));

            doc.Validate(_xmlSchemaSet, (o, e) =>  
            {  
                Debug.Log(e.Message); 
                validationError = true;
            });

            if (validationError)
            {
                Debug.LogError($"QuestManager::ReadQuestXML: Validation for file {QuestFile.name} failed");
                return;
            }

            foreach (var mainQuest in doc.Descendants("MainQuest"))
            {
                var mainQuestData = new QuestData()
                {
                    DefaultText = mainQuest.Element("DefaultText")?.Value ?? "",
                    TranslationKey = mainQuest.Element("TranslationKey")?.Value ?? "",
                };
                foreach (var subQuest in mainQuest.Elements().Where(element => element.Name == "SubQuest"))
                {
                    var subQuestDataObject = new QuestData
                    {
                        DefaultText = subQuest.Element("DefaultText")?.Value ?? "",
                        TranslationKey = subQuest.Element("TranslationKey")?.Value ?? "",
                        Script = subQuest.Element("Script")?.Value ?? "",
                        QuestHint = GameObject.Find(subQuest.Element("QuestHint")?.Value), 
                        QuestAchievement = GameObject.Find(subQuest.Element("QuestAchievement")?.Value),
                    };

                    var additionalInfoData = subQuest.Element("AdditionalInformation");

                    if (additionalInfoData != null)
                    {
                        subQuestDataObject.AdditionalInfoTranslationKey = 
                            additionalInfoData.Element("AdditionalTranslationKey")?.Value ?? "";
                        subQuestDataObject.AdditionalInformation = 
                            additionalInfoData.Element("DefaultText")?.Value ?? "";
                        var additionalImageData = additionalInfoData.Element("Image");
                        if (additionalImageData != null)
                        {
                            subQuestDataObject.AdditionalImagePath = 
                                additionalImageData.Element("ImagePath")?.Value ?? "";
                            subQuestDataObject.AdditionalImageWidth = 
                                int.Parse(additionalImageData.Element("Width")?.Value ?? "0");
                            subQuestDataObject.AdditionalImageHeight = 
                                int.Parse(additionalImageData.Element("Height")?.Value ?? "0");
                        }
                    }

                    subQuestDataObject.QuestHint?.SetActive(false);
                    subQuestDataObject.QuestAchievement?.SetActive(false);
                    
                    mainQuestData.SubQuestData.Add(subQuestDataObject);
                    SubQuests.Add(subQuestDataObject);
                }

                MainQuests.Add(mainQuestData);
            }
            Debug.Log($"QuestManager::ReadQuestXML: Reading successful");
            OnQuestsRead.Invoke();
        }

        public void ActivateNextMainQuest()
        {
            if (_currentQuestIndex >= MainQuests.Count)
                return;

            var questData = MainQuests[_currentQuestIndex++];
            var activeMainQuest = (MainQuest) questData.QuestBody;
            activeMainQuest.IsActive = true;
            activeMainQuest.ActivateNextSubQuest();
            activeMainQuest.onQuestFinished.AddListener(() =>
            {
                var quote = _completedQuests / MainQuests.Count;
                
                if ( (quote > 0.33 && _starScore == 0) || ( quote > 0.66 && _starScore == 1) || quote > 0.99 )
                {
                    _starScore++;
                    OnStarEarned.Invoke();
                }
                
                if (activeMainQuest.finishLine != null) 
                    activeMainQuest.finishLine.SetActive(true);
                else
                    activeMainQuest.gameObject.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;

                if (hideFinishedQuests)
                    activeMainQuest.HideSubQuests();
                
                ActivateNextMainQuest();
            });
        }

    }
}