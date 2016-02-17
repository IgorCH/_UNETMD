/*
	This is a simple little script to send a message to the helicopter object to notify it that one of the props
has been destroyed. The helicopter has functions called "MainRotorDestroyed" and "TailRotorDestroyed". These functions
basically disable certain aspects of the helicopter script, and are called from the props themselves using the 
"SendMessage" function. This is necessary because the helicopter body itself can not respond to the built in "OnTriggerEnter"
event for another object, so using the same script, it is impossible to determine whether the props had hit anything. This
script is not necessary, but allows us to use these functions easily.
*/

using UnityEngine;
using UnityEngine.Networking;

namespace MostDanger {

	public class HelicopterRotorDestruction : MonoBehaviour
	{
				
		public string rotorName;		// A string that tells the script which function to call.
		public GameObject main_Body;	// A variable so the script knows what object the function is in.
		public GameObject explosion_Prefab;	// The explosion object to be created when the rotor is destroyed.


		// Now we can respond to the basic OnTriggerEnter event. This function is called automatically any time an object with a collider 
		// marked as a "trigger" comes in contact with another.
		void OnTriggerEnter () {

			// now we simply set a value in the helicopter script to set the rotor enabled value
			if ( rotorName == "MainRotor" ) {
				main_Body.GetComponent<HelicopterController>().mainRotorActive = false;
			} else if ( rotorName == "TailRotor" ) {
				main_Body.GetComponent<HelicopterController>().tailRotorActive = false;
			}

			// Instantiate the explosion prefab object.
			Instantiate( explosion_Prefab, transform.position, transform.rotation );

			// and finally, we destroy the propeller gameobject itself.
			Destroy( gameObject );
		}
	}
}