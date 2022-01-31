using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GameLabGraz.VRInteraction
{
    public class VRColorBarcodeScanner : MonoBehaviour
    {
        [Header("Needed Objects")] [SerializeField]
        protected VRColorPicker colorPicker;

        [SerializeField] protected BarcodeScanner barcodeScanner;
        [SerializeField] protected GameObject barcodeBeam;
        [SerializeField] protected TextMeshPro barcodeText;

        // Start is called before the first frame update
        void Start()
        {
            colorPicker.onColorChanged.AddListener(OnColorChanged);
            barcodeScanner.onBarcodeScanned.AddListener(OnNewBarcodeScanned);

            var hoverEvents = GetComponent<InteractableHoverEvents>();
            if (hoverEvents)
            {
                hoverEvents.onAttachedToHand.AddListener(OnAttachedToHand);
                hoverEvents.onDetachedFromHand.AddListener(OnDetachedFromHand);
            }

            barcodeBeam.SetActive(false);
        }

        protected void OnColorChanged(Color newPickedColor)
        {
            if (!barcodeScanner.HasScannedBarcode()) return;
        }

        protected void OnNewBarcodeScanned(Barcode newScannedObject)
        {
            barcodeText.text = newScannedObject.barcodeContent;
        }

        protected void OnAttachedToHand()
        {
            barcodeBeam.SetActive(true);
        }

        protected void OnDetachedFromHand()
        {
            barcodeBeam.SetActive(false);
            barcodeScanner.scannedBarcode = null;
            barcodeText.text = "";
        }
    }
}