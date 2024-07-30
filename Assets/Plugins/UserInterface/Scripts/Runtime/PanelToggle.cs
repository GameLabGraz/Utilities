using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    [SerializeField] private Transform _arrow;
    [SerializeField] private GameObject _content;

    public void Toggle() {
        if (_content.activeSelf) {
            _content.SetActive(false);
        } else {
            _content.SetActive(true);
        }
        
        RotateArrow();
    }

    void RotateArrow() {
        _arrow.Rotate(0, 0, 180);
    }
}
