using System;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace GameLabGraz.VRInteraction
{
    [RequireComponent(typeof(Interactable))]
    public class BarcodeScannerController : MonoBehaviour
    {
        public SteamVR_Action_Boolean actionScan =
            SteamVR_Input.GetAction<SteamVR_Action_Boolean>("VRInteractionPlugin", "ColorScanner_ScanButton");

        public GameObject barcodeScanner = null;

        public bool showHint = false;
        public float hintDuration = 10;
        
        protected VRInteractable interactable;
        protected Hand attachedHand;
        
        protected bool _lastState = false;

        protected float _attachedTime = 0;
        protected bool _hintIsShown = false;

        private void Start()
        {
            interactable = GetComponent<VRInteractable>();
            if (barcodeScanner)
                barcodeScanner.SetActive(false);
            else
                Debug.LogWarning("BarcodeScannerController: No barcode gameObject set.");
        }

        public void OnPickUp()
        {
            Debug.Log(interactable.IsAttachedToHand());

            attachedHand = interactable.attachedToHand;
            var hand = attachedHand.handType;
            var currentState = actionScan.GetState(hand);

            if (showHint && _attachedTime <= hintDuration && !_hintIsShown)
            {
                ControllerButtonHints.ShowButtonHint(attachedHand, actionScan);
                _hintIsShown = true;
            }
            else if (_hintIsShown && _attachedTime > hintDuration)
            {
                ControllerButtonHints.HideButtonHint(attachedHand, actionScan);
                _hintIsShown = false;
            }

            if (currentState != _lastState)
            {
                _lastState = currentState;
                if (barcodeScanner)
                    barcodeScanner.SetActive(_lastState);
            }

            _attachedTime += Time.deltaTime;
        }
        
        public void OnRelease()
        {
            if (_hintIsShown)
            {
                ControllerButtonHints.HideButtonHint(attachedHand, actionScan);
                _hintIsShown = false;
            }
            if (_lastState)
            {
                _lastState = false;
                if (barcodeScanner)
                    barcodeScanner.SetActive(_lastState);
            }
        }
    }
}
