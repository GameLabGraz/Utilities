# Utilities
The GameLabGraz/Utilities project contains a list of useful packages for [Unity](https://unity.com/):

- [LanguageManager](https://github.com/GameLabGraz/Utilities/tree/package/languagemanager) - Used to localize text components in Unity.
- [LimeSurveyUploader](https://github.com/GameLabGraz/Utilities/tree/package/limesurveyuploader) - Used to upload VV survey files to LimeSurvey.
- [QuestManager](https://github.com/GameLabGraz/Utilities/tree/package/questmanager) - Used to show users different main/sub quests.
- [SerializeProperty](https://github.com/GameLabGraz/Utilities/tree/package/serializeproperty) - Used to serialze c# properties for the Unity Editor.
- [Gadgets](https://github.com/GameLabGraz/Utilities/tree/package/gadgets) - Includes different useful stuff.

## Import packages into your Unity project
To import a package into a Unity project, add the following line to your /Packages/manifest.json file:

    "com.gamelabgraz.<package name>": "https://github.com/GameLabGraz/Utilities.git#package/<package name>"

## Update package in your Unity project
Remove the "lock" block for the package in your /Packages/manifest.json file.
Unity will then automatically import the latest version of the package.

    "com.gamelabgraz.<package name>": {
      "revision": "package/<package name>",
      "hash": "<hash value>"
    }
 
 ## Supported SDK's
| SDK | Download Link |
|---------------|---------------|
| Unity 2018.4 and higher  | [Unity] |
| HTC Vive | [HTC Vive] |
| Oculus | [Oculus Integration] |

[Unity]: https://unity3d.com
[HTC Vive]: https://www.vive.com
[Oculus Integration]: https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022
