using System.Collections.Generic;
using UnityEditor;

namespace RotaryHeart.Lib.AutoComplete
{
    [InitializeOnLoad]
    public class AutoCompleteDefiner : Definer
    {
        static AutoCompleteDefiner()
        {
            List<string> defines = new List<string>(1)
            {
                "RH_AutoComplete"
            };

            ApplyDefines(defines);
        }
    }
}