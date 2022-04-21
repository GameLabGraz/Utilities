---------------------------------------------------------------------------------------------------
SteamVR Change list: SteamVR v2.7.3 (sdk 1.14.15)
---------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------

1. Files you should not overwrite/delete:
---------------------------------------------------------------------------------------------------
- SteamVR\Input\ExampleJSON\bindings_oculus_touch.json



2. Script Changes:
---------------------------------------------------------------------------------------------------
	
File: SteamVR\InteractionSystem\Core\Scripts\CircularDrive.cs
	- Member Variables: worldPlaneNormal, localPlaneNormal, start
		delete the following code in VRHoverButton.cs:
			* GetFieldValue function
			* SetFieldValue function
			* replace the setters/getters with the real variables
	- HandHoverUpdate(Hand):
		remove the callBaseMethod call in VRCircularDrive.cs once this is protected
	- ComputeAngle(Hand):
		remove the callBaseMethod call in VRCircularDrive.cs once this is protected
	- Remove the function UpdateAll() from VRCircularDrive.cs once the UpdateAll method and the called methods are protected/virtual 
  

File: SteamVR\InteractionSystem\Core\Scripts\HoverButton.cs
	- Start():
		remove the callBaseMethod call in VRHoverButton.cs once this is protected
	- InvokeEvents(bool, bool):
		remove the callBaseMethod call in VRHoverButton.cs once this is protected
	- Member Variables: startPosition, endPosition, handEnteredPosition, hovering, lastHoveredHand
		delete the following code in VRHoverButton.cs:
			* GetFieldValue function
			* SetFieldValue function
			* replace the setters/getters with the real variables
