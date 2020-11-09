using System;
using System.Collections.Generic;
using System.Linq;
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
        
        [Tooltip("Currently snapped object. If null, then no object is snapped per default.")] 
        public Interactable snappedObject = null;
        [SerializeField]
        [Tooltip("The object which will be highlighted once the user entered the snap zone.")]
        private GameObject highlightedObject = null;

        [Header("Restrictions")] 
        public List<string> allowedTags = new List<string>();

        [Header("Special Settings")] 
        public bool setScaleOnSnap = false;
        public bool resetScaleOnUnsnap = false;
        public Vector3 localScale = Vector3.one;
        public bool allowUnsnap = true;
        public bool cloneOnUnsnap = false;

        [Header("Events")] 
        public SnapZoneEvent onSnapZoneEnter;
        public SnapZoneEvent onSnapZoneExit;
        public SnapUnsnapEvent onSnap;
        public SnapUnsnapEvent onUnsnap;
        public SnapUnsnapEvent onCreatedClone;
        public SnapZoneEvent onStartHighlight;
        public SnapZoneEvent onEndHighlight;
        
        private Transform _snappedObjectParent = null;
        private readonly Dictionary<Rigidbody, RigidBodySettings> _snappedRigidBodies = new Dictionary<Rigidbody, RigidBodySettings>();
        private GameObject _highlighter = null;
        private Vector3 _previousScale = Vector3.zero;

        private List<Interactable> _objInZone = new List<Interactable>();

        public GameObject HighlightedObject => _highlighter;

        private void Awake()
        {
            SetupHighlightedObject(highlightedObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (snappedObject != null)
            {
                Snap(snappedObject.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var interactable = GetInteractable(other.gameObject);
            
            if (interactable && interactable != snappedObject && IsTagAllowed(interactable.tag))
            {
                interactable.onAttachedToHand += SnapZoneEntered;
                interactable.onDetachedFromHand += OnObjectDetachedFromHand;
                SnapZoneEntered(interactable.attachedToHand);

                _objInZone.Add(interactable);
            }
        }

        protected void SnapZoneEntered(Hand h)
        {
            Debug.Log("Snap Zone Entered - Hand: " + h);
            onSnapZoneEnter.Invoke(this);
            
            if (snappedObject == null && h != null)
            {
                Debug.Log("Should start highlight");
                onStartHighlight.Invoke(this);
            }
        }

        private bool IsTagAllowed(string tag)
        {
            return allowedTags.Count == 0 || allowedTags.Contains(tag);
        }
        
        private void OnTriggerExit(Collider other)
        {
            var interactable = GetInteractable(other.gameObject);
            if (interactable)
            {
                interactable.onAttachedToHand -= SnapZoneEntered;
                interactable.onDetachedFromHand -= OnObjectDetachedFromHand;
                
                onSnapZoneExit.Invoke(this);
                _objInZone.Remove(interactable);
                
                Debug.Log("On Trigger Exit");

                if (snappedObject == null && _objInZone.Any(obj => obj.attachedToHand != null))
                {
                    Debug.Log("Should start highlight");
                    onStartHighlight.Invoke(this);
                }
                else
                    onEndHighlight.Invoke(this);
            }
        }
        
        protected void OnObjectDetachedFromHand(Hand hand) {
            // we have an interactable object that was just detached from the user's hand -> snap it
            if (snappedObject) return; // we already snapped an object
            
            // adapt the events and add the snapped obj
            Snap(hand.currentAttachedObject);
            
            Debug.Log("Obj detached from hand -> highlight?");

            if (snappedObject == null && _objInZone.Any(obj => obj.attachedToHand != null)) {
                Debug.Log("Should start highlight");
                onStartHighlight.Invoke(this);
            }
            else
            {
                onEndHighlight.Invoke(this);
            }
        }


        protected void OnObjectAttachedToHand(Hand hand)
        {
            // our snapped object is now in the hand of the user
            // -> remove as snapped object and adapt events
            Unsnap();
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
        
        protected void Snap(GameObject newSnappedObject)
        {
            var interactable = GetInteractable(newSnappedObject);
            if (!interactable || (snappedObject && snappedObject != interactable)) return;
            
            snappedObject = interactable;
            _snappedObjectParent = newSnappedObject.transform.parent;
            newSnappedObject.transform.parent = transform;
            newSnappedObject.transform.localPosition = Vector3.zero;
            if(_highlighter)
                newSnappedObject.transform.localRotation = _highlighter.transform.localRotation;
            if (setScaleOnSnap)
            {
                _previousScale = newSnappedObject.transform.localScale;
                newSnappedObject.transform.localScale = localScale;
            }

            snappedObject.onAttachedToHand += OnObjectAttachedToHand;
            snappedObject.onAttachedToHand -= SnapZoneEntered;
            snappedObject.onDetachedFromHand -= OnObjectDetachedFromHand;
            snappedObject.onDetachedFromHand -= UpdateRigidbodyAfterHandDetached;
            
            interactable.GetComponents<Rigidbody>().ForEach(rb =>
            {
                if (!_snappedRigidBodies.ContainsKey(rb))
                    _snappedRigidBodies.Add(rb, new RigidBodySettings(rb.isKinematic, rb.useGravity));
                rb.isKinematic = true;
                rb.useGravity = false;
            });
            interactable.GetComponentsInChildren<Rigidbody>().ForEach(rb =>
            {
                if (!_snappedRigidBodies.ContainsKey(rb)) 
                    _snappedRigidBodies.Add(rb, new RigidBodySettings(rb.isKinematic, rb.useGravity));
                rb.isKinematic = true;
                rb.useGravity = false;
            });
            
            onSnap.Invoke(this, newSnappedObject);

            if (!allowUnsnap)
            {
                foreach (var comp in snappedObject.GetComponents<Collider>())
                    comp.enabled = false;
                foreach (var comp in snappedObject.GetComponentsInChildren<Collider>())
                    comp.enabled = false;
            }
        }
        
        protected void Unsnap()
        {
            if (!snappedObject) return;

            //adapt events
            snappedObject.onAttachedToHand -= OnObjectAttachedToHand;
            snappedObject.onAttachedToHand += SnapZoneEntered;
            snappedObject.onDetachedFromHand += OnObjectDetachedFromHand;
            snappedObject.onDetachedFromHand += UpdateRigidbodyAfterHandDetached;
            
            //reset scale and set to old parent
            if (setScaleOnSnap && resetScaleOnUnsnap)
            {
                snappedObject.transform.localScale = _previousScale;
            }
            snappedObject.transform.parent = _snappedObjectParent;
            var oldSnappedObj = snappedObject.gameObject;
            
            //reset old rigidbody behaviours
            foreach (var rbSetting in _snappedRigidBodies)
            {
                rbSetting.Key.isKinematic = rbSetting.Value.wasKinematic;
                rbSetting.Key.useGravity = rbSetting.Value.wasGravityUsed;
            }
            
            //reset objs to null
            _snappedObjectParent = null;
            snappedObject = null;
            
            onUnsnap.Invoke(this, oldSnappedObj);

            if (cloneOnUnsnap)
            {
                var clone = Instantiate(oldSnappedObj, transform);
                Snap(clone);
                onCreatedClone.Invoke(this, clone);
            }
        }
        
        protected void UpdateRigidbodyAfterHandDetached(Hand hand)
        {
            // set gravity of rigidbody accordingly
            if (hand.currentAttachedObjectInfo.HasValue)
            {
                var attachedInfo = hand.currentAttachedObjectInfo.Value;
                if (snappedObject && attachedInfo.attachedObject == snappedObject.gameObject)
                {
                    // we still need to update the rigid body once the object is really detached from the hand 
                    // and isn't our snapped object
                }
                else if (_snappedRigidBodies.ContainsKey(attachedInfo.attachedRigidbody))
                {
                    var rbSetting = _snappedRigidBodies[attachedInfo.attachedRigidbody];
                    attachedInfo.attachedRigidbody.isKinematic = rbSetting.wasKinematic;
                    attachedInfo.attachedRigidbody.useGravity = rbSetting.wasGravityUsed;

                    _snappedRigidBodies.Remove(attachedInfo.attachedRigidbody);
                }

                attachedInfo.interactable.onDetachedFromHand -= UpdateRigidbodyAfterHandDetached;
            }
        }

        protected void SetupHighlightedObject(GameObject obj)
        {
            if (!obj) return;

            _highlighter = GameObject.Instantiate(obj, transform);
            _highlighter.transform.localPosition = Vector3.zero;

            //destroy all unused components
            foreach (var comp in _highlighter.GetComponents<Collider>())
                comp.enabled = false;
            foreach (var comp in _highlighter.GetComponentsInChildren<Collider>())
                comp.enabled = false;
            foreach (var comp in _highlighter.GetComponents<Rigidbody>())
            {
                comp.detectCollisions = false;
                comp.isKinematic = true;
            }
            foreach (var comp in _highlighter.GetComponentsInChildren<Rigidbody>()) 
            {
                comp.detectCollisions = false;
                comp.isKinematic = true;
            }
            
            _highlighter.name = "HighlightContainer";
            _highlighter.SetActive(false);
        }
    }
}


