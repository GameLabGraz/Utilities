using UnityEditor;
using UnityEngine;

namespace GameLabGraz.LimeSurvey.Editor
{
    [CustomEditor(typeof(LimeSurveyManager))]
    public class LimeSurveyManagerEditor : UnityEditor.Editor
    {
        // ------------
        // Members

        // Image
        private const string TexturePathDarkTheme = "images/LIME_logo_darkTheme";
        private const string TexturePathLightTheme = "images/LIME_logo_lightTheme";
        private const float logoHeight = 48;
        private Texture2D _logoTexture;

        // Server Config
        private bool configActive = false;
        private bool configSaved = false;
        private string url = "url/index.php/admin/remotecontrol";
        private string username = "username";
        private string password = "password";

        // Survey
        private SerializedProperty surveyId;

        // ------------
        // Methods

        private void OnEnable()
        {
            _logoTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? TexturePathDarkTheme : TexturePathLightTheme);
            surveyId = serializedObject.FindProperty("surveyId");
        }

        public override void OnInspectorGUI()
        {
            DrawLogoTexture();
            serializedObject.Update();

            // Server Configuration
            EditorGUILayout.LabelField("Server Configuration", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Configuration is stored here:");
            EditorGUILayout.LabelField("LimeSurveyServerConfig.json");
            EditorGUILayout.Space();

            if (!configActive)
            {
                if(GUILayout.Button("Generate Config File"))
                {
                    configSaved = false;
                    configActive = true;
                }

                if(configSaved)
                {
                    EditorGUILayout.LabelField("Saved config file!");
                }
            }

            if(configActive)
            {
                EditorGUILayout.LabelField("URL");
                url = EditorGUILayout.TextField(url);

                EditorGUILayout.LabelField("Username");
                username = EditorGUILayout.TextField(username);

                EditorGUILayout.LabelField("Password");
                password = EditorGUILayout.PasswordField(password);

                if(GUILayout.Button("Save Config File"))
                {
                    SaveConfigFile();
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // Survey Configuration
            EditorGUILayout.LabelField("Survey Config", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(surveyId);
            EditorGUILayout.Space();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawLogoTexture()
        {
            if (_logoTexture)
            {
                GUILayout.Label(_logoTexture, GUILayout.Height(logoHeight), GUILayout.MinHeight(logoHeight), GUILayout.MaxHeight(logoHeight), GUILayout.ExpandHeight(false));
            }
        }

        private void SaveConfigFile()
        {
            // Create directory
            string dirPath = Application.dataPath + "/Resources/LimeSurveyServerConfig/";
            System.IO.Directory.CreateDirectory(dirPath);

            // Write .gitignore
            string gitPath = dirPath + ".gitignore";
            string[] gitLines =
            {
                "/LimeSurveyServerConfig.json",
                "/LimeSurveyServerConfig.json.meta"
            };

            System.IO.File.WriteAllLines(gitPath, gitLines);

            // Write Config file
            string configPath = dirPath + "LimeSurveyServerConfig.json";
            string[] configLines =
            {
                "{",
                "    \"url\": \"" + url + "\",",
                "    \"username\": \"" + username + "\",",
                "    \"password\": \"" + password + "\"",
                "}"
            };

            System.IO.File.WriteAllLines(configPath, configLines);
            
            // Update state
            configActive = false;
            configSaved = true;
        }
        
    }
}