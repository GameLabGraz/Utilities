using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLabGraz.VRInteraction
{
    public class VRHighlightInfo : MonoBehaviour
    {
        [Tooltip("The material index if only one of the material should be highlighted")]
        public uint MaterialIndex = 0;
    }
}