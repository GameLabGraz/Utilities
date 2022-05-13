using UnityEngine;

namespace RotaryHeart.Lib.AutoComplete
{
    public class AutoCompleteAttribute : PropertyAttribute
    {
        string[] m_entries;

        public string[] Entries
        {
            get { return m_entries; }
        }

        public AutoCompleteAttribute(string[] entries)
        {
            m_entries = entries;
        }
    }
}