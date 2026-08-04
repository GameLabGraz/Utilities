using UnityEngine;

namespace RotaryHeart.Lib.AutoComplete
{
    public class AutoCompleteAttribute : PropertyAttribute
    {
        public string[] Entries { get; }
        
        public AutoCompleteAttribute(string[] entries)
        {
            Entries = entries;
        }
    }
    
    public class AutoCompleteTextFieldAttribute : AutoCompleteAttribute
    {
        public AutoCompleteTextFieldAttribute(string[] entries) : base(entries) { }
    }
    
    public class AutoCompleteDropDownAttribute : AutoCompleteAttribute
    {
        public AutoCompleteDropDownAttribute(string[] entries) : base(entries) { }
    }
}