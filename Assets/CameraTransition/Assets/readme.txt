Camera Dissolve effect for unity
Copyright Sebastian Strandber 2017

This is a simple effect that will let you softly fade between two cameras,
instead of having to fade to  black or white inbetween.

To use this effect in a scene, you will need
	1) CameraFadeManager on a Game Object in the scene
	2) An active camera to transition from.
	3) An inactive camera to transition to.

When you want to execute the effect, call DissolveBetween() on the
CameraTransitionManager, with the camera references and a transition time as
parameters. After the transition is complete, the first camera will be set to
be inactive and the second active automatically.

Nothing in the script will affect audio listeners, so those will have to be
moved manually from one location to another.

If you have any questions, email me at sfdstrandberg@gmail.com or contact me 
on twitter @sfdstrandberg 