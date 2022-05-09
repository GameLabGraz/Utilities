using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GameLabGraz.VRInteraction
{
    public class VRRotatable : MonoBehaviour
    {
        [EnumFlags] [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags =
            Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.TurnOnKinematic;

        [Tooltip("When detaching the object, should it return to its original parent?")]
        public bool restoreOriginalParent = false;

        public bool showGrabHint = false;
        public bool showDebugMessages = false;

        [Header("Locked Rotation")] public bool LockXRotation = false;
        public bool LockYRotation = false;
        public bool LockZRotation = false;

        [HideInInspector] public Interactable interactable;

        protected Quaternion attachRotation;
        protected Quaternion handRotation;
        protected Vector3 attachPosition;
        protected Hand attachedHand = null;

        protected virtual void Awake()
        {
            interactable = GetComponent<Interactable>();
            attachmentFlags &= (~Hand.AttachmentFlags.ParentToHand);
        }

        protected virtual void OnHandHoverBegin(Hand hand)
        {
            if (showGrabHint)
            {
                hand.ShowGrabHint();
            }
        }

        protected virtual void OnHandHoverEnd(Hand hand)
        {
            hand.HideGrabHint();
        }

        protected virtual void HandHoverUpdate(Hand hand)
        {
            var startingGrabType = hand.GetGrabStarting();

            if (startingGrabType != GrabTypes.None)
            {
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
                hand.HideGrabHint();
            }
        }

        protected virtual void OnAttachedToHand(Hand hand)
        {
            attachedHand = hand;
            hand.HoverLock(null);
            attachRotation = transform.rotation;
            attachPosition = transform.position;
            handRotation = hand.transform.rotation;

            if (showDebugMessages)
            {
                Debug.Log("VRRotatable::OnAttachedToHand: Start Rotation = " + attachRotation);
            }
        }

        protected virtual void OnDetachedFromHand(Hand hand)
        {
            attachedHand = null;
            hand.HoverUnlock(null);
        }

        protected virtual void HandAttachedUpdate(Hand hand)
        {
            if (!attachmentFlags.HasFlag(Hand.AttachmentFlags.ParentToHand))
            {
                //update rotation manually
                var currentHandRotation = hand.transform.rotation;

                if (showDebugMessages)
                {
                    Debug.Log("VRRotatable::HandAttachedUpdate: Prev Hand Rotation = " + handRotation.eulerAngles);
                    Debug.Log("VRRotatable::HandAttachedUpdate: Current Hand Rotation = " +
                              currentHandRotation.eulerAngles);
                }

                //get the difference between old and new rotation
                var changed = currentHandRotation * Quaternion.Inverse(handRotation);
                var changedEuler = changed.eulerAngles;

                if (LockXRotation)
                    changedEuler.x = 0;
                if (LockYRotation)
                    changedEuler.y = 0;
                if (LockZRotation)
                    changedEuler.z = 0;

                changed = Quaternion.Euler(changedEuler);

                if (showDebugMessages)
                    Debug.Log("VRRotatable::HandAttachedUpdate: Changed Rotation = " + changed.eulerAngles);

                transform.rotation = changed * transform.rotation;
                handRotation = currentHandRotation;
            }
            else
            {
                Debug.LogWarning(
                    "VRRotatable: Rotatable was attached to hand with flag ParentToHand. This can cause unexpected behaviour.");
                transform.position = attachPosition;
            }

            if (hand.IsGrabEnding(gameObject))
            {
                hand.DetachObject(gameObject, restoreOriginalParent);
            }
        }
    }
}