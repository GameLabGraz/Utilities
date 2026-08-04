using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RotaryHeart.Lib.AutoComplete
{
    internal class AddItemWindow
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
        
        class Styles : IStyle
        {
            readonly GUIStyle header;
            readonly GUIStyle headerText;
            readonly GUIStyle componentButton;
            readonly Color backgroundColor;
            readonly Color selectionColor;
            readonly Color currentElementColor;
            readonly Color oddElementColor;
            readonly Color evenElementColor;
            readonly Texture2D whiteTexture;
            readonly GUIStyle searchBarStyle;
            readonly GUIStyle clearSearchButtonStyle;
            
            public GUIStyle Header
            {
                get { return header; }
            }
            public GUIStyle HeaderText
            {
                get { return headerText; }
            }
            public GUIStyle ComponentButton
            {
                get { return componentButton; }
            }
            public Color BackgroundColor
            {
                get { return backgroundColor; }
            }
            public Color SelectionColor
            {
                get { return selectionColor; }
            }
            public Color CurrentElementColor
            {
                get { return currentElementColor; }
            }
            public Color OddElementColor
            {
                get { return oddElementColor; }
            }
            public Color EvenElementColor
            {
                get { return evenElementColor; }
            }
            public Texture2D Background
            {
                get { return null; }
            }
            public Texture2D WhiteTexture
            {
                get { return whiteTexture; }
            }
            public GUIStyle SearchBarStyle
            {
                get { return searchBarStyle; }
            }
            public GUIStyle ClearSearchButtonStyle
            {
                get { return clearSearchButtonStyle; }
            }

            public Styles()
            {
                header = new GUIStyle("In BigTitle");
                
                headerText = new GUIStyle("BoldLabel")
                {
                    alignment = TextAnchor.MiddleCenter
                };

                componentButton = new GUIStyle("Label")
                {
                    alignment = TextAnchor.MiddleLeft,
                    fixedHeight = 20f,
                    normal =
                    {
                        textColor = Color.black
                    },
                    hover =
                    {
                        textColor = Color.white
                    },
                    active =
                    {
                        background = null
                    }
                };

                selectionColor = new Color(0.24f, 0.49f, 0.91f);
                currentElementColor = new Color(0.31f, 0.54f, 0.8f);
                oddElementColor = new Color(0.76f, 0.76f, 0.76f);
                evenElementColor = new Color(0.72f, 0.72f, 0.72f);
                backgroundColor = new Color(.72f, .72f, .72f);
                
                whiteTexture = new Texture2D(1, 1);
                WhiteTexture.SetPixel(0, 0, Color.white);
                WhiteTexture.Apply();
                
                searchBarStyle = new GUIStyle(GUI.skin.textArea);
                
                clearSearchButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    alignment = TextAnchor.MiddleCenter
                };
            }
        }
        
        readonly List<SectionData> m_sectionData = new List<SectionData>();
        
        Action<string> m_onItemAdded;
        string[] m_availableItems;
        string[] m_ignoreItems;
        string[] m_usedItems;
        IStyle m_styles;

        Rect m_position;
        Vector2 m_scrollPosition;
        string m_separation = string.Empty;
        string m_searchString = string.Empty;
        string m_insideSection = string.Empty;
        string m_backText = string.Empty;
        bool m_useFullPath = true;
        bool m_allowCustom = false;
        bool m_allowEmpty = false;
        bool m_firstOpen = true;
        bool m_closed = false;
        int m_selectionIndex = -1;
        Vector2? m_prevMousePos;

        public bool Closed { get { return m_closed; } }
        public Rect Position { get { return m_position; } }

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
        /// <param name="returnFullPath">If ture, returns the full path including the separator; otherwise just the selected item</param>
        /// <param name="allowCustom">Allow custom items to be added from the search bar</param>
        /// <param name="style">Contains the style to use for the window (colors, textures, etc)</param>
        /// <param name="allowEmpty">Allows to select an empty element</param>
        public void Show(Rect position, string[] items, string[] usedItems, Action<string> onItemAdded, 
                                string separator = null, string[] ignore = null, string backText = "Select Item", bool returnFullPath = true, bool allowCustom = false, IStyle style = null, bool allowEmpty = false)
        {
            Init(position, separator, backText, items, ignore, usedItems, onItemAdded, returnFullPath, allowCustom, allowEmpty, style);
        }
        
        void Init(Rect rect, string separator, string backText, string[] items, string[] ignore, string[] usedItems, Action<string> onItemAdded, bool returnFullPath, bool allowCustom, bool allowEmpty, IStyle style)
        {
            Vector2 v2 = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
            rect.x = v2.x;
            rect.y = v2.y;

            m_position = new Rect(rect.position, new Vector2(rect.width, 320f));
            // ShowAsDropDown(rect, new Vector2(rect.width, 320f));
            // Focus();
            // wantsMouseMove = true;

            m_separation = separator;
            m_onItemAdded = onItemAdded;
            m_availableItems = items;
            m_ignoreItems = ignore;
            m_usedItems = usedItems;
            m_backText = backText;
            m_useFullPath = returnFullPath;
            m_allowCustom = allowCustom;
            m_allowEmpty = allowEmpty;

            if (style == null)
                m_styles = new Styles();
            else
                m_styles = style;
            
            CalculateVisibleElements();
        }

        public void OnGUI()
        {
            GUI.BeginGroup(m_position);

            Color prevColor = GUI.color;
            GUI.color = m_styles.BackgroundColor;
            GUI.DrawTexture(new Rect(0.0f, 0.0f, m_position.width, m_position.height), m_styles.Background == null ? m_styles.WhiteTexture : m_styles.Background);
            GUI.color = prevColor;
            // GUI.Label(new Rect(0.0f, 0.0f, m_position.width, m_position.height), GUIContent.none, Styles.Background);
            GUILayout.Space(7);
            using (new GUILayout.HorizontalScope(GUILayout.Width(m_position.width)))
            {
                GUILayout.Space(16);

                GUI.SetNextControlName("SearchTextField");
                m_searchString = GUILayout.TextField(m_searchString, m_styles.SearchBarStyle);//, GUI.skin.FindStyle("SearchTextField"));
                if (m_firstOpen)
                {
                    GUI.FocusControl("SearchTextField");
                    m_firstOpen = false;
                }
                if (GUILayout.Button("x", m_styles.ClearSearchButtonStyle, GUILayout.Width(20)))//, string.IsNullOrEmpty(m_searchString) ? GUI.skin.FindStyle("SearchCancelButtonEmpty") : GUI.skin.FindStyle("SearchCancelButton")))
                {
                    m_searchString = "";
                    GUI.FocusControl(null);
                }
                
                GUILayout.Space(16);
            }
            
            Rect rect = m_position;
            rect.x = +1f;
            rect.y = 30f;
            rect.height -= 30f;
            rect.width -= 2f;

            using (new GUILayout.AreaScope(rect))
            {
                DrawBackButton();
                DrawElements();
            }

            GUI.EndGroup();

            if (Event.current.isMouse && Event.current.button == 0 && !m_position.Contains(Event.current.mousePosition))
            {
                Close();
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

            GUI.FocusControl(null);
            CalculateVisibleElements(m_insideSection);
            m_selectionIndex = -1;
        }
        
        void DrawBackButton()
        {
            Color prevColor = GUI.color;
            GUI.color = Color.black;
            Rect rect = GUILayoutUtility.GetRect(10f, 25f);

            GUI.Label(rect, string.Empty, m_styles.Header);

            if (string.IsNullOrEmpty(m_insideSection))
            {
                GUI.Label(rect, string.IsNullOrEmpty(m_searchString) ? m_backText : "Search", m_styles.HeaderText);
            }
            else
            {
                if (GUI.Button(rect, m_insideSection, m_styles.HeaderText))
                {
                    GoBack();
                    return;
                }

                GUI.Label(new Rect(5, 5, 20, 20), "<");
            }
            GUI.color = prevColor;
        }
        
        void DrawElements()
        {
            string searchString = m_searchString.ToLower();
            Event currentEvent = Event.current;
            bool moveInside = false;
            bool enter = false;

            if (m_prevMousePos == null)
                m_prevMousePos = currentEvent.mousePosition;

            if (currentEvent.isKey && currentEvent.type == EventType.KeyUp)
            {
                switch (currentEvent.keyCode)
                {
                    case KeyCode.UpArrow:
                        m_selectionIndex--;
                        GUI.FocusControl(null);
                        break;
                    
                    case KeyCode.DownArrow:
                        m_selectionIndex++;
                        GUI.FocusControl(null);
                        break;
                    
                    case KeyCode.LeftArrow:
                        if (string.IsNullOrEmpty(GUI.GetNameOfFocusedControl()))
                            GoBack();
                        return;
                    
                    case KeyCode.RightArrow:
                    case KeyCode.Tab:
                        if (string.IsNullOrEmpty(GUI.GetNameOfFocusedControl()))
                            moveInside = true;
                        break;
                    
                    case KeyCode.KeypadEnter:
                    case KeyCode.Return:
                        enter = true;
                        if (GUI.GetNameOfFocusedControl().Equals("SearchTextField"))
                        {
                            if (string.IsNullOrEmpty(searchString) && m_allowEmpty)
                            {
                                m_onItemAdded(string.Empty);
                                Close();
                                return;
                            }

                            if (m_allowCustom)
                            {
                                m_onItemAdded((m_useFullPath ? m_insideSection : "") + searchString);
                                Close();
                                return;
                            }
                        }
                        break;
                    
                    case KeyCode.Backspace:
                        GUI.FocusControl("SearchTextField");
                        break;
                    
                    case KeyCode.Escape:
                        Close();
                        break;
                }
            }

            m_selectionIndex = Mathf.Clamp(m_selectionIndex, -1, m_sectionData.Count - 1);

            if (m_selectionIndex == -1)
            {
                GUI.FocusControl("SearchTextField");
            }
            
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

                    Color prevColor = GUI.color;
                    
                    if (m_selectionIndex == i)
                    {
                        GUI.color = m_styles.SelectionColor;
                    }
                    else if (m_usedItems != null && m_usedItems.Contains(m_insideSection + section.data))
                    {
                        GUI.color = m_styles.CurrentElementColor;
                    }
                    else
                    {
                        if (odd)
                        {
                            GUI.color = m_styles.OddElementColor;
                        }
                        else
                        {
                            GUI.color = m_styles.EvenElementColor;
                        }
                    }

                    GUI.DrawTexture(buttonRect, m_styles.WhiteTexture);
                    GUI.color = prevColor;

                    if (section.isSection)
                    {
                        float lineHeight = 16;
                        GUI.Label(new Rect(buttonRect.xMax - lineHeight, buttonRect.y, lineHeight, lineHeight), ">", m_styles.ComponentButton);
                    }

                    if (GUI.Button(buttonRect, section.data, m_styles.ComponentButton))
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

                    //TODO: Modify this to allow selecting an empty element
                    if (enter && m_selectionIndex == i)
                    {
                        if (section.isSection)
                            moveInside = true;
                        else
                        {
                            if (section.data.Equals("(Nothing)"))
                            {
                                m_onItemAdded((m_useFullPath ? m_insideSection : string.Empty) + string.Empty);
                            }
                            else
                            {
                                m_onItemAdded((m_useFullPath ? m_insideSection : string.Empty) + section.data);
                            }
                            Close();
                        }
                    }
                    
                    if (moveInside && section.isSection && m_selectionIndex == i)
                    {
                        m_insideSection += section.data + m_separation;
                        CalculateVisibleElements(m_insideSection);
                        m_selectionIndex = 0;
                        break;
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
            
            if (m_allowEmpty)
            {
                m_sectionData.Add(new SectionData()
                {
                    data = "(Nothing)",
                    isSection = false
                });
            }
            
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

        void Close()
        {
            m_firstOpen = true;
            m_closed = true;
        }
    }
}