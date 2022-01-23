using System;
using System.Collections;
using System.Collections.Generic;
using GEAR.VRInteraction;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ValueChangeEventColor: UnityEvent<Color>
{
}

public class VRColorPicker : MonoBehaviour
{
    [SerializeField] 
    protected VRCircularDrive ColorCircle;
    [SerializeField] 
    protected VR3DDrive ColorRect;

    [SerializeField] protected Renderer ColorRectRenderer;
    [SerializeField] protected int ColorRectRenderMaterialIndex = 0;
    [SerializeField] protected Renderer OutputRenderer;
    [SerializeField] protected int OutputRenderMaterialIndex;
    
    [SerializeField] protected Color InitialColor = Color.red;

    protected Color CurrentRGB = new Color(1, 0, 0);
    protected Vector3 CurrentHSV = new Vector3(0, 100, 0);
    
    public ValueChangeEventColor onColorChanged;
    private static readonly int ColorAngle = Shader.PropertyToID("_ColorAngle");

    // Start is called before the first frame update
    protected void Start()
    {
        ChangeColor(CurrentHSV, false);
        ColorCircle.onValueChanged.AddListener(arg0 => OnColorCircleChanged());
        ColorRect.onValueChanged.AddListener(OnColorRectChanged);

        ForceToColor(InitialColor);
    }

    protected void ChangeColor(Vector3 hsv, bool notify)
    {
        CurrentHSV = hsv;
        CurrentRGB = HSVToRGB(CurrentHSV.x, CurrentHSV.y / 100f, CurrentHSV.z / 100f);

        if (ColorRectRenderer && ColorRectRenderMaterialIndex < ColorRectRenderer.materials.Length)
        {
            ColorRectRenderer.materials[ColorRectRenderMaterialIndex].SetFloat(ColorAngle, (int)CurrentHSV.x);
        }

        if (OutputRenderer && OutputRenderMaterialIndex < OutputRenderer.materials.Length)
        {
            OutputRenderer.materials[OutputRenderMaterialIndex].color = CurrentRGB;
        }
        
        if(notify)
            onColorChanged.Invoke(CurrentRGB);
    }
    
    protected void OnColorCircleChanged()
    {
        CurrentHSV.x = Mathf.Clamp((1f - ColorCircle.linearMapping.value) * 360f, 0f, 360f); //hue
        ChangeColor(CurrentHSV, true);
    }

    protected void OnColorRectChanged(Vector3 newColorCoordinates)
    {
        //ignore Y Position
        CurrentHSV.y = Mathf.Clamp(newColorCoordinates.x * 100f, 0f, 100f); //saturation
        CurrentHSV.z = Mathf.Clamp(100f - (newColorCoordinates.z * 100f), 0f, 100f); //lightness
        ChangeColor(CurrentHSV, true);
    }
    
    public Color HSVToRGB(float h, float s, float v)
    { //hue, saturation, value (lightness)
        var c = s * v;
        var x = c * (1 - Mathf.Abs(((h/60) % 2) - 1));
        var m = v - c;
        Vector3 rgb1;

        if(0 <= h && h < 60)
            rgb1 = new Vector3(c, x, 0);
        else if(60 <= h && h < 120)
            rgb1 = new Vector3(x, c, 0);
        else if(120 <= h && h < 180)
            rgb1 = new Vector3(0, c, x);
        else if(180 <= h && h < 240)
            rgb1 = new Vector3(0, x, c);
        else if(240 <= h && h < 300)
            rgb1 = new Vector3(x, 0, c);
        else
            rgb1 = new Vector3(c, 0, x);
            
        return new Color(rgb1.x + m, rgb1.y + m, rgb1.z + m, 1f);
    }

    public Vector3 RGBToHSV(Color rgb)
    {
        var ret = Vector3.zero;
        Color.RGBToHSV(rgb, out ret.x, out ret.y, out ret.z);
        
        //multiply so that hue is in range (0, 360) and saturation and lightness in range (0, 100)
        ret.x *= 360f;
        ret.y *= 100f;
        ret.z *= 100f;
        return ret;
    }

    public void ForceToColor(Color rgbColor)
    {
        CurrentHSV = RGBToHSV(rgbColor);
        Debug.Log("Force to Color HSV: " + CurrentHSV);

        ColorCircle.ForceToAngle(360f - CurrentHSV.x);
        ColorRect.ForceToValue(new Vector3(CurrentHSV.y / 100f, 0f, 1f - CurrentHSV.z / 100f));
        
        ChangeColor(CurrentHSV, false);
    }

}
