using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GameLabGraz.VRInteraction
{
    public class VRInteractable : Interactable
    {
        [Header("VR Interaction Plugin")] 
        public bool allowHandSwitching = true;

        protected bool prevHighlightOnHover;

        protected override void OnAttachedToHand(Hand hand)
        {
            if (allowHandSwitching || attachedToHand == null)
            {
                base.OnAttachedToHand(hand);

                if (!allowHandSwitching)
                {
                    prevHighlightOnHover = highlightOnHover;
                    // only that it doesn't get highlighted when the second hand is hovering
                    highlightOnHover = false;
                }
            }
        }

        protected override void OnDetachedFromHand(Hand hand)
        {
            if (!allowHandSwitching)
            {
                //reset the highlightOnHover variable
                highlightOnHover = prevHighlightOnHover;
            }
            base.OnDetachedFromHand(hand);
        }

        public bool IsAttachedToHand()
        {
            return attachedToHand != null;
        }
    }
}
