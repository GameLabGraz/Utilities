using UnityEngine;

namespace GEAR.QuestManager.Reader
{
    public class QMXmlReader : QMReader
    {
        private TextAsset _xmlFile;

        public QMXmlReader (TextAsset xmlFile)
        {
            _xmlFile = xmlFile;
        }

        public override void ReadData ()
        {
            throw new System.NotImplementedException();
        }
    }
}