using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoCompleteElement : MonoBehaviour
{
    [SerializeField]
    Button btn;
    [SerializeField]
    TMP_Text text;
    [SerializeField]
    GameObject isSection;
    [SerializeField]
    Sprite nonSelected;
    [SerializeField]
    Sprite selected;
    
    public string Text
    {
        get { return text.text; }
    }
    public bool IsSection
    {
        get { return isSection.activeSelf; }
    }

    public void Init(System.Action<AutoCompleteElement> onClick)
    {
        btn.onClick.AddListener(() =>
        {
            onClick.Invoke(this);
        });
    }

    public void UpdateSelection(bool selected)
    {
        btn.image.sprite = selected ? this.selected : nonSelected;
    }

    public void UpdateData(string text, bool isSection, bool enabled)
    {
        UpdateSelection(false);
        this.text.text = text;
        this.isSection.SetActive(isSection);
        btn.interactable = enabled;
    }

    public void PerformClick()
    {
        btn.onClick.Invoke();
    }
}
