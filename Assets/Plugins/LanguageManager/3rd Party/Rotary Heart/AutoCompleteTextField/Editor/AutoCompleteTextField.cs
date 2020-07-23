using UnityEngine;

namespace RotaryHeart.Lib.AutoComplete
{
    public static class AutoCompleteTextField
    {
        /// <summary>
        /// Uses UnityEditor.EditorGUILayout to draw the text field
        /// </summary>
        public static class EditorGUILayout
        {
            #region Polymorphism
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string text, string[] entries, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField("", text, GUI.skin.textField, entries, "", options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string text, string[] entries, string hint, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField("", text, GUI.skin.textField, entries, hint, options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string text, GUIStyle style, string[] entries, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField("", text, style, entries, "", options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string text, GUIStyle style, string[] entries, string hint, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField("", text, style, entries, hint, options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string label, string text, string[] entries, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(label, text, GUI.skin.textField, entries, "", options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string label, string text, string[] entries, string hint, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(label, text, GUI.skin.textField, entries, hint, options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string label, string text, GUIStyle style, string[] entries, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(label, text, style, entries, "", options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string label, string text, GUIStyle style, string[] entries, string hint, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(new GUIContent(label), text, style, entries, hint, options);
            }

            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(GUIContent label, string text, string[] entries, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(label, text, GUI.skin.textField, entries, "", options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(GUIContent label, string text, string[] entries, string hint, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(label, text, GUI.skin.textField, entries, hint, options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(GUIContent label, string text, GUIStyle style, string[] entries, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(label, text, style, entries, "", options);
            }
            #endregion

            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(GUIContent label, string text, GUIStyle style, string[] entries, string hint, params GUILayoutOption[] options)
            {
                //Get the rect to draw the text field
                Rect lastRect = UnityEditor.EditorGUILayout.GetControlRect(!string.IsNullOrEmpty(label.text), UnityEditor.EditorGUIUtility.singleLineHeight, style, options);

                //Draw it without using layout
                return EditorGUI.AutoCompleteTextField(lastRect, label, text, style, entries, hint);
            }
        }

        /// <summary>
        /// Uses UnityEditor.EditorGUI to draw the text field
        /// </summary>
        public static class EditorGUI
        {
            #region Polymorphism
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string text, string[] entries)
            {
                return AutoCompleteTextField(position, "", text, GUI.skin.textField, entries, "");
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string text, string[] entries, string hint)
            {
                return AutoCompleteTextField(position, "", text, GUI.skin.textField, entries, hint);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string text, GUIStyle style, string[] entries)
            {
                return AutoCompleteTextField(position, "", text, style, entries, "");
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string text, GUIStyle style, string[] entries, string hint)
            {
                return AutoCompleteTextField(position, "", text, style, entries, hint);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string label, string text, string[] entries)
            {
                return AutoCompleteTextField(position, label, text, GUI.skin.textField, entries, "");
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string label, string text, string[] entries, string hint)
            {
                return AutoCompleteTextField(position, label, text, GUI.skin.textField, entries, hint);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string label, string text, GUIStyle style, string[] entries)
            {
                return AutoCompleteTextField(position, label, text, style, entries, "");
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string label, string text, GUIStyle style, string[] entries, string hint)
            {
                return AutoCompleteTextField(position, new GUIContent(label), text, style, entries, hint);
            }

            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, GUIContent label, string text, string[] entries)
            {
                return AutoCompleteTextField(position, label, text, GUI.skin.textField, entries, "");
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, GUIContent label, string text, string[] entries, string hint)
            {
                return AutoCompleteTextField(position, label, text, GUI.skin.textField, entries, hint);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, GUIContent label, string text, GUIStyle style, string[] entries)
            {
                return AutoCompleteTextField(position, label, text, style, entries, "");
            }
            #endregion

            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, GUIContent label, string text, GUIStyle style, string[] entries, string hint)
            {
                //Check for focus, the system is only shown if the text field is focused
                GUI.SetNextControlName("CheckFocus");
                string value = UnityEditor.EditorGUI.TextField(position, label, text, style);

                return _AutoCompleteLogic(position, label, value, entries, hint);
            }
        }

        //This is a hack to prevent saving the return value to the wrong UI element
        static Vector2 oldPosition;
        //Saves the returned value (if any was selected)
        static readonly GUIContent M_ReturnedContent = new GUIContent();
        static readonly GUIStyle M_MyStyle = new GUIStyle(GUIStyle.none);

        static bool returnedValue;

        /// <summary>
        /// Logic for the auto complete draw on text field focus
        /// </summary>
        /// <param name="position">TextField position</param>
        /// <param name="label">Label from the TextField</param>
        /// <param name="text">Value from the TextField</param>
        /// <param name="entries">Entries to display</param>
        /// <param name="hint">Hint information to show, if any</param>
        /// <returns>Selected value from autocomplete window</returns>
        static string _AutoCompleteLogic(Rect position, GUIContent label, string text, string[] entries, string hint)
        {
            //Used to draw the window
            Rect lastRect = position;

            if (returnedValue)
            {
                float offset = 0;

                if (!string.IsNullOrEmpty(label.text))
                {
                    offset = UnityEditor.EditorGUIUtility.labelWidth - ((UnityEditor.EditorGUI.indentLevel) * 15);
                }

                Vector2 pos = lastRect.position;

                pos.x += UnityEditor.EditorGUI.indentLevel * 15 + offset;

                //The system returned a value, need to check if this UI element is the one that called
                if (pos.Equals(oldPosition))
                {
                    returnedValue = false;
                    string val = M_ReturnedContent.text;
                    M_ReturnedContent.text = "";
                    return val;
                }
            }

            //Hack to avoid hint being drawn on the wrong location for attribute drawers
            M_MyStyle.normal.textColor = Color.clear;
            GUI.enabled = false;
            UnityEditor.EditorGUI.TextField(lastRect, " ", " ", M_MyStyle);
            GUI.enabled = true;

            //Display the hint
            if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(hint))
            {
                //Nothing is typed, show a hint to the user
                GUI.enabled = false;
                
                UnityEditor.EditorGUI.TextField(lastRect, label, " " + hint, GUI.skin.label);
                GUI.enabled = true;
            }

            //Only display the system if the text field is focused
            if (GUI.GetNameOfFocusedControl().Equals("CheckFocus"))
            {
                //New position for this window
                Rect newRect = lastRect;

                //Remove focus
                GUI.FocusControl(null);

                if (!string.IsNullOrEmpty(label.text))
                {
                    float offset = UnityEditor.EditorGUIUtility.labelWidth - ((UnityEditor.EditorGUI.indentLevel) * 15);
                    newRect.x += offset;
                    newRect.width -= offset;
                }

                newRect.x += UnityEditor.EditorGUI.indentLevel * 15;
                newRect.width -= UnityEditor.EditorGUI.indentLevel * 15;

                AddItemWindow.Show(newRect, entries, null, s =>
                {
                    M_ReturnedContent.text = s;
                    returnedValue = true;
                }, "/", returnFullPath: false);
                
                oldPosition = newRect.position;
            }

            return text;
        }
    }
}
