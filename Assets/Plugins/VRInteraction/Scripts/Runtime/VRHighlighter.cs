using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GEAR.VRInteraction
{
    public class VRHighlighter : MonoBehaviour
    {
        public Color highlightColor = Color.cyan;
        public Color pressedColor = Color.gray;
        public Color defaultColor = Color.white;
        public Color attachedColor = Color.white;

        private Color _defaultCol;
        private bool _isHovering = false;

        protected void Start()
        {
            var maroonHoverBtn = GetComponent<VRHoverButton>();
            if(maroonHoverBtn && maroonHoverBtn.stayPressed)
            {
                maroonHoverBtn.OnButtonOn.AddListener(() => { _defaultCol = pressedColor; });
                maroonHoverBtn.OnButtonOff.AddListener(() => { _defaultCol = defaultColor; });
            } 
            var hoverButton = GetComponent<HoverButton>();
            if (hoverButton)
            {
                _defaultCol = default;
                hoverButton.onButtonDown.AddListener(OnButtonDown);
                hoverButton.onButtonUp.AddListener(OnButtonUp);
            }
            
            var snapZone = GetComponent<VRSnapDropZone>();
            if (snapZone)
            {
                Debug.Log("We have a snap zone");
                ColorObj(snapZone.HighlightedObject);
                Debug.Log("We have a highlighted obj");
                snapZone.onStartHighlight.AddListener(sz =>
                {
                    Debug.Log("Highlighter: onStartHighlight");
                    sz.HighlightedObject?.SetActive(true);
                });                    
                snapZone.onEndHighlight.AddListener(sz => {
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
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var t in renderers)
            {
                t.material.color = newColor;
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
            foreach (var mat in r.materials)
            {
                var oldCol = mat.color;
                mat.color = new Color(oldCol.r * highlightColor.r, oldCol.g * highlightColor.g,
                    oldCol.b * highlightColor.b, highlightColor.a);
                
                Debug.Log("new Col: " + mat.color);
            }
        }
    }
}
