using UnityEngine;

namespace GEAR.QuestManager.Reader
{
    [ExecuteInEditMode]
    public abstract class QMReader : MonoBehaviour
    {
        public GameObject Root { get; set; }

        public Vector3 CoverSize { get; set; }
        public GameObject MainQuestPrefab { get; set; }
        public Vector3 MainQuestIniOffset { get; set; }
        public Vector3 MainQuestBodyOffset { get; set; }

        public GameObject SubQuestPrefab { get; set; }
        public Vector3 SubQuestBodyOffset { get; set; }
        public bool autoPositionQuests { get; set; }

        public abstract void ReadData ();

        
    }
}