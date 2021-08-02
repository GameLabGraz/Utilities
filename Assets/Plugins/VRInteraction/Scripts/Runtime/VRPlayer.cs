using System;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace GEAR.VRInteraction
{
    public class VRPlayer : Player
    {
        [SerializeField]
        protected GameObject snapTurnGameObject;

        [Header("Maroon VR Specific")] 
        [Tooltip("The user can turn the player via the controllers.")]
        public bool allowSnapTurns = true;

        [Header("Skeleton Settings")] 
        public bool showController = true;
        public bool animateWithController = false;

        // Start is called before the first frame update
        protected void Start()
        {
            if (snapTurnGameObject)
            {
                snapTurnGameObject.SetActive(allowSnapTurns);
            }

            foreach (var hand in instance.hands)
            {
                if (hand == null) continue;
                
                if (showController) hand.ShowController(true);
                else hand.HideController(true);
                
                hand.SetSkeletonRangeOfMotion(animateWithController? 
                    Valve.VR.EVRSkeletalMotionRange.WithController :
                    Valve.VR.EVRSkeletalMotionRange.WithoutController);
            }

            var activeRig = GetActiveRig();
            if (activeRig)
            {
                var activePosTrans = activeRig.GetComponentInChildren<Camera>();

                if (activePosTrans)
                {
                    //assuming all scales are 1:
                    var newPosition = transform.position;
                    var position = activePosTrans.transform.position;

                    newPosition.x -= position.x;
                    // do not change y position -> player height
                    newPosition.z -= position.z;
                    transform.position = newPosition;
                }
            }
        }

        protected GameObject GetActiveRig()
        {
            if (rigSteamVR.activeSelf)
                return rigSteamVR;
            if (rig2DFallback.activeSelf)
                return rig2DFallback;

            return null;
        }
    }
}
