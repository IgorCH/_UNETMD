/*
============================================================================================================
	 _	 _  ____  _      _  ____  _____  _____  _______  ____  _____
	| | | || ___|| |    | || ___||  _  || ___ ||__   __|| ___|| ___ |
	| |_| || |__ | |    | || |   | | | || |_| |   | |   | |__ | |_| |
	|  _  || ___|| |    | || |   | | | || ____|   | |   | ___|| __  |
	| | | || |__ | |___ | || |__ | |_| || |       | |   | |__ | | \ \
	|_| |_||____||_____||_||____||_____||_|       |_|   |____||_|  \_\
	
		 _______  _   _  _______  _____  _____   _  _____  _
		|__   __|| | | ||__   __||  _  || ___ | | ||  _  || |
		   | |   | | | |   | |   | | | || |_| | | || |_| || |
		   | |   | | | |   | |   | | | ||  _  | | ||  _  || |
		   | |   | |_| |   | |   | |_| || | \ \ | || | | || |___
		   |_|   |_____|   |_|   |_____||_|  \_\|_||_| |_||_____|
	   
				   	 _________________________
					|_____by: Andrew Gotow____|
			
============================================================================================================

	This script draws the HUD for the helicopter. It's fairly simple and uses only the basic Unity GUI functions.
*/


var		player_gameobject 		: GameObject;	// the Player helicopter object
var		altimeter_texture 		: Texture;		// the image to be used as the altimeter
var 	throttle_texture 		: Texture[];	// the set of images to be used for the throttle Because you can not
												// rotate a GUI image, in order to make the needle on the gauge, you
												// need to have a series of images that you animate through.
												
private var helicopter_throttle : float;		// This variable simply represents the player throttle. It is set by
												// accessing the static global variable and reading it's value.

function OnGUI () {
	// the following raycast is used to get the distance from the ground. This allows us to display an accurate measure of the
	// distance the player is from the ground, making the helicopter much easier to control.
	
	var groundHit : RaycastHit;		// define a "raycast hit" variable. When a raycast is called, a raycast hit will be
									// automatically set up and filled with all of the results from the raycast, including
									// distance, the surface normal, and the other object.
	Physics.Raycast( player_gameobject.transform.position - Vector3.up, -Vector3.up, groundHit );	// now we actually call the
																									// raycast, from the copter
																									// position
	
	helicopter_throttle = player_gameobject.GetComponent( "Helicopter_Script" ).rotor_Velocity;		// now set the helicopter
																									// throttle value to the
																									// velocity specified in the
																									// main helicopter script.
	
	// now we draw the GUI objects...
	GUI.Label( Rect( 0, 0, 128, 128 ), altimeter_texture );		// first, we draw the background texture for the altimeter gauge
	GUI.Label( Rect( 0, 128, 128, 128 ), throttle_texture[ helicopter_throttle * 10 ] );	// now we draw the throttle texture
																							// based on what the helicopter script
																							// gives us (the 10 is in there because
																							// there are 10 frames to the gauge 
																							// animation).
																							
	
	GUI.Label( Rect( 40, 40, 256, 256 ), Mathf.Round( groundHit.distance ) + " m" );	// now we write out the distance from the 
																						// ground on top of the altimeter back
	GUI.Label( Rect( 20, 182, 256, 256 ), "ENG" );		// and print "ENG" on top of the throttle so you can see what it is.
}