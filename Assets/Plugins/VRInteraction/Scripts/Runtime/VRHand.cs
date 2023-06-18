using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GameLabGraz.VRInteraction
{
    public class VRHand : Hand
    {
        public override void AttachObject(GameObject objectToAttach, GrabTypes grabbedWithType,
            AttachmentFlags flags = defaultAttachmentFlags, Transform attachmentOffset = null)
        {
            if (objectToAttach)
            {
                var vrInteraction = objectToAttach.GetComponent<VRInteractable>();
                if (vrInteraction && !vrInteraction.IsInteractable())
                {
                    return;
                }
            }
            base.AttachObject(objectToAttach, grabbedWithType, flags, attachmentOffset);
        }
    }
}