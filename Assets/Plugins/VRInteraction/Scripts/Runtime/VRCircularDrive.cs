using System;
using PrivateAccess;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GameLabGraz.VRInteraction
{
    public class VRCircularDrive : CircularDrive
    {
        [Header("VR Interaction Plugin")] 
        public bool useStepsForValues = true;
        public bool useStepsForRotation = true;
        public bool useStepsAfterGrabEnded = true;
        [Range(0f, 10f)] public float stepSize = 1f;
        public bool useAsInteger = false;
        public bool useAsBool = false;

        public float initialValue = 5f;
        public float minimum = 0f;
        public float maximum = 10f;
        public int boolIsTrueWhenValue = 1;
        public bool forceToInitialValueAfterGrabEnded = false;

        public ValueChangeEventFloat onValueChanged;
        public ValueChangeEventInt onValueChangedInt;
        public ValueChangeEventBool onValueChangedBool;

        protected float _currentValue;
        protected bool _currentValueBool;
        protected float _valueRange;
        
        protected Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.DetachFromOtherHand;
        
        //#TODO Update SteamVR: remove code start (member variables are protected)
        protected Vector3 WorldPlaneNormal
        {
            get => this.GetBaseFieldValue<Vector3>("worldPlaneNormal");
            set => this.SetBaseFieldValue("worldPlaneNormal", value);
        }
        protected Vector3 LocalPlaneNormal
        {
            get => this.GetBaseFieldValue<Vector3>("localPlaneNormal");
            set => this.SetBaseFieldValue("localPlaneNormal", value);
        }
        protected Quaternion start
        {
            get => this.GetBaseFieldValue<Quaternion>("start");
            set => this.SetBaseFieldValue("start", value);
        }
        protected GrabTypes grabbedWithType
        {
            get => this.GetBaseFieldValue<GrabTypes>("grabbedWithType");
            set => this.SetBaseFieldValue("grabbedWithType", value);
        }
        protected Interactable interactable
        {
            get => this.GetBaseFieldValue<Interactable>("interactable");
            set => this.SetBaseFieldValue("interactable", value);
        }
        //#TODO Update SteamVR: remove code end (member variables are protected)
                
        protected void Start()
        {
            if ( childCollider == null )
                childCollider = GetComponentInChildren<Collider>();

            if ( linearMapping == null )
                linearMapping = GetComponent<LinearMapping>();

            if ( linearMapping == null )
                linearMapping = gameObject.AddComponent<LinearMapping>();

            WorldPlaneNormal = new Vector3(0.0f, 0.0f, 0.0f) {[(int) axisOfRotation] = 1.0f};
            LocalPlaneNormal = WorldPlaneNormal;

            if ( transform.parent )
                WorldPlaneNormal = transform.parent.localToWorldMatrix.MultiplyVector( WorldPlaneNormal ).normalized;

            if ( limited )
            {
                start = Quaternion.identity;
                outAngle = transform.localEulerAngles[(int)axisOfRotation];

                if ( forceStart )
                {
                    outAngle = Mathf.Clamp( startAngle, minAngle, maxAngle );
                }
            }
            else
            {
                start = Quaternion.AngleAxis( transform.localEulerAngles[(int)axisOfRotation], LocalPlaneNormal );
                outAngle = 0.0f;
            }

            if ( debugText )
            {
                debugText.alignment = TextAlignment.Left;
                debugText.anchor = TextAnchor.UpperLeft;
            }

            _valueRange = maximum - minimum;
            ForceToValue(initialValue);
        }
        protected void UpdateGameObject()
        {
            if ( rotateGameObject )
            {
                var realAngle = outAngle;
                if (useStepsForRotation)
                {
                    realAngle = minAngle + (maxAngle - minAngle) * linearMapping.value;
                }

                transform.localRotation = start * Quaternion.AngleAxis( realAngle, LocalPlaneNormal );
            }
        }
        
        protected bool lastGrabEnded = false;
        
        protected void HandHoverUpdate( Hand hand )
        {
            GrabTypes startingGrabType = hand.GetGrabStarting();
            bool isGrabEnding = hand.IsGrabbingWithType(grabbedWithType) == false;

            var vrInteractable = interactable as VRInteractable;
            if (vrInteractable != null && !vrInteractable.IsInteractable())
            {
                return;    
            }
            
            //#TODO Update SteamVR: call HandHoverUpdate once it is protected
            this.CallBaseMethod("HandHoverUpdate", new object[]{hand});
            // base.HandHoverUpdate(hand);
            
            if (lastGrabEnded != isGrabEnding && isGrabEnding && useStepsAfterGrabEnded && rotateGameObject)
            {
                var realAngle = minAngle + (maxAngle - minAngle) * linearMapping.value;
                transform.localRotation = start * Quaternion.AngleAxis( realAngle, LocalPlaneNormal );
            }
            lastGrabEnded = isGrabEnding;
            
            if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
            {
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
            }
        }
        
        protected virtual void HandAttachedUpdate(Hand hand)
        {
            //#TODO Update SteamVR: call ComputeAngle once it is protected
            this.CallBaseMethod("ComputeAngle", new object[]{hand});
            // ComputeAngle(hand)
            UpdateAll();
            
            if (hand.IsGrabEnding(gameObject))
            {
                hand.DetachObject(gameObject);
            
                // Debug.Log("hand Detached");
                if(forceToInitialValueAfterGrabEnded){
                    if ( limited )
                    {
                        outAngle = transform.localEulerAngles[(int)axisOfRotation];
                        if ( forceStart )
                        {
                            outAngle = Mathf.Clamp( startAngle, minAngle, maxAngle );
                        }
                    }
                    else
                    {
                        outAngle = 0.0f;
                    }
            
                    Debug.Log("ForceToInitialValue");
                    ForceToValue(initialValue);
                }
            }
        }
        
        protected void UpdateLinearMapping()
        {
            if (!linearMapping)
            {
                linearMapping = GetComponent<LinearMapping>();
            }
            
            if ( limited )
            {
                // Map it to a [0, 1] value
                linearMapping.value = ( outAngle - minAngle ) / ( maxAngle - minAngle );
            }
            else
            {
                // Normalize to [0, 1] based on 360 degree windings
                float flTmp = outAngle / 360.0f;
                linearMapping.value = flTmp - Mathf.Floor( flTmp );
            }
            
            _currentValue = Mathf.Clamp(linearMapping.value * _valueRange + minimum, minimum, maximum);
            
            if (useAsInteger)
                _currentValue = Mathf.RoundToInt(_currentValue);
            if (useAsBool)
                _currentValueBool = Mathf.RoundToInt(_currentValue) == boolIsTrueWhenValue;
            
            if (useStepsForValues)
            {
                var tmp = _currentValue % stepSize;
                if (tmp > stepSize / 2)
                    _currentValue = _currentValue - tmp + stepSize;
                else
                    _currentValue -= tmp;
				
                if (useAsInteger)
                    _currentValue = Mathf.RoundToInt(_currentValue);

                linearMapping.value = (_currentValue - minimum) / _valueRange;
            }
            
            onValueChanged.Invoke(_currentValue);
            if(useAsInteger)
                onValueChangedInt.Invoke(Mathf.RoundToInt(_currentValue));
            if(useAsBool)
                onValueChangedBool.Invoke(_currentValueBool);
		
            //#TODO Update SteamVR: call UpdateDebugText once it is protected
            this.CallBaseMethod("UpdateDebugText", new object[0]);
            // UpdateDebugText();
        }

        public void ForceToAngle(float angle)
        {
            outAngle = angle;
            UpdateAll();
        }
        
        public void ForceToValue(float value)
        {
            _currentValue = Mathf.Clamp(value, minimum, maximum);
            linearMapping.value = (_currentValue - minimum) / _valueRange;

            // Debug.Log("before: " + _currentValue + " - " + linearMapping.value);

            onValueChanged.Invoke(_currentValue);
            if(useAsInteger)
                onValueChangedInt.Invoke(Mathf.RoundToInt(_currentValue));
            _currentValueBool = Mathf.RoundToInt(_currentValue) == boolIsTrueWhenValue;
            if(useAsBool)
                onValueChangedBool.Invoke(_currentValueBool);
            
            UpdateAll();
        }
        
        //#TODO Update SteamVR: remove this once all functions are protected/virtual
        protected void UpdateAll()
        {
            UpdateLinearMapping();
            UpdateGameObject();
            
            //#TODO Update SteamVR: call UpdateDebugText once it is protected
            this.CallBaseMethod("UpdateDebugText", new object[0]);
            // UpdateDebugText();
        }
    }
}
