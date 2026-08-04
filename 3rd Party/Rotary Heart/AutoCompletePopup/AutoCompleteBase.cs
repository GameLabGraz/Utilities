using UnityEngine;

namespace RotaryHeart.Lib.AutoComplete
{
    public interface IStyle
    {
        GUIStyle Header { get; }
        GUIStyle HeaderText { get; }
        GUIStyle ComponentButton { get; }
        Color BackgroundColor { get; }
        Color SelectionColor { get; }
        Color CurrentElementColor { get; }
        Color OddElementColor { get; }
        Color EvenElementColor { get; }
        Texture2D Background { get; }
        Texture2D WhiteTexture { get; }
        GUIStyle SearchBarStyle { get; }
        GUIStyle ClearSearchButtonStyle { get; }
    }

    internal class AutoCompleteBase
    {
        //This is a hack to prevent saving the return value to the wrong UI element
        internal static Vector2 M_returnedScreenPos;
        //Saves the returned value (if any was selected)
        internal static readonly GUIContent M_ReturnedContent = new GUIContent();
        internal static readonly GUIStyle M_MyStyle = new GUIStyle(GUIStyle.none);
        internal static readonly GUIStyle M_dropdownStyle = new GUIStyle("DropDownButton");

        static bool M_returnedValue;

        static AddItemWindow M_addItemWindow;

        /// <summary>
        /// Logic for the auto complete draw on text field focus
        /// </summary>
        /// <param name="position">TextField position</param>
        /// <param name="label">Label from the TextField</param>
        /// <param name="text">Value from the TextField</param>
        /// <param name="entries">Entries to display</param>
        /// <param name="allowCustom">Should the system allow custom entries</param>
        /// <param name="allowEmpty">Should the system allow to select empty objects</param>
        /// <param name="fromEditor">Is this being called from editor inspector</param>
        /// <param name="windowStyle">The style to use for the window</param>
        /// <param name="separator">The separator to use for grouping sections</param>
        /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
        /// <returns>Selected value from autocomplete window</returns>
        internal static string _AutoCompleteLogic(Rect position, GUIContent label, string text, string[] entries, bool allowCustom, bool allowEmpty, bool fromEditor,
            IStyle windowStyle, string separator, bool returnFullPath)
        {
            //Used to draw the window
            Rect lastRect = position;
            Vector2 myScreenPos = GUIUtility.GUIToScreenPoint(new Vector2(position.x, position.y));

            //The system returned a value, need to check if this UI element is the one that called
            if (M_returnedValue && M_returnedScreenPos == myScreenPos)
            {
                M_returnedValue = false;
                string val = M_ReturnedContent.text;
                M_ReturnedContent.text = "";
                M_returnedScreenPos = Vector2.zero;
                return val;
            }

            //Only display the system if the text field is focused
            if (GUI.GetNameOfFocusedControl().Equals("CheckFocus"))
            {
                //New position for this window
                Rect newRect = lastRect;

                //Remove focus
                GUI.FocusControl(null);

                if (fromEditor)
                {
#if UNITY_EDITOR
                    if (!string.IsNullOrEmpty(label.text))
                    {
                        float offset = UnityEditor.EditorGUIUtility.labelWidth - ((UnityEditor.EditorGUI.indentLevel) * 15);
                        newRect.x += offset;
                        newRect.width -= offset;
                    }

                    newRect.x += UnityEditor.EditorGUI.indentLevel * 15;
                    newRect.width -= UnityEditor.EditorGUI.indentLevel * 15;

                    EditorAddItemWindow.Show(newRect, entries, new []{ text }, s =>
                    {
                        M_ReturnedContent.text = s;
                        M_returnedValue = true;
                        M_returnedScreenPos = myScreenPos;
                    }, separator, returnFullPath: returnFullPath, allowCustom: allowCustom, allowEmpty: allowEmpty);
#endif
                }
                else
                {
                    M_addItemWindow = new AddItemWindow();
                    M_addItemWindow.Show(newRect, entries, new []{ text }, s =>
                    {
                        M_ReturnedContent.text = s;
                        M_returnedValue = true;
                        M_returnedScreenPos = myScreenPos;
                    }, separator, returnFullPath: returnFullPath, allowCustom: allowCustom, style: windowStyle, allowEmpty: allowEmpty);
                }
            }

            if (M_addItemWindow != null && new Rect(lastRect.position, new Vector2(lastRect.width, 320)) == M_addItemWindow.Position)
            {
                if (M_addItemWindow.Closed)
                {
                    M_addItemWindow = null;
                }
                else
                {
                    M_addItemWindow.OnGUI();
                }
            }

            return text;
        }

        internal static void _AutoCompleteLogic(Rect position, GUIContent label, string text, string[] entries, bool allowCustom, bool allowEmpty, bool fromEditor,
            IStyle windowStyle, System.Action<string> onItemAdded, string separator, bool returnFullPath)
        {
            Rect lastRect = position;

            if (GUI.GetNameOfFocusedControl().Equals("CheckFocus"))
            {
                //New position for this window
                Rect newRect = lastRect;

                //Remove focus
                GUI.FocusControl(null);
                if (fromEditor)
                {
#if UNITY_EDITOR
                    if (!string.IsNullOrEmpty(label.text))
                    {
                        float offset = UnityEditor.EditorGUIUtility.labelWidth - ((UnityEditor.EditorGUI.indentLevel) * 15);
                        newRect.x += offset;
                        newRect.width -= offset;
                    }

                    newRect.x += UnityEditor.EditorGUI.indentLevel * 15;
                    newRect.width -= UnityEditor.EditorGUI.indentLevel * 15;

                    EditorAddItemWindow.Show(newRect, entries, new[] { text }, onItemAdded, separator, returnFullPath: returnFullPath, allowCustom: allowCustom,
                        allowEmpty: allowEmpty);
#endif
                }
                else
                {
                    M_addItemWindow = new AddItemWindow();
                    M_addItemWindow.Show(newRect, entries, new[] { text }, onItemAdded, separator, returnFullPath: returnFullPath, allowCustom: allowCustom,
                        style: windowStyle, allowEmpty: allowEmpty);
                }
            }

            if (M_addItemWindow != null && new Rect(lastRect.position, new Vector2(lastRect.width, 320)) == M_addItemWindow.Position)
            {
                if (M_addItemWindow.Closed)
                {
                    M_addItemWindow = null;
                }
                else
                {
                    M_addItemWindow.OnGUI();
                }
            }
        }
    }
}