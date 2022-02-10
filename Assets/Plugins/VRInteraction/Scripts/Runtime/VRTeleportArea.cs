using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GameLabGraz.VRInteraction
{
	//same as the Teleport Area with from steam without the area highlighting
    public class VRTeleportArea : TeleportArea
    {
	    //-------------------------------------------------
		public override void Highlight( bool highlight )
		{
			//Do nothing as we do not want any highlighting
		}
		
		//-------------------------------------------------
		public override void UpdateVisuals()
		{
			//Do nothing as we do not want any highlighting
		}
		
		public override void SetAlpha( float tintAlpha, float alphaPercent )
		{
			//Do nothing as we do not want any highlighting
		}
		
    }
}
