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

        public bool _useShaderColorVariable;
        public string _shaderColorVariableName;
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
                    if (!info._useShaderColorVariable || info._shaderColorVariableName == "")
                    {
                        info._renderer.materials[info._materialIndex].color = newColor;
                    }
                    else
                    {
                        info._renderer.materials[info._materialIndex].SetColor(info._shaderColorVariableName, newColor);
                    }
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
                if(!info._useShaderColorVariable || info._shaderColorVariableName == "")
                    return info._renderer.materials[info._materialIndex].color;
                return info._renderer.materials[info._materialIndex].GetColor(info._shaderColorVariableName);
            }
            
            return Color.black;
        }
    }
}