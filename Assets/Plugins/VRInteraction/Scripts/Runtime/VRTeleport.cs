using UnityEngine;
using PrivateAccess;
using Valve.VR.InteractionSystem;

namespace GameLabGraz.VRInteraction
{
	public class VRTeleport : Teleport
	{
		protected TeleportMarkerBase[] TeleportMarkers
		{
			get => this.GetBaseFieldValue<TeleportMarkerBase[]>("teleportMarkers");
			set => this.SetBaseFieldValue("teleportMarkers", value);
		}
		

		public void ReinitTeleports()
		{
			TeleportMarkers = GameObject.FindObjectsOfType<TeleportMarkerBase>();
			HideTeleportPointer();
		}
	}
}