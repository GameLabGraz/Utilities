using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GEAR.VRInteraction
{
    public class VRLoadingComponent : MonoBehaviour
    {
        public float loadingTime = 0f;
        public List<VRLoadingObject> loadingHighlightObjects = new List<VRLoadingObject>();

        [Tooltip(
            "UnityEvent(void): When loading is enabled and the button has been pressed for 'loading Time' in seconds.")]
        public UnityEvent OnLoaded;
        
        [Tooltip(
            "UnityEvent(void): Called when the loading process starts.")]
        public UnityEvent OnLoadingStart;
        
        [Tooltip(
            "UnityEvent(void): Called when the loading process ends before the 'loading Time' was reached.")]
        public UnityEvent OnLoadingCancel;

        [Tooltip(
            "UnityEvent(float): Event that gets called during loading. Includes the progress in percent between 0 and 1.")]
        public ValueChangeEventFloat OnLoadingProgressChanged;
        
        private bool _isLoading = false;
        private float _currentLoadingTime = 0f;

        public void OnLoading(bool load)
        {
            if(load)
                OnStartLoading();
            else
                OnCancelLoading();
        }
        
        public void OnStartLoading()
        {
            if (_isLoading) return;
            
            Debug.Log("VRInteraction::VRLoadingComponent: Is loading...");
            _currentLoadingTime = 0f;
            _isLoading = true;
            
            UpdateLoadingEffect(0f);
            OnLoadingStart.Invoke();
        }

        public void OnCancelLoading()
        {
            Debug.Log("VRInteraction::VRLoadingComponent: Canceled loading.");
            _currentLoadingTime = 0f;
            _isLoading = false;
            
            UpdateLoadingEffect(0f);
            OnLoadingCancel.Invoke();
        }

        protected void UpdateLoadingEffect(float percentage)
        {
            foreach (var obj in loadingHighlightObjects)
            {
                obj.UpdateLoadingEffect(percentage);
            }
            OnLoadingProgressChanged.Invoke(percentage);
        }

        protected void FixedUpdate()
        {
            if (_isLoading)
            {
                _currentLoadingTime += Time.deltaTime;
                UpdateLoadingEffect(Mathf.Clamp(_currentLoadingTime / loadingTime, 0f, 1f));

                if (_currentLoadingTime >= loadingTime)
                {
                    _isLoading = false;
                    UpdateLoadingEffect(1f);
                    OnLoaded.Invoke();
                }
            }
        }
    }
}