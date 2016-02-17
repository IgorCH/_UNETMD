/* 	


	This is a simple script that is attached to the player object to make a simple gun. It uses a raycast to draw an imaginary
line forward, and uses that data to determine the object it hit. In this case, it simply instantiates a prefab object (a bullet 
impact effect) at the point of contact.
*/

using UnityEngine;
using UnityEngine.Networking;

namespace MostDanger {

	public class HelicopterWeapon : MonoBehaviour
	{

		public GameObject gunEmitterObject;	// the object the script uses to determine the gun orientation (also has audio 
		// and particle emitters attached

		private float weaponFireDelay = 0.0f;	// the delay between shots (in seconds)
		public GameObject bulletImpactPrefab;	// the effect created when a "bullet" hits the ground

		private float weaponFireTimer = 0.0f;	// a counter to time up to the weapon fire delay.

		void Update () {
			//gunEmitterObject.GetComponent<ParticleEmitter>().emit = false;	// turn off the particle emitter attached to the gun...

			if ( Input.GetKeyDown(KeyCode.E) && weaponFireTimer >= weaponFireDelay ) {	// if the fire key is down, and the fire timer
				// is greater than the fire delay, then
				weaponFireTimer = 0.0f;	// set the fire timer to 0 so we can begin counting again.
				//gunEmitterObject.GetComponent<AudioSource>().Play();	// play the sound attached to the gun object.
				//gunEmitterObject.GetComponent<ParticleEmitter>().emit = true;		// and turn on it's particle emitter.


				// now we perform the raycast. The variable "hit" is defined as a "Raycast hit". When you perform a raycast with a raycast hit
				// as one of the variables, it is automatically set up with all of the results from the raycast, including distance, object hit,
				// normal of the surface hit, ect.
				RaycastHit hit;

				// now if the raycast from the gun object returns true (if we hit something)
				if ( Physics.Raycast( gunEmitterObject.transform.position, gunEmitterObject.transform.forward, out hit ) ) {
					// instantiate the bullet impact prefab so that it is pointing at the normal direction of the surface, and is at the hit 
					// location
					Instantiate( bulletImpactPrefab, hit.point, Quaternion.LookRotation( hit.normal ) );
				}
			}

			// and last, increase the fire timer.
			weaponFireTimer += Time.deltaTime;
		}
	}
}