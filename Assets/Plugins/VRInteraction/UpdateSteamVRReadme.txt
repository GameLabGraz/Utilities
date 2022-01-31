---------------------------------------------------------------------------------------------------
SteamVR Change list: SteamVR v2.5.0 (sdk 1.8.19)
---------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------

1. Files you show not overwrite/delete:
---------------------------------------------------------------------------------------------------
- SteamVR\Input\ExampleJSON\bindings_oculus_touch.json



2. Script Changes:
---------------------------------------------------------------------------------------------------
File: SteamVR\Input\SteamVR_Action_Vibration.cs
      Change in Line 197 (first if in function Execute(...))
	  
	  if (SteamVR_Input.isStartupFrame || OpenVR.Input == null) 
		return;
		
		
File: SteamVR\InteractionSystem\Core\Scripts\CircularDrive.cs
	  Change all everything from 'private' to 'protected'
	  Make the following methods 'virtual':
		- Start()
		- HandHoverUpdate(...)
		- UpdateLinearMapping()
		- UpdateGameObject()
		
		
File: SteamVR\InteractionSystem\Core\Scripts\FallbackCameraController.cs
	  Change the Update() function to FixedUpdate()
	  

File: SteamVR\InteractionSystem\Core\Scripts\HoverButton.cs
	  Change all everything from 'private' to 'protected'
	  
	 
NOT SURE: 
File: SteamVR\InteractionSystem\Samples\Scripts\ButtonEffect.cs
	  Add the following function:
	    public void OnButtonPressed(Hand fromHand)
        {
            ColorSelf(Color.magenta);
        }

	  
	  