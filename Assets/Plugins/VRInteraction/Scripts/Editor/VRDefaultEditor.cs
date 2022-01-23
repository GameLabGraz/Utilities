using UnityEngine;
using UnityEditor;

namespace GEAR.VRInteraction.Editor
{
    public class VRDefaultEditor : UnityEditor.Editor
    {
        private const string TexturePathDarkTheme = "images/VRInteraction_logo_darkTheme";
        private const string TexturePathLightTheme = "images/VRInteraction_logo_lightTheme";
        private const float logoHeight = 48;
        private Texture2D _logoTexture;
        private void Awake()
        {
            _logoTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin? TexturePathDarkTheme : TexturePathLightTheme);
        }

        public override void OnInspectorGUI()
        {
            DrawLogoTexture();
            DrawDefaultInspector();
        }

        private void DrawLogoTexture()
        {
            if (_logoTexture)
            {
                GUILayout.Label(_logoTexture, GUILayout.Height(logoHeight), GUILayout.MinHeight(logoHeight), GUILayout.MaxHeight(logoHeight), GUILayout.ExpandHeight(false));
            }
        }
    }


    [CustomEditor(typeof(VRCircularDrive))]
    public class VRCircularDriveEditor : VRDefaultEditor
    {
    }
    
    [CustomEditor(typeof(VRLinearDrive))]
    public class VRLinearDriveEditor : VRDefaultEditor
    {
    }
    
    [CustomEditor(typeof(VRHoverButton))]
    public class VRHoverButtonEditor : VRDefaultEditor
    {
    }
    
    [CustomEditor(typeof(VRSnapDropZone))]
    public class VRSnapDropZoneEditor : VRDefaultEditor
    {
    }
    
    [CustomEditor(typeof(VRPlayer))]
    public class VRPlayerEditor : VRDefaultEditor
    {
    }
    
    [CustomEditor(typeof(VRHighlighter))]
    public class VRHighlighterEditor : VRDefaultEditor
    {
    }
    
    [CustomEditor(typeof(TextFormatterTMP))]
    public class TextFormatterTMPEditor : VRDefaultEditor
    {
    }
    
    [CustomEditor(typeof(VR3DDrive))]
    public class VR3DDriveEditor : VRDefaultEditor
    {
    }
    
        
    [CustomEditor(typeof(VRColorPicker))]
    public class VRColorPickerEditor : VRDefaultEditor
    {
    }
}