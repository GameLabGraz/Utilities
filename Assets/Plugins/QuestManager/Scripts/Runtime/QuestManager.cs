using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameLabGraz.QuestManager
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private TextAsset _questFile;
        [SerializeField] private bool _hideCompletedQuests = false;

        private int _currentQuestIndex = 0;
        private int _completedQuests = 0;
        
        private const string _xmlSchemaFile = "qmSchema";
        private readonly XmlSchemaSet _xmlSchemaSet = new XmlSchemaSet();


        public readonly List<QuestData> MainQuests = new List<QuestData>();
        public readonly List<QuestData> SubQuests = new List<QuestData>();
        
        public static readonly UnityEvent OnQuestsRead = new UnityEvent();
        public UnityEvent OnQuestCompleted = new UnityEvent();
        public UnityEvent<GameObject, float> ScrollDownVR;


        private void Start()
        {
            _currentQuestIndex = 0; 
            _xmlSchemaSet.Add("", XmlReader.Create(
                new MemoryStream(Resources.Load<TextAsset>(_xmlSchemaFile).bytes)));
            ReadQuestXml();
        }

        private void Update()
        {
            if (SubQuests.Count(quest => quest.IsCompleted) != _completedQuests)
            {
                _completedQuests++;
                OnQuestCompleted.Invoke();
            }
        }

        private void ReadQuestXml()
        {
            MainQuests.Clear();
            if (!_questFile)
            {
                Debug.LogError($"QuestManager::ReadQuestXML: Unable to load quest file {_questFile.name}");
                return;
            }

            var validationError = false;
            var doc = XDocument.Load(new MemoryStream(_questFile.bytes));

            doc.Validate(_xmlSchemaSet, (o, e) =>  
            {  
                Debug.Log(e.Message); 
                validationError = true;
            });

            if (validationError)
            {
                Debug.LogError($"QuestManager::ReadQuestXML: Validation for file {_questFile.name} failed");
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
                if (activeMainQuest.FinishLine != null)
                {
                    activeMainQuest.FinishLine.SetActive(true);
                    var questCount = activeMainQuest.GetSubQuestCount();
                    var questManager = FindObjectOfType<QuestManager>();
                    ScrollDownVR?.Invoke(activeMainQuest.gameObject, 0.25f * questCount);
                }
                else
                {
                    activeMainQuest.gameObject.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    StartCoroutine(activeMainQuest.MoveMainQuestToBottom());

                    if (_hideCompletedQuests)
                        StartCoroutine(activeMainQuest.HideSubQuests());
                }

                ActivateNextMainQuest();
            });
        }

    }
}