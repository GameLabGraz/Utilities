using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLabGraz.VRInteraction
{
    [System.Serializable]
    public struct RendererInfo
    {
        public Renderer _renderer;
        public int _materialIndex;
    }
    
    
    public class Barcode : MonoBehaviour
    {
        public string barcodeContent = "display name";

        [Header("Color Barcode")] 
        public bool allowColorChange = true;
        public List<RendererInfo> colorChangingObjects;
        public int displayColorRendererIndex = 0;
        
        public ValueChangeEventColor onColorChanged;
        
        public void ChangeColor(Color newColor)
        {
            if (!allowColorChange)
                return;
            
            foreach (var info in colorChangingObjects)
            {
                if (info._renderer != null && info._materialIndex >= 0 && info._materialIndex < info._renderer.materials.Length)
                {
                    info._renderer.materials[info._materialIndex].color = newColor;
                }
            }

            onColorChanged.Invoke(newColor);
        }

        public Color GetCurrentColor()
        {
            if(displayColorRendererIndex < 0 || displayColorRendererIndex >= colorChangingObjects.Count)
                return Color.black;

            var info = colorChangingObjects[displayColorRendererIndex];
            if (info._renderer != null && info._materialIndex >= 0 && info._materialIndex < info._renderer.materials.Length)
            {
                return info._renderer.materials[info._materialIndex].color;
            }

            return Color.black;
        }
    }
}