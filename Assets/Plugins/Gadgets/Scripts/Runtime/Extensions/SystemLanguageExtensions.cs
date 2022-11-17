using UnityEngine;

namespace GEAR.Gadgets.Extensions
{
    public static class SystemLanguageExtensions
    {
        public static string ToLanguageCode(this SystemLanguage language)
        {
            switch (language)
            {
                case SystemLanguage.English:
                    return "en";
                case SystemLanguage.German:
                    return "de";
                default:
                    return "unknown";
            }
        }
    }
}
