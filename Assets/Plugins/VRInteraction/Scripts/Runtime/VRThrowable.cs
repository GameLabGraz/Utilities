using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GameLabGraz.VRInteraction
{
    public class VRThrowable : Throwable
    {
        [Header("VR Interaction Plugin")] 
        public bool keepPressingToHoldInteractable = true;

        protected enum GrabState
        {
            GrabNone,
            GrabFirstGrab,
            GrabFirstDegrab,
            GrabSecondGrab
        }

        protected GrabState _currentState = GrabState.GrabNone;
        protected GrabTypes _lastGrabType = GrabTypes.None;

        protected override void HandHoverUpdate(Hand hand)
        {
            var startingGrabType = hand.GetGrabStarting();

            if (startingGrabType != GrabTypes.None 
                && (_currentState == GrabState.GrabNone || keepPressingToHoldInteractable))
            {
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags, attachmentOffset);
                hand.HideGrabHint();

                _lastGrabType = startingGrabType;
                _currentState = GrabState.GrabFirstGrab;
            }
        }

        protected override void HandAttachedUpdate(Hand hand)
        {
            if (keepPressingToHoldInteractable)
            {
                //same as in original class
                if (hand.IsGrabEnding(this.gameObject))
                {
                    hand.DetachObject(gameObject, restoreOriginalParent);
                }
            }
            else
            {
                var currentGrabState = hand.GetGrabStarting();
                switch (_currentState)
                {
                    case GrabState.GrabFirstGrab:
                        if (currentGrabState == GrabTypes.None)
                        {
                            _currentState = GrabState.GrabFirstDegrab;
                        }
                        break;
                    
                    case GrabState.GrabFirstDegrab:
                        if (currentGrabState != GrabTypes.None)
                        {
                            _currentState = GrabState.GrabSecondGrab;
                        }
                        break;
                    
                    case GrabState.GrabSecondGrab:
                        if (currentGrabState == GrabTypes.None)
                        {
                            _currentState = GrabState.GrabNone;
                            hand.DetachObject(gameObject, restoreOriginalParent);
                        }
                        break;
                }
            }

            if (onHeldUpdate != null)
                onHeldUpdate.Invoke(hand);
        }
    }
}