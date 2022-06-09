using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PrivateAccess;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace GameLabGraz.VRInteraction
{
    public class VRPlayer : Player
    {
        [Header("VR Interaction Plugin")]
        [SerializeField]
        protected GameObject snapTurnGameObject;
        [Tooltip("The user can turn the player via the controllers.")]
        public bool allowSnapTurns = true;
        [Tooltip("Forces the transform to stay at the start coordinates.")]
        public bool forcePosition = true;

        [Header("Skeleton Settings")] 
        public bool showController = true;
        public bool animateWithController = false;

        protected Vector3 _position = Vector3.zero;
        private bool initialUpdate = true;

        protected void Awake()
        {
            _position = transform.position;
            
            this.CallBaseMethod("Awake", new object[0]);
        }
        
        protected override void Update()
        {
            if (initialUpdate)
            {
                Init();
                initialUpdate = false;
            }
            
            base.Update();
        }

        // Start is called before the first frame update
        protected void Init()
        {
            if (SteamVR.instance != null)
            {
                if (snapTurnGameObject)
                {
                    snapTurnGameObject.SetActive(allowSnapTurns);
                }

                foreach (var hand in instance.hands)
                {
                    if (!hand) continue;

                    if (showController) hand.ShowController(true);
                    else hand.HideController(true);

                    // hand.SetSkeletonRangeOfMotion(animateWithController? 
                    //     Valve.VR.EVRSkeletalMotionRange.WithController :
                    //     Valve.VR.EVRSkeletalMotionRange.WithoutController);
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

            if (forcePosition)
            {
                _position.y = transform.position.y;
                transform.position = _position;
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
