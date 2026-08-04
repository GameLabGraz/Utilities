using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RotaryHeart.Lib.AutoComplete
{
    public class AutoCompletePrefab : MonoBehaviour
    {
        struct SectionData
        {
            public string data;
            public bool isSection;
            
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
        
        [SerializeField]
        Sprite clearEmptyText;
        [SerializeField]
        Sprite clearWithText;

        [Header("References")]
        [SerializeField]
        CanvasGroup canvasGroup;
        [SerializeField]
        AutoCompleteElement elementPrefab;
        [SerializeField]
        TMP_InputField search;
        [SerializeField]
        Button clearBtn;
        [SerializeField]
        Button backBtn;
        [SerializeField]
        TMP_Text backText;
        [SerializeField]
        GameObject backButtonArrow;
        [SerializeField]
        RectTransform componentsHolder;

        readonly List<SectionData> m_sectionData = new List<SectionData>();
        readonly List<AutoCompleteElement> m_spawnedElements = new List<AutoCompleteElement>();

        System.Action<string> m_onItemClicked;
        string[] m_availableItems;
        List<string> m_ignoreItems;
        List<string> m_usedItems;
        string m_separation = string.Empty;
        string m_insideSection = string.Empty;
        string m_backText = string.Empty;
        bool m_useFullPath = true;
        bool m_allowCustom = false;
        bool m_mouseIsOver = false;
        int m_selectionIndex = -1;
        
        void Awake()
        {
            search.onValueChanged.AddListener(value =>
            {
                if (search.text.Length > 0)
                    clearBtn.image.sprite = clearWithText;
                else
                    clearBtn.image.sprite = clearEmptyText;

                CalculateVisibleElements(m_insideSection);
            });

            clearBtn.onClick.AddListener(() =>
            {
                search.text = "";
            });

            backBtn.onClick.AddListener(GoBack);
        }

        void OnGUI()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            
            Event currentEvent = Event.current;

            if (currentEvent.isKey && currentEvent.type == EventType.KeyUp)
            {
                switch (currentEvent.keyCode)
                {
                    case KeyCode.UpArrow:
                        if (m_selectionIndex >= 0 && m_selectionIndex < m_spawnedElements.Count)
                            m_spawnedElements[m_selectionIndex].UpdateSelection(false);
                        
                        m_selectionIndex--;
                        
                        if (m_selectionIndex >= 0)
                        {
                            m_spawnedElements[m_selectionIndex].UpdateSelection(true);
                            EventSystem.current.SetSelectedGameObject(m_spawnedElements[m_selectionIndex].gameObject);
                        }
                        else
                        {
                            EventSystem.current.SetSelectedGameObject(search.gameObject);
                        }
                        
                        m_selectionIndex = Mathf.Clamp(m_selectionIndex, -1, m_spawnedElements.Count - 1);
                        break;
                    
                    case KeyCode.DownArrow:
                        if (m_selectionIndex >= 0 && m_selectionIndex < m_spawnedElements.Count)
                            m_spawnedElements[m_selectionIndex].UpdateSelection(false);
                        
                        m_selectionIndex++;

                        m_selectionIndex = Mathf.Clamp(m_selectionIndex, -1, m_spawnedElements.Count - 1);
                        
                        m_spawnedElements[m_selectionIndex].UpdateSelection(true);
                        EventSystem.current.SetSelectedGameObject(m_spawnedElements[m_selectionIndex].gameObject);
                        break;
                    
                    case KeyCode.LeftArrow:
                        if (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.transform.parent == componentsHolder)
                            GoBack();
                        return;
                    
                    case KeyCode.RightArrow:
                    case KeyCode.Tab:
                        if (m_selectionIndex != -1 && m_selectionIndex >= 0 && m_selectionIndex < m_spawnedElements.Count)
                            m_spawnedElements[m_selectionIndex].PerformClick();
                        break;
                    
                    case KeyCode.KeypadEnter:
                    case KeyCode.Return:
                        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject == search.gameObject)
                        {
                            if (string.IsNullOrEmpty(search.text))
                            {
                                m_onItemClicked(string.Empty);
                                Close();
                                return;
                            }

                            if (m_allowCustom)
                            {
                                m_onItemClicked((m_useFullPath ? m_insideSection : "") + search.text);
                                Close();
                                return;
                            }
                            
                            m_spawnedElements[m_selectionIndex].PerformClick();
                        }
                        break;
                    
                    case KeyCode.Backspace:
                        EventSystem.current.SetSelectedGameObject(search.gameObject);
                        break;
                    
                    case KeyCode.Escape:
                        Close();
                        break;
                }
            }

            if (Event.current.isMouse && Event.current.button >= 0 && !IsPointerOverUIElement())
            {
                Close();
            }
        }

        bool IsPointerOverUIElement()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Event.current.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            
            if (raycastResults.Count > 0)
            {
                if (raycastResults[0].gameObject.GetComponentInParent<AutoCompletePrefab>() != null)
                {
                    return true;
                }
            }
            
            return false;
        }

        public void AutoCompleteWindow(string[] items, string[] usedItems, System.Action<string> onItemClicked, string separator = "", string[] ignore = null,
            string backText = "Select Item", bool returnFullPath = true, bool allowCustom = false)
        {
            canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            
            StartCoroutine(Init(items, usedItems, onItemClicked, separator, ignore, backText, returnFullPath, allowCustom));
        }

        IEnumerator Init(string[] items, string[] usedItems, System.Action<string> onItemClicked, string separator = "", string[] ignore = null,
            string backText = "Select Item", bool returnFullPath = true, bool allowCustom = false)
        {
            for (int i = 0; i < 5; i++)
                yield return null;
            
            EventSystem.current.SetSelectedGameObject(search.gameObject);
            
            search.text = "";
            m_insideSection = "";
            m_selectionIndex = -1;
            backButtonArrow.gameObject.SetActive(false);
            
            m_separation = separator;
            m_onItemClicked = onItemClicked;
            m_availableItems = items;
            m_ignoreItems = ignore == null ? null : new List<string>(ignore);
            m_usedItems = usedItems == null ? null : new List<string>(usedItems);
            m_backText = backText;
            m_useFullPath = returnFullPath;
            m_allowCustom = allowCustom;

            this.backText.text = m_backText;
            
            CalculateVisibleElements();
            
            canvasGroup.alpha = 1;
        }

        void GoBack()
        {
            if (m_insideSection.EndsWith(m_separation))
                m_insideSection = m_insideSection.Substring(0, m_insideSection.LastIndexOf(m_separation));

            if (m_insideSection.Contains(m_separation))
                m_insideSection = m_insideSection.Substring(0, m_insideSection.LastIndexOf(m_separation) + 1);
            else
            {
                m_insideSection = string.Empty;
                backText.text = m_backText;
                backButtonArrow.gameObject.SetActive(false);
            }

            CalculateVisibleElements(m_insideSection);
            m_selectionIndex = 0;
        }

        void CalculateVisibleElements(string prefix = null)
        {
            m_sectionData.Clear();
            
            foreach (string item in m_availableItems)
            {
                string itemToAdd = item;
                
                if (!string.IsNullOrEmpty(prefix))
                {
                    if (!item.StartsWith(prefix))
                        continue;

                    itemToAdd = itemToAdd.Remove(0, prefix.Length);
                }
                
                SectionData data = new SectionData();

                if (!string.IsNullOrEmpty(m_separation) && itemToAdd.Contains(m_separation))
                {
                    itemToAdd = itemToAdd.Split(new[] { m_separation }, System.StringSplitOptions.RemoveEmptyEntries)[0];

                    data.isSection = true;
                }

                data.data = itemToAdd;

                if (!m_sectionData.Contains(data))
                    m_sectionData.Add(data);
            }

            foreach (AutoCompleteElement element in m_spawnedElements)
            {
                element.gameObject.SetActive(false);
            }

            for (int i = 0; i < m_sectionData.Count; i++)
            {
                SectionData section = m_sectionData[i];
                AutoCompleteElement element;
                
                if (m_ignoreItems != null && m_ignoreItems.Contains(m_insideSection + section.data))
                    continue;

                if (!string.IsNullOrEmpty(search.text) && !section.data.ToLower().Contains(search.text))
                    continue;

                if (m_spawnedElements.Count <= i)
                {
                    element = Instantiate(elementPrefab, componentsHolder, false);
                    element.Init((clicked) =>
                    {
                        if (clicked.IsSection)
                        {
                            m_insideSection += clicked.Text + m_separation;
                            CalculateVisibleElements(m_insideSection);
                            m_selectionIndex = -1;
                            backText.text = m_insideSection;
                            backButtonArrow.gameObject.SetActive(true);
                        }
                        else
                        {
                            m_onItemClicked((m_useFullPath ? m_insideSection : "") + clicked.Text);
                            Close();
                        }
                    });
                    m_spawnedElements.Add(element);
                }
                else
                {
                    element = m_spawnedElements[i];
                }

                element.gameObject.SetActive(true);
                element.UpdateData(section.data, section.isSection, !(m_usedItems != null && m_usedItems.Contains(m_insideSection + section.data)));
            }
        }

        void Close()
        {
            gameObject.SetActive(false);
        }
    }
}