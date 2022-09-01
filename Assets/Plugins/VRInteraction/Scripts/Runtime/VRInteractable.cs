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
        public bool interactable = true;
        public bool hoverable = true;

        protected bool prevHighlightOnHover;

        protected override void Start()
        {
            if (!hoverable)
            {
                var colliders = GetComponentsInChildren<Collider>();
                foreach(var collider in colliders)
                {
                    collider.gameObject.AddComponent(typeof(IgnoreHovering));
                }
            }
        }

        protected override void OnHandHoverBegin(Hand hand)
        {
            if (!hoverable)
                return;
            
            base.OnHandHoverBegin(hand);
        }
        
        protected override void OnHandHoverEnd(Hand hand)
        {
            if (!hoverable)
                return;
            base.OnHandHoverEnd(hand);
        }
        
        protected virtual void HandHoverUpdate( Hand hand )
        {
            if (!hoverable)
                return;
        }

        protected override void OnAttachedToHand(Hand hand)
        {
            if (!interactable || !hoverable)
                return;
            
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
            if (!interactable || !hoverable)
                return;
            
            if (!allowHandSwitching)
            {
                //reset the highlightOnHover variable
                highlightOnHover = prevHighlightOnHover;
            }
            base.OnDetachedFromHand(hand);
        }
        
        protected virtual void HandAttachedUpdate(Hand hand)
        {
            if (!interactable || !hoverable)
                return;
        }

        public bool IsAttachedToHand()
        {
            return attachedToHand != null;
        }

        public bool IsInteractable()
        {
            return hoverable && interactable;
        }
    }
}
