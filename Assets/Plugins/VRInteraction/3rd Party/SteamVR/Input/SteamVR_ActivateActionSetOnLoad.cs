//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using UnityEngine;
using System.Collections;

namespace Valve.VR
{
    /// <summary>
    /// Automatically activates an action set on Start() and deactivates the set on OnDestroy(). Optionally deactivating all other sets as well.
    /// </summary>
    public class SteamVR_ActivateActionSetOnLoad : MonoBehaviour
    {
        public SteamVR_ActionSet actionSet = SteamVR_Input.GetActionSet("default");
        public SteamVR_ActionSet vrInteractionSet = SteamVR_Input.GetActionSet("VRInteractionPlugin");

        public SteamVR_Input_Sources forSources = SteamVR_Input_Sources.Any;

        public bool disableAllOtherActionSets = false;

        public bool activateOnStart = true;
        public bool deactivateOnDestroy = true;

        public int initialPriority = 0;

        private void Start()
        {
            if(activateOnStart)
            {
                if (actionSet != null)
                {
                    actionSet.Activate(forSources, initialPriority, disableAllOtherActionSets);
                }
                if (vrInteractionSet != null)
                {
                    vrInteractionSet.Activate();
                }
            }
        }

        private void OnDestroy()
        {
            if (deactivateOnDestroy)
            {
                if (actionSet != null)
                {
                    actionSet.Deactivate(forSources);
                }
                if (vrInteractionSet != null)
                {
                    vrInteractionSet.Deactivate();
                }
            }
        }
    }
}