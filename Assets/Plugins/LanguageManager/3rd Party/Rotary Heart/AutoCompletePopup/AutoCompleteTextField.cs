using UnityEngine;

namespace RotaryHeart.Lib.AutoComplete
{
    public static class AutoCompleteTextField
    {
#if UNITY_EDITOR
        
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
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string text, string[] entries, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField("", text, UnityEngine.GUI.skin.textField, entries, "", allowCustom, allowEmpty, separator, returnFullPath, options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string text, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField("", text, UnityEngine.GUI.skin.textField, entries, hint, allowCustom, allowEmpty, separator, returnFullPath, options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string text, GUIStyle style, string[] entries, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField("", text, style, entries, "", allowCustom, allowEmpty, separator, returnFullPath, options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string text, GUIStyle style, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField("", text, style, entries, hint, allowCustom, allowEmpty, separator, returnFullPath, options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string label, string text, string[] entries, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(label, text, UnityEngine.GUI.skin.textField, entries, "", allowCustom, allowEmpty, separator, returnFullPath, options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string label, string text, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(label, text, UnityEngine.GUI.skin.textField, entries, hint, allowCustom, allowEmpty, separator, returnFullPath, options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string label, string text, GUIStyle style, string[] entries, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(label, text, style, entries, "", allowCustom, allowEmpty, separator, returnFullPath, options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(string label, string text, GUIStyle style, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(new GUIContent(label), text, style, entries, hint, allowCustom, allowEmpty, separator, returnFullPath, options);
            }

            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(GUIContent label, string text, string[] entries, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(label, text, UnityEngine.GUI.skin.textField, entries, "", allowCustom, allowEmpty, separator, returnFullPath, options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(GUIContent label, string text, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(label, text, UnityEngine.GUI.skin.textField, entries, hint, allowCustom, allowEmpty, separator, returnFullPath, options);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(GUIContent label, string text, GUIStyle style, string[] entries, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false, params GUILayoutOption[] options)
            {
                return AutoCompleteTextField(label, text, style, entries, "", allowCustom, allowEmpty, separator, returnFullPath, options);
            }
            
            #endregion Polymorphism

            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <param name="options">
            /// An optional list of layout options that specify extra layouting properties.<para>&#160;</para>
            /// Any values passed in here will override settings defined by the style.<para>&#160;</para>
            /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
            /// </param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(GUIContent label, string text, GUIStyle style, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false, params GUILayoutOption[] options)
            {
                //Get the rect to draw the text field
                Rect lastRect = UnityEditor.EditorGUILayout.GetControlRect(!string.IsNullOrEmpty(label.text), UnityEditor.EditorGUIUtility.singleLineHeight, style, options);

                //Draw it without using layout
                return EditorGUI.AutoCompleteTextField(lastRect, label, text, style, entries, hint, allowCustom, allowEmpty, separator, returnFullPath);
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
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string text, string[] entries, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, "", text, UnityEngine.GUI.skin.textField, entries, "", allowCustom, allowEmpty, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string text, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, "", text, UnityEngine.GUI.skin.textField, entries, hint, allowCustom, allowEmpty, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string text, GUIStyle style, string[] entries, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, "", text, style, entries, "", allowCustom, allowEmpty, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string text, GUIStyle style, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, "", text, style, entries, hint, allowCustom, allowEmpty, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string label, string text, string[] entries, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, label, text, UnityEngine.GUI.skin.textField, entries, "", allowCustom, allowEmpty, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string label, string text, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, label, text, UnityEngine.GUI.skin.textField, entries, hint, allowCustom, allowEmpty, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string label, string text, GUIStyle style, string[] entries, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, label, text, style, entries, "", allowCustom, allowEmpty, separator, returnFullPath);
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
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string label, string text, GUIStyle style, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, new GUIContent(label), text, style, entries, hint, allowCustom, allowEmpty, separator, returnFullPath);
            }

            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, GUIContent label, string text, string[] entries, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, label, text, UnityEngine.GUI.skin.textField, entries, "", allowCustom, allowEmpty, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, GUIContent label, string text, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, label, text, UnityEngine.GUI.skin.textField, entries, hint, allowCustom, allowEmpty, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, GUIContent label, string text, GUIStyle style, string[] entries, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, label, text, style, entries, "", allowCustom, allowEmpty, separator, returnFullPath);
            }
            
            #endregion Polymorphism

            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="separator">The separator to use for grouping selections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, GUIContent label, string text, GUIStyle style, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, string separator = "/", bool returnFullPath = false)
            {
                //Check for focus, the system is only shown if the text field is focused
                UnityEngine.GUI.SetNextControlName("CheckFocus");
                string value = UnityEditor.EditorGUI.TextField(position, label, text, style);
                
                //Hack to avoid hint being drawn on the wrong location for attribute drawers
                AutoCompleteBase.M_MyStyle.normal.textColor = Color.clear;
                UnityEngine.GUI.enabled = false;
                UnityEditor.EditorGUI.TextField(position, " ", " ", AutoCompleteBase.M_MyStyle);
                UnityEngine.GUI.enabled = true;

                //Display the hint
                if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(hint))
                {
                    //Nothing is typed, show a hint to the user
                    UnityEngine.GUI.enabled = false;
                
                    UnityEditor.EditorGUI.TextField(position, label, " " + hint, UnityEngine.GUI.skin.label);
                    UnityEngine.GUI.enabled = true;
                }

                string result = AutoCompleteBase._AutoCompleteLogic(position, label, value, entries, allowCustom, allowEmpty, true, null, separator, returnFullPath);
                UnityEngine.GUI.changed = result != text;
                return result;
            }
        }

#endif

        public static class GUI
        {
            #region Polymorphism
            
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="windowStyle">Contains the style to use for the window (colors, textures, etc)</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string text, string[] entries, bool allowCustom = false, bool allowEmpty = true, IStyle windowStyle = null, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, "", text, UnityEngine.GUI.skin.textField, entries, "", allowCustom, allowEmpty, windowStyle, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="windowStyle">Contains the style to use for the window (colors, textures, etc)</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string text, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, IStyle windowStyle = null, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, "", text, UnityEngine.GUI.skin.textField, entries, hint, allowCustom, allowEmpty, windowStyle, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="windowStyle">Contains the style to use for the window (colors, textures, etc)</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string text, GUIStyle style, string[] entries, bool allowCustom = false, bool allowEmpty = true, IStyle windowStyle = null, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, "", text, style, entries, "", allowCustom, allowEmpty, windowStyle, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="style">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="windowStyle">Contains the style to use for the window (colors, textures, etc)</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string text, GUIStyle style, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, IStyle windowStyle = null, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, "", text, style, entries, hint, allowCustom, allowEmpty, windowStyle, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="windowStyle">Contains the style to use for the window (colors, textures, etc)</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string label, string text, string[] entries, bool allowCustom = false, bool allowEmpty = true, IStyle windowStyle = null, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, label, text, UnityEngine.GUI.skin.textField, entries, "", allowCustom, allowEmpty, windowStyle, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="windowStyle">Contains the style to use for the window (colors, textures, etc)</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string label, string text, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, IStyle windowStyle = null, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, label, text, UnityEngine.GUI.skin.textField, entries, hint, allowCustom, allowEmpty, windowStyle, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="inputStyle">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="windowStyle">Contains the style to use for the window (colors, textures, etc)</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string label, string text, GUIStyle inputStyle, string[] entries, bool allowCustom = false, bool allowEmpty = true, IStyle windowStyle = null, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, label, text, inputStyle, entries, "", allowCustom, allowEmpty, windowStyle, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="inputStyle">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="windowStyle">Contains the style to use for the window (colors, textures, etc)</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, string label, string text, GUIStyle inputStyle, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, IStyle windowStyle = null, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, new GUIContent(label), text, inputStyle, entries, hint, allowCustom, allowEmpty, windowStyle, separator, returnFullPath);
            }

            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="windowStyle">Contains the style to use for the window (colors, textures, etc)</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, GUIContent label, string text, string[] entries, bool allowCustom = false, bool allowEmpty = true, IStyle windowStyle = null, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, label, text, UnityEngine.GUI.skin.textField, entries, "", allowCustom, allowEmpty, windowStyle, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="windowStyle">Contains the style to use for the window (colors, textures, etc)</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, GUIContent label, string text, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, IStyle windowStyle = null, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, label, text, UnityEngine.GUI.skin.textField, entries, hint, allowCustom, allowEmpty, windowStyle, separator, returnFullPath);
            }
            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="inputStyle">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="windowStyle">Contains the style to use for the window (colors, textures, etc)</param>
            /// <param name="separator">The separator to use for grouping sections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, GUIContent label, string text, GUIStyle inputStyle, string[] entries, bool allowCustom = false, bool allowEmpty = true, IStyle windowStyle = null, string separator = "/", bool returnFullPath = false)
            {
                return AutoCompleteTextField(position, label, text, inputStyle, entries, "", allowCustom, allowEmpty, windowStyle, separator, returnFullPath);
            }
            
            #endregion Polymorphism

            /// <summary>
            /// Make a TextField that has an <paramref name="AutoCompleteWindow"/> logic for selecting options.
            /// </summary>
            /// <param name="position">Rectangle on the screen to use for the text field</param>
            /// <param name="label">Optional label to display in front of the text field</param>
            /// <param name="text">The text to edit</param>
            /// <param name="inputStyle">Optional GUIStyle</param>
            /// <param name="entries">Entries to display</param>
            /// <param name="hint">Hint information to show, if any</param>
            /// <param name="allowCustom">Should the system allow custom entries</param>
            /// <param name="allowEmpty">Should the system add a Nothing element and allow returning an empty string</param>
            /// <param name="windowStyle">Contains the style to use for the window (colors, textures, etc)</param>
            /// <param name="separator">The separator to use for grouping selections</param>
            /// <param name="returnFullPath">Should the full path be returned when an option is selected</param>
            /// <returns>Selected value from autocomplete window</returns>
            public static string AutoCompleteTextField(Rect position, GUIContent label, string text, GUIStyle inputStyle, string[] entries, string hint, bool allowCustom = false, bool allowEmpty = true, IStyle windowStyle = null, string separator = "/", bool returnFullPath = false)
            {
                //Check for focus, the system is only shown if the text field is focused
                UnityEngine.GUI.SetNextControlName("CheckFocus");
                Rect labelPos = new Rect(position.position, inputStyle.CalcSize(label));
                UnityEngine.GUI.Label(labelPos, label, inputStyle);

                position.x = labelPos.xMax;
                position.width -= labelPos.width;
                
                string value = UnityEngine.GUI.TextField(position, text, inputStyle);
                
                AutoCompleteBase.M_MyStyle.normal.textColor = Color.clear;
                //Display the hint
                if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(hint))
                {
                    //Nothing is typed, show a hint to the user
                    UnityEngine.GUI.enabled = false;
                
                    UnityEngine.GUI.Label(position, " " + hint, UnityEngine.GUI.skin.label);
                    UnityEngine.GUI.enabled = true;
                }

                position.y = position.yMax;
                
                string result = AutoCompleteBase._AutoCompleteLogic(position, label, value, entries, allowCustom, allowEmpty, false, windowStyle, separator, returnFullPath);
                UnityEngine.GUI.changed = result != text;
                return result;
            }
        }
    }
}