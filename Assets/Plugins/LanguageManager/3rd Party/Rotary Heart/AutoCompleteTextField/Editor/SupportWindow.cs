using UnityEditor;

namespace RotaryHeart.Lib.AutoComplete
{
    public class SupportWindow : BaseSupportWindow
    {
        const string SUPPORT_FORUM = "https://forum.unity.com/threads/released-autocomplete-textfield.521902/";
        const string STORE_LINK = "https://assetstore.unity.com/packages/tools/gui/autocomplete-textfield-112403";
        const string ASSET_NAME = "AutoComplete TextField";
        const string VERSION = "1.5";

        protected override string SupportForum
        {
            get { return SUPPORT_FORUM; }
        }
        protected override string StoreLink
        {
            get { return STORE_LINK; }
        }
        protected override string AssetName
        {
            get { return ASSET_NAME; }
        }
        protected override string Version
        {
            get { return VERSION; }
        }

        [MenuItem("Tools/Rotary Heart/AutoComplete TextField/About")]
        public static void AboutClicked()
        {
            ShowWindow<SupportWindow>();
        }
    }
}