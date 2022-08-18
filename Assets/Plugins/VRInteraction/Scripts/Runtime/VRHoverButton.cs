using System;
using System.Linq;
using System.Reflection;
using PrivateAccess;
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
        [Header("VR Interaction Plugin")]
        public bool stayPressed = true;
        public Vector3 localPressedDistance = new Vector3(0, -0.5f, 0);

        public bool initialState;

        [Tooltip("UnityEvent(bool): When the button is pressed down. True when stays pressed, false otherwise.")]
        public ValueChangeEventBool OnButtonPressed;
        [Tooltip("UnityEvent(void): When the button is pressed down and is in its 'pressed' state.")]
        public UnityEvent OnButtonOn;
        [Tooltip("UnityEvent(void): When the button is pressed down and is in its 'not pressed' state.")]
        public UnityEvent OnButtonOff;
        
        [Tooltip("UnityEvent(bool): When the button state is changed via the force method.")]
        public ValueChangeEventBool OnForcedButtonState;
        
        protected bool _isPressed;
        protected bool _lastEngaged;
        protected Vector3 _pressedPosition;
        protected Vector3 _notPressedPosition;

        //#TODO Update SteamVR: remove code start (member variables are protected)
        protected Vector3 StartPosition
        {
            get => this.GetBaseFieldValue<Vector3>("startPosition");
            set => this.SetBaseFieldValue("startPosition", value);
        }
        protected Vector3 EndPosition
        {
            get => this.GetBaseFieldValue<Vector3>("endPosition");
            set => this.SetBaseFieldValue("endPosition", value);
        }
        protected Vector3 HandEnteredPosition
        {
            get => this.GetBaseFieldValue<Vector3>("handEnteredPosition");
            set => this.SetBaseFieldValue("handEnteredPosition", value);
        }
        protected bool Hovering
        {
            get => this.GetBaseFieldValue<bool>("hovering");
            set => this.SetBaseFieldValue("hovering", value);
        }
        protected Hand LastHoveredHand
        {
            get => this.GetBaseFieldValue<Hand>("lastHoveredHand");
            set => this.SetBaseFieldValue("lastHoveredHand", value);
        }
        //#TODO Update SteamVR: remove code end (member variables are protected)
        
        protected void Start()
        {
            //#TODO Update SteamVR: call base.Start() once it is protected
            this.CallBaseMethod("Start", new object[0]);
            // base.Start();
            
            _notPressedPosition = StartPosition;
            _pressedPosition = _notPressedPosition + localPressedDistance;
            
            ForceButtonState(initialState);
        }

        protected void HandHoverUpdate(Hand hand)
        {
            Hovering = true;
            LastHoveredHand = hand;

            var wasEngaged = engaged;

            var currentDistance = Vector3.Distance(movingPart.parent.InverseTransformPoint(hand.transform.position), EndPosition);
            var enteredDistance = Vector3.Distance(HandEnteredPosition, EndPosition);
            
            if (currentDistance > enteredDistance)
            {
                enteredDistance = currentDistance;
                HandEnteredPosition = movingPart.parent.InverseTransformPoint(hand.transform.position);
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
                StartPosition = _isPressed ? _pressedPosition : _notPressedPosition;

                OnButtonPressed.Invoke(_isPressed);
                if(_isPressed) OnButtonOn.Invoke();
                else OnButtonOff.Invoke();
            }
            _lastEngaged = engaged;

            movingPart.localPosition = Vector3.Lerp(StartPosition, EndPosition, lerp);
            
            //#TODO Update SteamVR: call InvokeEvents once it is protected
            this.CallBaseMethod("InvokeEvents", new object[]{wasEngaged, engaged});
            // InvokeEvents(wasEngaged, engaged);
        }

        // this function is only useful when stayPressed is true
        public void ForceButtonState(bool isOn)
        {
            if (!stayPressed) return;
            if (isOn)
            {
                _isPressed = true;
                StartPosition = _pressedPosition;
                OnButtonPressed.Invoke(true);
                OnButtonOn.Invoke();
            }
            else
            {
                _isPressed = false;
                StartPosition = _notPressedPosition;
                OnButtonPressed.Invoke(false);
                OnButtonOff.Invoke();
            }

            var wasEngaged = engaged;
            _lastEngaged = engaged = false;
            movingPart.localPosition = StartPosition;
            
            //#TODO Update SteamVR: call InvokeEvents once it is protected
            this.CallBaseMethod("InvokeEvents", new object[]{wasEngaged, engaged});
            // InvokeEvents(wasEngaged, engaged);
            
            OnForcedButtonState.Invoke(_isPressed);
        }
    }
}