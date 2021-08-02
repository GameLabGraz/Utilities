using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GEAR.VRInteraction
{
    public class VRLoadingObject : MonoBehaviour
    {
        protected Renderer Render;
        protected Shader DissolveShader;
        protected static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");

        public bool useReverse = false;

        void Start()
        {
            DissolveShader = Shader.Find("DissolverShader/DissolveShader");
            Render = GetComponent<Renderer>();
        }

        public void UpdateLoadingEffect(float percentageLoaded)
        {
            var newPercentage = Mathf.Clamp(useReverse ? 1f - percentageLoaded : percentageLoaded, 0f, 1f);
            foreach (var renderMaterial in Render.materials)
            {
                if (renderMaterial.shader == DissolveShader)
                {
                    renderMaterial.SetFloat(DissolveAmount, newPercentage);
                }
            }
        }
    }
}