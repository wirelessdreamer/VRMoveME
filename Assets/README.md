# VRMoveME

Set up in existing project that already has SteamVR

1. Open The CameraRig Prefab
1. Attach the VRMoveME Script to it
1. Drag the Camera Rig GameObject into the Gamera Rig spot
1. On the Left controller Create an empty game object, I call mine VRMoveMe Orientation, set its y rotation to 180
1. Duplicate the Right Controller, rename it to Tracker1
1. Duplicate Tracker1, rename it to Tracker2
1. Under Steam VR_Controller Manager (on the camera rig) change Objects size from 0 to 2
1. Drag Tracker1 to element 0, and Tracker2 to element 1
1. Drag Tracker1 to Tracker 1 in VRMoveME Script, and Tracker2 to Tracker 2 in VRMoveME Script
1. Drag Camera (Head) Gameobject to Headset Obj in VRMoveME Script
1. I like to attach a Textmesh to the left controller that says to put it on your back, so people know if they have the correct controller or not.