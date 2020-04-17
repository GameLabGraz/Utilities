using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using GEAR.QuestManager.Data;

namespace GEAR.QuestManager.Reader
{
    public class QMXmlReader : QMReader
    {
        private TextAsset _xmlFile;

        public QMXmlReader(TextAsset xmlFile)
        {
            _xmlFile = xmlFile;
        }

        public List<MainQuestInfo> ReadData()
        {
            var mainQuestInfos = new List<MainQuestInfo>();

            if (!_xmlFile)
            {
                Debug.LogError($"QMXmlReader::ReadData: Unable to load xml file.");
                return new List<MainQuestInfo>();
            }

            //ToDo XML validation
            var doc = XDocument.Load(new MemoryStream(_xmlFile.bytes));
            foreach (var mainQuest in doc.Descendants("MainQuest"))
            {
                 var mainQuestInfo = new MainQuestInfo(
                    int.Parse(mainQuest.Element("Id").Value), 
                    mainQuest.Element("Name").Value);

                 foreach (var subQuest in mainQuest.Elements().Where(element => element.Name == "SubQuest"))
                 {
                     mainQuestInfo.AddSubQuestInfo(new SubQuestInfo(
                             int.Parse(mainQuest.Element("Id").Value),
                             subQuest.Element("Name").Value,
                         Type.GetType(subQuest.Element("Script").Value),
                         subQuest.Attribute("HasAdditionalInformation").Value == "True"));
                 }

                 mainQuestInfos.Add(mainQuestInfo);
            }

            return mainQuestInfos;
        }
    }
}