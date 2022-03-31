using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GameLabGraz.VRInteraction
{
    public class VRHighlighter : MonoBehaviour
    {
        public bool _showDebugMessages = false;
        public Color highlightColor = Color.cyan;
        public Color pressedColor = Color.gray;
        public Color defaultColor = Color.white;
        public Color attachedColor = Color.white;
        public List<GameObject> highlightedObjects = new List<GameObject>();

        private Color _defaultCol;
        private bool _isHovering = false;

        protected void Start()
        {
            var maroonHoverBtn = GetComponent<VRHoverButton>();
            if(maroonHoverBtn)
            {
                if (maroonHoverBtn.stayPressed)
                {
                    maroonHoverBtn.OnButtonOn.AddListener(() => { _defaultCol = pressedColor; });
                    maroonHoverBtn.OnButtonOff.AddListener(() => { _defaultCol = defaultColor; });
                }
                else
                {
                    _defaultCol = defaultColor;
                }
                maroonHoverBtn.onButtonDown.AddListener(OnButtonDown);
                maroonHoverBtn.onButtonUp.AddListener(OnButtonUp);
            }
            else
            {
                var hoverButton = GetComponent<HoverButton>();
                if (hoverButton)
                {
                    _defaultCol = defaultColor;
                    hoverButton.onButtonDown.AddListener(OnButtonDown);
                    hoverButton.onButtonUp.AddListener(OnButtonUp);
                }
            }

            var snapZone = GetComponent<VRSnapDropZone>();
            if (snapZone)
            {
                //Debug.Log("We have a snap zone");
                ColorObj(snapZone.HighlightedObject);
                //Debug.Log("We have a highlighted obj");
                snapZone.onStartHighlight.AddListener(sz =>
                {
                    if(_showDebugMessages)
                        Debug.Log("Highlighter: onStartHighlight");
                    sz.HighlightedObject?.SetActive(true);
                });                    
                snapZone.onEndHighlight.AddListener(sz => {
                    if(_showDebugMessages)
                        Debug.Log("Highlighter: onEndHighlight");
                    sz.HighlightedObject?.SetActive(false);
                });
            }
            
            var hoverEvents = GetComponent<InteractableHoverEvents>();
            if(hoverEvents){
                hoverEvents.onHandHoverBegin.AddListener(() => { 
                    ColorSelf(highlightColor);
                    _isHovering = true;
                });
                hoverEvents.onHandHoverEnd.AddListener(() =>
                {
                    ColorSelf(defaultColor);
                    _isHovering = false;
                });
                hoverEvents.onAttachedToHand.AddListener(() => { ColorSelf(attachedColor); });
                hoverEvents.onDetachedFromHand.AddListener(() =>
                {
                    if (_isHovering)
                        ColorSelf(highlightColor);
                    else
                        ColorSelf(defaultColor);
                });
            }
        }
        
        public void OnButtonDown(Hand fromHand)
        {
            ColorSelf(highlightColor);
        }

        public void OnButtonUp(Hand fromHand)
        {
            ColorSelf(_defaultCol);
        }

        private void ColorSelf(Color newColor)
        {
            if (highlightedObjects.Count == 0)
            {
                ColorSelf(gameObject, newColor);
            }
            else
            {
                foreach (var obj in highlightedObjects)
                    ColorSelf(obj, newColor);
            }
        }

        private void ColorSelf(GameObject obj, Color newColor)
        {
            var r = obj.GetComponent<Renderer>();
            var info = obj.GetComponent<VRHighlightInfo>();
            if (r)
            {
                if (info && info.MaterialIndex < r.materials.Length)
                {
                    r.materials[info.MaterialIndex].color = newColor;
                }
                else
                {
                    foreach (var mat in r.materials)
                        mat.color = newColor;
                }
            }

            var renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (var rend in renderers)
            {
                var childInfo = rend.gameObject.GetComponent<VRHighlightInfo>();
                if (childInfo && childInfo.MaterialIndex < rend.materials.Length)
                {
                    rend.materials[childInfo.MaterialIndex].color = newColor;
                }
                else
                {
                    foreach (var mat in rend.materials)
                        mat.color = newColor;
                }
            }
        }
        
        // Only used for snap zone -> we never need to change the obj color back
        private void ColorObj(GameObject obj)
        {
            if (!obj) return;
            var renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (var r in renderers)
                SetRendererPermanentHighlightColor(r);
            renderers = obj.GetComponents<Renderer>();
            foreach (var r in renderers)
                SetRendererPermanentHighlightColor(r);
        }

        private void SetRendererPermanentHighlightColor(Renderer r)
        {
            if(!r)
                return;
            
            foreach (var mat in r.materials)
            {
                if(!mat || !mat.HasProperty("_Color"))
                    continue;
            
                var oldCol = mat.color;
                mat.color = new Color(oldCol.r * highlightColor.r, oldCol.g * highlightColor.g,
                    oldCol.b * highlightColor.b, highlightColor.a);
            }
        }
    }
}
