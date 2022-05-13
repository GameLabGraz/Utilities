using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameLabGraz.UI
{
    [Serializable]
    public class SliderInitEvent : UnityEvent<float> { }

    public class Slider : UnityEngine.UI.Slider
    {
        [SerializeField] private SliderInitEvent onSliderInit;
        [SerializeField] private UnityEvent onStartDrag;
        [SerializeField] private UnityEvent onEndDrag;
        [SerializeField] private UnityEvent onSetSliderValueViaInput;

        protected override void Start()
        {
            base.Start();
            onSliderInit?.Invoke(value);
        }

        public void SetSliderValue(object valueObject)
        {
            try
            {
                value = (float)Convert.ToDouble(valueObject);
                onSetSliderValueViaInput?.Invoke();
            }
            catch (Exception e)
            {
                value = 0;
                Debug.LogException(e);
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            onStartDrag?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            onEndDrag?.Invoke();
        }
    }
}
