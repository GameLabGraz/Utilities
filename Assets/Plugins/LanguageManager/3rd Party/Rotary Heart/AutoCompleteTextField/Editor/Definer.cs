using System.Collections.Generic;
using UnityEditor;

namespace RotaryHeart.Lib.AutoComplete
{
    [InitializeOnLoad]
    public class Definer
    {
        static Definer()
        {
            List<string> defines = new List<string>(1)
            {
                "RH_AutoComplete"
            };

            RotaryHeart.Lib.Definer.ApplyDefines(defines);
        }
    }
}