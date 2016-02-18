using UnityEngine;
using UnityEngine.Networking;

namespace MostDanger {

	public class TruckController : NetworkBehaviour
	{
		private Rigidbody rigidBody;
		private AudioSource audioSource;
		private Enginery _enginery;

		public GameObject mainRotor; // gameObject to be animated
		public GameObject tailRotor; // gameObject to be animated

		private float maxRotorForce = 22241.1081f;	// newtons
		private const float maxRotorVelocity = 7200f; // degrees per second
		private float rotorVelocity = 0.0f; // value between 0 and 1
		private float rotorRotation = 0.0f; // degrees... used for animating rotors

		private float maxTailRotorForce = 15000.0f; // newtons
		private	float maxTailRotorVelocity = 2200.0f; // degrees per second
		private float tailRotorVelocity = 0.0f; // value between 0 and 1
		private float tailRotorRotation = 0.0f; // degrees... used for animating rotors

		private float forwardRotorTorqueMultiplier = 0.05f; // multiplier for control input
		private float sidewaysRotorTorqueMultiplier = 0.05f; // multiplier for control input

		public bool mainRotorActive = true; // boolean for determining if a prop is active
		public bool tailRotorActive = true; // boolean for determining if a prop is active

		void Awake ()
		{
			rigidBody = GetComponent<Rigidbody> ();
			audioSource = GetComponent<AudioSource> ();
			_enginery = GetComponent<Enginery> ();
		}

		void Update ()
		{
			if (hasAuthority && _enginery.Pilot) {
				ManualUpdate ();
			} else {
				//TODO Автопилот
			}
		}
		// Forces are applied in a fixed update function so that they are consistent no matter what the frame rate of the game is. This is 
		// important to keeping the helicopter stable in the air. If the forces were applied at an inconsistent rate, the helicopter would behave 
		// irregularly.
		void FixedUpdate ()
		{

			// First we must compute the torque values that are applied to the helicopter by the propellers. The "Control Torque" is used to simulate
			// the variable angle of the blades on a helicopter and the "Torque Value" is the final sum of the torque from the engine attached to the 
			// main rotor, and the torque applied by the tail rotor.
			Vector3 torqueValue = Vector3.zero;

			Vector3 controlTorque = new Vector3( Input.GetAxis( "Mouse Y" ) * forwardRotorTorqueMultiplier, 1.0f, -Input.GetAxis( "Mouse X" ) * sidewaysRotorTorqueMultiplier );

			// Now check if the main rotor is active, if it is, then add it's torque to the "Torque Value", and apply the forces to the body of the 
			// helicopter.
			if ( mainRotorActive ) {
				torqueValue += (controlTorque * maxRotorForce * rotorVelocity);

				// Now the force of the prop is applied. The main rotor applies a force direclty related to the maximum force of the prop and the 
				// prop velocity (a value from 0 to 1)
				rigidBody.AddRelativeForce( Vector3.up * maxRotorForce * rotorVelocity );

				// This is simple code to help stabilize the helicopter. It essentially pulls the body back towards neutral when it is at an angle to
				// prevent it from tumbling in the air.
				if ( Vector3.Angle( Vector3.up, transform.up ) < 80 ) {
					transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.Euler( 0, transform.rotation.eulerAngles.y, 0 ), Time.deltaTime * rotorVelocity * 2 );
				}
			}

			// Now we check to make sure the tail rotor is active, if it is, we add it's force to the "Torque Value"
			if ( tailRotorActive ) {
				torqueValue -= (Vector3.up * maxTailRotorForce * tailRotorVelocity);
			}

			// And finally, apply the torques to the body of the helicopter.
			rigidBody.AddRelativeTorque( torqueValue );
		}

		void ManualUpdate ()
		{
			
			// This line simply changes the pitch of the attached audio emitter to match the speed of the main rotor.
			audioSource.pitch = rotorVelocity;

			// Now we animate the rotors, simply by setting their rotation to an increasing value multiplied by the helicopter body's rotation.
			if (mainRotorActive)
			{
				mainRotor.transform.rotation = transform.rotation * Quaternion.Euler( 0, rotorRotation, 0 );
			}

			if (tailRotorActive)
			{
				tailRotor.transform.rotation = transform.rotation * Quaternion.Euler( tailRotorRotation, 0, 0 );
			}

			// this just increases the rotation value for the animation of the rotors.
			rotorRotation += maxRotorVelocity * rotorVelocity * Time.deltaTime;
			tailRotorRotation += maxTailRotorVelocity * rotorVelocity * Time.deltaTime;

			// here we find the velocity required to keep the helicopter level. With the rotors at this speed, all forces on the helicopter cancel 
			// each other out and it should hover as-is.
			var hover_Rotor_Velocity = rigidBody.mass * Mathf.Abs( Physics.gravity.y ) / maxRotorForce;
			var hover_Tail_Rotor_Velocity = maxRotorForce * rotorVelocity / maxTailRotorForce;

			// Now check if the player is applying any throttle control input, if they are, then increase or decrease the prop velocity, otherwise, 
			// slowly LERP the rotor speed to the neutral speed. The tail rotor velocity is set to the neutral speed plus the player horizontal input. 
			// Because the torque applied by the main rotor is directly proportional to the velocity of the main rotor and the velocity of the tail rotor,
			// so when the tail rotor velocity decreases, the body of the helicopter rotates.

			if ( Input.GetAxis( "Vertical1" ) != 0.0f ) {
				rotorVelocity += Input.GetAxis( "Vertical1" ) * 0.001f;
			} else {
				rotorVelocity = Mathf.Lerp( rotorVelocity, hover_Rotor_Velocity, Time.deltaTime * Time.deltaTime * 5 );
			}

			tailRotorVelocity = hover_Tail_Rotor_Velocity - Input.GetAxis( "Horizontal1" );

			// now we set velocity limits. The multiplier for rotor velocity is fixed to a range between 0 and 1. You can limit the tail rotor velocity 
			// too, but this makes it more difficult to balance the helicopter variables so that the helicopter will fly well.
			rotorVelocity = Mathf.Clamp01(rotorVelocity);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Catapult();
            }

		}

        public void Catapult()
        {

            CmdCatapult();
        }

        [Command]
        public void CmdCatapult()
        {
            var pilot = GetComponent<Enginery>().Pilot;

            var pilotClientConnection = pilot.GetComponent<NetworkIdentity>().connectionToClient;
            GetComponent<NetworkIdentity>().RemoveClientAuthority(pilotClientConnection);
            GetComponent<Enginery>().Pilot = null;


            pilot.gameObject.SetActive(true);
            pilot.GetComponent<Transform>().position = GetComponent<Transform>().position - new Vector3(0, 5, 0);
            pilot.GetComponent<Transform>().rotation = GetComponent<Transform>().rotation;

        }

	}

}