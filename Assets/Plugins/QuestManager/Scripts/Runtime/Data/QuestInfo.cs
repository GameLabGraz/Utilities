using System;

namespace GEAR.QuestManager.Data
{
    [Serializable]
    public abstract class QuestInfo
    {
        public int Id { get; }

        public string Name { get; }

        protected QuestInfo(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
