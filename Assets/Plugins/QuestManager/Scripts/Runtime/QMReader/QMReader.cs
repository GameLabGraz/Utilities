using System.Collections.Generic;
using GEAR.QuestManager.Data;
using UnityEngine;

namespace GEAR.QuestManager.Reader
{
    [ExecuteInEditMode]
    public abstract class QMReader : MonoBehaviour
    {
        public abstract List<MainQuestInfo> ReadData ();
    }
}