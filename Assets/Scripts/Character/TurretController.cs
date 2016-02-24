using UnityEngine;
using UnityEngine.Networking;

namespace MostDanger {

	public class TurretController : NetworkBehaviour
	{
		//private Rigidbody rigidBody;
		//private AudioSource audioSource;
	    private Enginery _enginery;

		void Awake ()
		{
			//rigidBody = GetComponent<Rigidbody> ();
			//audioSource = GetComponent<AudioSource> ();
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

		void FixedUpdate ()
		{


		}

		void ManualUpdate ()
		{
			

		}

        public void Catapult()
        {

        }

        [Command]
        public void CmdCatapult()
        {


        }

	}

}