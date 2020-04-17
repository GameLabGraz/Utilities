using System.Collections.Generic;
using GEAR.QuestManager.Data;

namespace GEAR.QuestManager.Reader
{
    public interface QMReader
    {
        List<MainQuestInfo> ReadData ();
    }
}