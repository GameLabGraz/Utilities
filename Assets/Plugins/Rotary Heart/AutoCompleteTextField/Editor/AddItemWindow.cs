using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RotaryHeart.Lib.AutoComplete
{
    public class AddItemWindow : EditorWindow
    {
        class SectionData
        {
            public string data = string.Empty;
            public bool isSection = false;
            
            public bool Equals(SectionData other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return other.data == data;
            }
 
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != typeof(SectionData)) return false;
                return Equals((SectionData)obj);
            }
 
            public override int GetHashCode()
            {
                return data.GetHashCode();
            }
        }
        
        static class Styles
        {
            public static readonly GUIStyle Header = new GUIStyle("In BigTitle");
            public static readonly GUIStyle HeaderText = new GUIStyle();
            public static readonly GUIStyle ComponentButton = new GUIStyle("PR Label");
            public static readonly GUIStyle Background = "grey_border";
            public static readonly GUIStyle RightArrow = "AC RightArrow";
            public static readonly GUIStyle LeftArrow = "AC LeftArrow";
            public static readonly Color SelectionColor;
            public static readonly Color OddElementColor;
            public static readonly Color EvenElementColor;

            static Styles()
            {
                HeaderText.font = EditorStyles.boldFont;
                HeaderText.alignment = TextAnchor.MiddleCenter;
                
                ComponentButton.alignment = TextAnchor.MiddleLeft;
                ComponentButton.padding.left -= 15;
                ComponentButton.fixedHeight = 20f;
                ComponentButton.hover.textColor = Color.white;
                ComponentButton.active.background = null;

                SelectionColor = new Color(0.24f, 0.49f, 0.91f);
                OddElementColor = new Color(0.76f, 0.76f, 0.76f);
                EvenElementColor = new Color(0.72f, 0.72f, 0.72f);
            }
        }
        
        static AddItemWindow M_instance;

        readonly List<SectionData> m_sectionData = new List<SectionData>();
        
        Action<string> m_onItemAdded;
        string[] m_availableItems;
        string[] m_ignoreItems;
        string[] m_usedItems; 

        Vector2 m_scrollPosition;
        string m_separation = string.Empty;
        string m_searchString = string.Empty;
        string m_insideSection = string.Empty;
        string m_backText = string.Empty;
        bool m_useFullPath = true;
        int m_selectionIndex;
        Vector2? m_prevMousePos;

        static AddItemWindow Instance
        {
            get
            {
                if (M_instance == null)
                {
                    M_instance = ScriptableObject.CreateInstance<AddItemWindow>();
                }

                return M_instance;
            }
        }

        /// <summary>
        /// Shows the AddItemWindow dropdown
        /// </summary>
        /// <param name="position">Window position, ignoring height</param>
        /// <param name="items">Items to show</param>
        /// <param name="usedItems">Used items will be shown as disabled</param>
        /// <param name="onItemAdded">Action to call when an item is added</param>
        /// <param name="separator">Entry separator logic</param>
        /// <param name="ignore">Items to ignore (not show) from the Items array</param>
        /// <param name="backText">Text to show on the top part (back area)</param>
        public static void Show(Rect position, string[] items, string[] usedItems, Action<string> onItemAdded, 
                                string separator = null, string[] ignore = null, string backText = "Select Item", bool returnFullPath = true)
        {
            Instance.Init(position, separator, backText, items, ignore, usedItems, onItemAdded, returnFullPath);
            Instance.Repaint();
        }
        
        void Init(Rect rect, string separator, string backText, string[] items, string[] ignore, string[] usedItems, Action<string> onItemAdded, bool returnFullPath)
        {
            Vector2 v2 = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
            rect.x = v2.x;
            rect.y = v2.y;
            
            ShowAsDropDown(rect, new Vector2(rect.width, 320f));
            Focus();
            wantsMouseMove = true;

            m_separation = separator;
            m_onItemAdded = onItemAdded;
            m_availableItems = items;
            m_ignoreItems = ignore;
            m_usedItems = usedItems;
            m_backText = backText;
            m_useFullPath = returnFullPath;
            
            CalculateVisibleElements();
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0.0f, 0.0f, position.width, position.height), GUIContent.none,
                      Styles.Background);

            Rect searchArea = new Rect(2, 8, position.width - 4, EditorGUIUtility.singleLineHeight);

            if (searchArea.Contains(Event.current.mousePosition))
            {
                EditorGUIUtility.AddCursorRect(searchArea, MouseCursor.Text);
            }

            GUILayout.Space(7);
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Space(16);

                GUI.SetNextControlName("SearchTextField");
                m_searchString = GUILayout.TextField(m_searchString, GUI.skin.FindStyle("SearchTextField"));
                GUI.FocusControl("SearchTextField");
                if (GUILayout.Button("", string.IsNullOrEmpty(m_searchString) ? GUI.skin.FindStyle("SearchCancelButtonEmpty") : GUI.skin.FindStyle("SearchCancelButton")))
                {
                    m_searchString = "";
                    GUI.FocusControl(null);
                }
                
                GUILayout.Space(16);
            }
            
            Rect rect = position;
            rect.x = +1f;
            rect.y = 30f;
            rect.height -= 30f;
            rect.width -= 2f;

            using (new GUILayout.AreaScope(rect))
            {
                DrawBackButton();
                DrawElements();
            }
        }

        void GoBack()
        {
            if (m_insideSection.EndsWith(m_separation))
                m_insideSection = m_insideSection.Substring(0, m_insideSection.LastIndexOf(m_separation));

            if (m_insideSection.Contains(m_separation))
                m_insideSection =
                    m_insideSection.Substring(0, m_insideSection.LastIndexOf(m_separation) + 1);
            else
                m_insideSection = string.Empty;

            m_searchString = string.Empty;
            GUI.FocusControl(null);
            CalculateVisibleElements(m_insideSection);
        }
        
        void DrawBackButton()
        {
            Rect rect = GUILayoutUtility.GetRect(10f, 25f);

            GUI.Label(rect, string.Empty, Styles.Header);

            if (string.IsNullOrEmpty(m_insideSection))
            {
                GUI.Label(rect, string.IsNullOrEmpty(m_searchString) ? m_backText : "Search", Styles.HeaderText);
            }
            else
            {
                if (GUI.Button(rect, m_insideSection, Styles.HeaderText))
                {
                    GoBack();
                    return;
                }

                GUI.Label(new Rect(5, 5, 15, 15), string.Empty, Styles.LeftArrow);
            }
        }
        
        void DrawElements()
        {
            string searchString = m_searchString.ToLower();
            Event currentEvent = Event.current;
            bool moveInside = false;
            bool enter = false;

            if (m_prevMousePos == null)
                m_prevMousePos = currentEvent.mousePosition;

            if (currentEvent.isKey)
            {
                switch (currentEvent.keyCode)
                {
                    case KeyCode.UpArrow:
                        m_selectionIndex--;
                        break;
                    
                    case KeyCode.DownArrow:
                        m_selectionIndex++;
                        break;
                    
                    case KeyCode.LeftArrow:
                        if (string.IsNullOrEmpty(searchString))
                            GoBack();
                        return;
                    
                    case KeyCode.RightArrow:
                    case KeyCode.Tab:
                        if (string.IsNullOrEmpty(searchString))
                            moveInside = true;
                        break;
                    
                    case KeyCode.KeypadEnter:
                    case KeyCode.Return:
                        enter = true;
                        break;
                }
            }

            m_selectionIndex = Mathf.Clamp(m_selectionIndex, 0, m_sectionData.Count - 1);
            
            using (GUILayout.ScrollViewScope scrollViewScope = new GUILayout.ScrollViewScope(m_scrollPosition))
            {
                m_scrollPosition = scrollViewScope.scrollPosition;
                bool odd = false;

                for (int i = 0; i < m_sectionData.Count; i++)
                {
                    SectionData section = m_sectionData[i];
                    
                    if (m_ignoreItems != null && m_ignoreItems.Contains(m_insideSection + section.data))
                        continue;

                    if (!string.IsNullOrEmpty(searchString) && !section.data.ToLower().Contains(searchString))
                        continue;

                    Rect buttonRect = GUILayoutUtility.GetRect(16f, 20f, GUILayout.ExpandWidth(true));

                    if ((m_prevMousePos.Value != currentEvent.mousePosition) && (buttonRect.Contains(currentEvent.mousePosition)))
                    {
                        m_selectionIndex = i;
                    }

                    if (m_selectionIndex == i)
                    {
                        EditorGUI.DrawRect(buttonRect, Styles.SelectionColor);
                        Repaint();
                    }
                    else
                    {
                        if (odd)
                            EditorGUI.DrawRect(buttonRect, Styles.OddElementColor);
                        else
                            EditorGUI.DrawRect(buttonRect, Styles.EvenElementColor);
                    }

                    if (section.isSection)
                    {
                        float lineHeight = EditorGUIUtility.singleLineHeight;
                        GUI.Label(new Rect(buttonRect.xMax - lineHeight, buttonRect.y, lineHeight, lineHeight),
                                  string.Empty, Styles.RightArrow);
                    }

                    GUI.enabled = !(m_usedItems != null && m_usedItems.Contains(m_insideSection + section.data));

                    if (GUI.Button(buttonRect, section.data, Styles.ComponentButton))
                    {
                        if (section.isSection)
                        {
                            moveInside = true;
                        }
                        else
                        {
                            enter = true;
                        }
                    }

                    if (enter && m_selectionIndex == i)
                    {
                        if (section.isSection)
                            moveInside = true;
                        else
                        {
                            m_onItemAdded(m_useFullPath ? m_insideSection : "" + section.data);
                            Close();
                        }
                    }
                    
                    if (moveInside && section.isSection && m_selectionIndex == i)
                    {
                        m_insideSection += section.data + m_separation;
                        m_searchString = string.Empty;
                        CalculateVisibleElements(m_insideSection);
                    }

                    odd = !odd;
                }
            }
            
            m_prevMousePos = currentEvent.mousePosition;
        }

        /// <summary>
        /// Used to re calculate what items are available depending on the current section (prefix)
        /// </summary>
        /// <param name="prefix">Section where the search is</param>
        void CalculateVisibleElements(string prefix = null)
        {
            m_sectionData.Clear();
            
            foreach (string item in m_availableItems)
            {
                SectionData data = new SectionData();

                string itemToAdd = item;

                if (!string.IsNullOrEmpty(prefix))
                {
                    if (!item.StartsWith(prefix))
                        continue;
                    
                    itemToAdd = itemToAdd.Remove(0, prefix.Length);
                }

                if (!string.IsNullOrEmpty(m_separation) && itemToAdd.Contains(m_separation))
                {
                    itemToAdd = itemToAdd.Split(new[] {m_separation}, StringSplitOptions.RemoveEmptyEntries)[0];
                    
                    data.isSection = true;
                }

                data.data = itemToAdd;
                
                if (!m_sectionData.Contains(data))
                    m_sectionData.Add(data);
            }
        }
    }
}