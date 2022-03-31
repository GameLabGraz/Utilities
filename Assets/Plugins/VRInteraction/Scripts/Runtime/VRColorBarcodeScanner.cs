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
        [SerializeField] protected string clearBarcodeText = "Scan Barcode";

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
            barcodeText.text = clearBarcodeText;
        }

        protected void OnColorChanged(Color newPickedColor)
        {
            if (!barcodeScanner.HasScannedBarcode()) return;
            
            barcodeScanner.GetLastScannedBarcode().ChangeColor(newPickedColor);
        }

        protected void OnNewBarcodeScanned(Barcode newScannedObject)
        {
            barcodeText.text = newScannedObject.GetContentString();
            colorPicker.ForceToColor(newScannedObject.GetCurrentColor());
        }

        protected void OnAttachedToHand()
        {
            barcodeBeam.SetActive(true);
        }

        protected void OnDetachedFromHand()
        {
            barcodeBeam.SetActive(false);
            barcodeScanner.scannedBarcode = null;
            barcodeText.text = clearBarcodeText;
        }
    }
}