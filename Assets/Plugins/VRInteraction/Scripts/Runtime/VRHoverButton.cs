using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

namespace GameLabGraz.VRInteraction
{
    /**
     * #TODO SteamVR
     * HINT: you may need to make all private variables and methods of HoverButton protected
     */
    
    public class VRHoverButton : HoverButton
    {
        [Header("Maroon VR Specific")]
        public bool stayPressed = true;
        public Vector3 localPressedDistance = new Vector3(0, -0.5f, 0);

        [Tooltip("UnityEvent(bool): When the button is pressed down. True when stays pressed, false otherwise.")]
        public ValueChangeEventBool OnButtonPressed;
        [Tooltip("UnityEvent(void): When the button is pressed down and is in its 'pressed' state.")]
        public UnityEvent OnButtonOn;
        [Tooltip("UnityEvent(void): When the button is pressed down and is in its 'not pressed' state.")]
        public UnityEvent OnButtonOff;
        
        private bool _isPressed = false;
        private bool _lastEngaged = false;
        private Vector3 _pressedPosition;
        private Vector3 _notPressedPosition;
        
        // Start is called before the first frame update
        protected new void Start()
        {
            base.Start();
            _notPressedPosition = startPosition;
            _pressedPosition = startPosition + localPressedDistance;
        }

        protected new void HandHoverUpdate(Hand hand)
        {
            hovering = true;
            lastHoveredHand = hand;

            var wasEngaged = engaged;

            var currentDistance = Vector3.Distance(movingPart.parent.InverseTransformPoint(hand.transform.position), endPosition);
            var enteredDistance = Vector3.Distance(handEnteredPosition, endPosition);
            
            if (currentDistance > enteredDistance)
            {
                enteredDistance = currentDistance;
                handEnteredPosition = movingPart.parent.InverseTransformPoint(hand.transform.position);
            }

            var distanceDifference = enteredDistance - currentDistance;
            var lerp = Mathf.InverseLerp(0, localMoveDistance.magnitude, distanceDifference);

            if (lerp > engageAtPercent)
                engaged = true;
            else if (lerp < disengageAtPercent)
                engaged = false;

            if (stayPressed && _lastEngaged != engaged && engaged)
            {
                _isPressed = !_isPressed;
                startPosition = _isPressed ? _pressedPosition : _notPressedPosition;

                OnButtonPressed.Invoke(_isPressed);
                if(_isPressed) OnButtonOn.Invoke();
                else OnButtonOff.Invoke();
            }
            _lastEngaged = engaged;

            movingPart.localPosition = Vector3.Lerp(startPosition, endPosition, lerp);
            InvokeEvents(wasEngaged, engaged);
        }

        // this function is only useful when stayPressed is true
        public void ForceButtonState(bool isOn)
        {
            if (!stayPressed) return;
            if (isOn)
            {
                _isPressed = true;
                startPosition = _pressedPosition;
                OnButtonPressed.Invoke(true);
                OnButtonOn.Invoke();
            }
            else
            {
                _isPressed = false;
                startPosition = _notPressedPosition;
                OnButtonPressed.Invoke(false);
                OnButtonOff.Invoke();
            }

            var wasEngaged = engaged;
            _lastEngaged = engaged = false;
            movingPart.localPosition = startPosition;
            InvokeEvents(wasEngaged, engaged);
        }
        
    }
}