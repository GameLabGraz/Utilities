using System;
using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GameLabGraz.VRInteraction
{
    [RequireComponent( typeof( Interactable ) )]
    public class VR3DDrive : MonoBehaviour
    {
        public bool _showDebugMessages = false;
        
        public Transform startPosition;
        public Transform endXPosition;
        public Transform endYPosition;
        public Transform endZPosition;
        
        public bool fixXPosition = false;
        public bool fixYPosition = false;
        public bool fixZPosition = false;
        
        public bool repositionGameObject = true;
        public bool maintainMomentum = true;
        public float momentumDampenRate = 5.0f;
        
        [Tooltip( "If true, the drive will stay manipulating as long as the button is held down, if false, it will stop if the controller moves out of the collider" )]
        public bool hoverLock = false;
        [SerializeField]
        protected Hand.AttachmentFlags _attachmentFlags = Hand.AttachmentFlags.DetachFromOtherHand;

        public bool init = true;
        public Vector3 initialValue = Vector3.zero;

        public ValueChangeEventVector3 onValueChanged;

        public Vector3 CurrentMappedValue = Vector3.zero;
        protected Vector3 PrevMappedValue = Vector3.zero;
        protected Vector3 InitialMappingOffset = Vector3.zero;

        protected int numMappingChangeSamples = 5;
        protected Vector3[] MappingChangeSamples;
        protected Vector3 MappingChangeRate;
        protected int sampleCount = 0;
        
        protected Interactable _interactable;
        protected Hand _handHoverLocked;
        protected GrabTypes _grabbedWithType;

        protected virtual void Awake()
        {
            MappingChangeSamples = new Vector3[numMappingChangeSamples];
            _interactable = GetComponent<Interactable>();
        }

        protected virtual void Start()
        {
            InitialMappingOffset = CurrentMappedValue;
            if (repositionGameObject && init)
            {
                ForceToValue(initialValue);
            }
            else
            {
                CurrentMappedValue = CalculateMapping(transform);
            }
        }

        protected void OnDisable()
        {
            if (_handHoverLocked)
            {
                _handHoverLocked.HoverUnlock(_interactable);
                _handHoverLocked = null;
            }
        }

        protected virtual void HandHoverUpdate(Hand hand)
        {
            var startingGrabType = hand.GetGrabStarting();
            var isGrabEnding = hand.IsGrabbingWithType(_grabbedWithType) == false;

            if (_interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
            {
                _grabbedWithType = startingGrabType;
                var handTransform = hand.transform;
                InitialMappingOffset = CurrentMappedValue - CalculateMapping( handTransform );
                if (_showDebugMessages)
                {
                    Debug.Log("VRInteraction::VR3DDrive: Attached to Hand: CurrentMapping" + CurrentMappedValue);
                    Debug.Log("VRInteraction::VR3DDrive: Attached to Hand: " + InitialMappingOffset);
                }

                sampleCount = 0;
                MappingChangeRate = Vector3.zero;

                if (hoverLock)
                {
                    hand.HoverLock(_interactable);
                    _handHoverLocked = hand;
                }
                
                hand.AttachObject(gameObject, startingGrabType, _attachmentFlags);
            }
            else if (_grabbedWithType != GrabTypes.None && isGrabEnding)
            {
                //ungrab
                if (hoverLock)
                {
                    hand.HoverUnlock(_interactable);
                    _handHoverLocked = null;
                }

                _grabbedWithType = GrabTypes.None;
            }
        }

        protected virtual void HandAttachedUpdate(Hand hand)
        {
            var handTransform = hand.transform;
            UpdateMapping(handTransform);

            if (hand.IsGrabEnding(gameObject))
            {
                hand.DetachObject(gameObject);
            }
        }

        protected virtual void OnDetachedFromHand(Hand hand)
        {
            CalculateMappingChangeRate();
        }

        protected void CalculateMappingChangeRate()
        {
            //Compute the mapping change rate
            MappingChangeRate = Vector3.zero;
            int mappingSamplesCount = Mathf.Min( sampleCount, MappingChangeSamples.Length );
            if ( mappingSamplesCount != 0 )
            {
                for ( int i = 0; i < mappingSamplesCount; ++i )
                {
                    MappingChangeRate += MappingChangeSamples[i];
                }
                MappingChangeRate /= mappingSamplesCount;
            }
        }
        
        protected void UpdateMapping(Transform updateTransform)
        {
            if (_showDebugMessages)
                Debug.Log("VRInteraction::VR3DDrive: Update Mapping Transform: " + updateTransform.position);
            
            PrevMappedValue = CurrentMappedValue;
            
            CurrentMappedValue = InitialMappingOffset + CalculateMapping(updateTransform);
            CurrentMappedValue.x = Mathf.Clamp01(CurrentMappedValue.x);
            CurrentMappedValue.y = Mathf.Clamp01(CurrentMappedValue.y);
            CurrentMappedValue.z = Mathf.Clamp01(CurrentMappedValue.z);
            
            if(_showDebugMessages)
                Debug.Log("VRInteraction::VR3DDrive: Update Mapping: " + CurrentMappedValue);

            MappingChangeSamples[sampleCount % MappingChangeSamples.Length] = ( 1.0f / Time.deltaTime ) * ( CurrentMappedValue - PrevMappedValue );
            sampleCount++;
            
            if (repositionGameObject)
            {
                ForceToValue(CurrentMappedValue);
            }
            
            onValueChanged.Invoke(CurrentMappedValue);
        }
        
        private float CalculateLinearMapping( Transform updateTransform, Transform endPosition )
        {
            Vector3 direction = endPosition.position - startPosition.position;
            float length = direction.magnitude;
            direction.Normalize();

            Vector3 displacement = updateTransform.position - startPosition.position;

            return Vector3.Dot( displacement, direction ) / length;
        }
        
        protected Vector3 CalculateMapping(Transform updateTransform)
        {
            var value = Vector3.zero;
            
            if (!fixXPosition) 
                value.x = CalculateLinearMapping(updateTransform, endXPosition);

            if (!fixYPosition)
                value.y = CalculateLinearMapping(updateTransform, endYPosition);

            if (!fixZPosition)
                value.z = CalculateLinearMapping(updateTransform, endZPosition);

            if(_showDebugMessages)
                Debug.Log("VRInteraction::VR3DDrive: Calc_Mapping: " + value);
            return value;
        }

        public void ForceToValue(Vector3 newValue)
        {
            CurrentMappedValue = newValue;

            if (repositionGameObject)
            {
                var pos = startPosition.position;
                if (!fixXPosition)
                {
                    var dirX = endXPosition.position - startPosition.position;
                    pos += CurrentMappedValue.x * dirX;
                }

                if (!fixYPosition)
                {
                    var dirY = endYPosition.position - startPosition.position;
                    pos += CurrentMappedValue.y * dirY;
                }

                if (!fixZPosition)
                {
                    var dirZ = endZPosition.position - startPosition.position;
                    pos += CurrentMappedValue.z * dirZ;
                }
                
                if(_showDebugMessages)
                    Debug.Log("VRInteraction::VR3DDrive: Reposition Object: " + CurrentMappedValue);
                
                transform.position = pos;
            }
        }

        protected void Update()
        {
            if ( maintainMomentum && MappingChangeRate != Vector3.zero )
            {
                //Dampen the mapping change rate and apply it to the mapping
                MappingChangeRate = Vector3.Lerp(MappingChangeRate, Vector3.zero, momentumDampenRate * Time.deltaTime);
                CurrentMappedValue.x = Mathf.Clamp01(CurrentMappedValue.x + (MappingChangeRate.x * Time.deltaTime));
                CurrentMappedValue.y = Mathf.Clamp01(CurrentMappedValue.y + (MappingChangeRate.y * Time.deltaTime));
                CurrentMappedValue.z = Mathf.Clamp01(CurrentMappedValue.z + (MappingChangeRate.z * Time.deltaTime));
                
                if ( repositionGameObject )
                {
                    ForceToValue(CurrentMappedValue);
                }
            }
        }
    }
}