using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GEAR.VRInteraction
{
    public class VRSnapDropZone : MonoBehaviour
    {
        private struct RigidBodySettings
        {
            public bool wasKinematic;
            public bool wasGravityUsed;

            public RigidBodySettings(bool kinematic, bool gravity)
            {
                wasKinematic = kinematic;
                wasGravityUsed = gravity;
            }
        }
        
        [Tooltip("The highlighted object.")] 
        public Interactable snappedObject = null;
        private Transform _snappedObjectParent = null;
        private Dictionary<Rigidbody, RigidBodySettings> _snappedRigidBodies = new Dictionary<Rigidbody, RigidBodySettings>(); 
        
        // Start is called before the first frame update
        void Start()
        {
            if (snappedObject != null)
            {
                AdaptGameObjectOnSnap(snappedObject.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var interactable = GetInteractable(other.gameObject);
            if (interactable)
            {
                Debug.Log("SnapZone: An interactable entered the snap zone!");
                interactable.onDetachedFromHand += OnObjectDetachedFromHand;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var interactable = GetInteractable(other.gameObject);
            if (interactable)
            {
                Debug.Log("SnapZone: An interactable exited the snap zone!");
                interactable.onDetachedFromHand -= OnObjectDetachedFromHand;
            }
        }
        
        protected void OnObjectDetachedFromHand(Hand hand) {
            Debug.Log("SnapZone: OnObjectDetachedFromHand");
            
            // we have an interactable object that was just detached from the user's hand -> snap it
            if (snappedObject) return; // we already snapped an object
            
            var interactable = GetInteractable(hand.currentAttachedObject);
            if (!interactable) return;
            
            // 1. adapt the events and add the snapped obj
            AdaptGameObjectOnSnap(hand.currentAttachedObject);
            
            // TODO: 3. Adapt highlighting, position of the snapped object etc...

        }


        protected void OnObjectAttachedToHand(Hand hand)
        {
            Debug.Log("SnapZone: OnObjectAttachedToHand");
            // our snapped object is now in the hand of the user
            var interactable = GetInteractable(hand.currentAttachedObject);
            if (!interactable) return;
            
            // 2. remove as snapped object and adapt events
            Debug.Assert(interactable == snappedObject);
            AdaptGameObjectOnUnsnap();
            
            // TODO: 3. Highlight snapping object etc.

        }

        protected static Interactable GetInteractable(GameObject obj)
        {
            var interactable = obj.GetComponent<Interactable>();

            if (!interactable)
                interactable = obj.GetComponentInChildren<Interactable>();

            var parent = obj.transform.parent;
            while (!interactable && parent)
            {
                interactable = parent.GetComponent<Interactable>();
                parent = parent.parent;
            }

            return interactable;
        }

        protected void AdaptGameObjectOnSnap(GameObject newSnappedObject)
        {
            var interactable = GetInteractable(newSnappedObject);
            if (!interactable || (snappedObject && snappedObject != interactable)) return;

            snappedObject = interactable;
            _snappedObjectParent = newSnappedObject.transform;
            newSnappedObject.transform.parent = transform;
            newSnappedObject.transform.localPosition = Vector3.zero;
            
            snappedObject.onAttachedToHand += OnObjectAttachedToHand;
            snappedObject.onDetachedFromHand -= OnObjectDetachedFromHand;
            snappedObject.onDetachedFromHand -= OnUpdateRigidbodyAfterUnsnap;
            
            interactable.GetComponents<Rigidbody>().ForEach(rb =>
            {
                if (_snappedRigidBodies.ContainsKey(rb)) return;
                _snappedRigidBodies.Add(rb, new RigidBodySettings(rb.isKinematic, rb.useGravity));
                rb.isKinematic = true;
                rb.useGravity = false;
            });
            interactable.GetComponentsInChildren<Rigidbody>().ForEach(rb =>
            {
                if (_snappedRigidBodies.ContainsKey(rb)) return;
                _snappedRigidBodies.Add(rb, new RigidBodySettings(rb.isKinematic, rb.useGravity));
                rb.isKinematic = true;
                rb.useGravity = false;
            });
        }
        
        protected void OnUpdateRigidbodyAfterUnsnap(Hand hand)
        {
            // set gravity of rigidbody accordingly
             if (hand.currentAttachedObjectInfo.HasValue)
             {
                 var attachedInfo = hand.currentAttachedObjectInfo.Value;
                 if (_snappedRigidBodies.ContainsKey(attachedInfo.attachedRigidbody))
                 {
                     var rbSetting = _snappedRigidBodies[attachedInfo.attachedRigidbody];
                     attachedInfo.attachedRigidbody.isKinematic = rbSetting.wasKinematic;
                     attachedInfo.attachedRigidbody.useGravity = rbSetting.wasGravityUsed;

                     _snappedRigidBodies.Remove(attachedInfo.attachedRigidbody);
                 }

                 attachedInfo.interactable.onDetachedFromHand -= OnUpdateRigidbodyAfterUnsnap;
             }
        }

        protected void AdaptGameObjectOnUnsnap()
        {
            if (!snappedObject) return;

            //adapt events
            snappedObject.onAttachedToHand -= OnObjectAttachedToHand;
            snappedObject.onDetachedFromHand += OnObjectDetachedFromHand;
            snappedObject.onDetachedFromHand += OnUpdateRigidbodyAfterUnsnap;
            
            //set to old parent
            snappedObject.transform.parent = _snappedObjectParent;
            
            //reset old rigidbody behaviours
            foreach (var rbSetting in _snappedRigidBodies)
            {
                rbSetting.Key.isKinematic = rbSetting.Value.wasKinematic;
                rbSetting.Key.useGravity = rbSetting.Value.wasGravityUsed;
            }
            
            //reset objs to null
            _snappedObjectParent = null;
            snappedObject = null;
        }
    }
}


