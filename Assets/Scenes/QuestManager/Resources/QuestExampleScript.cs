using UnityEngine;
using GameLabGraz.QuestManager;

namespace Quests
{
    [RequireComponent(typeof(Quest))]
    public class QuestExampleScript : QuestCheck
    {

        protected override void InitCheck()
        {
        }

        protected override bool CheckCompliance()
        {
            return false;
        }
    }
}
