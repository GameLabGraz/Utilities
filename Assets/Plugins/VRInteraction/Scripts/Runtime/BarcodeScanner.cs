using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameLabGraz.VRInteraction
{
    [System.Serializable]
    public class BarcodeScanEvent : UnityEvent<Barcode>
    {
    }

    public class BarcodeScanner : MonoBehaviour
    {
        public Barcode scannedBarcode = null;

        public BarcodeScanEvent onBarcodeScanned;

        protected void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.GetComponent<Barcode>()) return;

            scannedBarcode = other.gameObject.GetComponent<Barcode>();
            onBarcodeScanned.Invoke(scannedBarcode);
        }

        public bool HasScannedBarcode()
        {
            return scannedBarcode != null;
        }

        public Barcode GetLastScannedBarcode()
        {
            return scannedBarcode;
        }

    }
}