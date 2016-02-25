using UnityEngine;
using UnityEngine.Networking;

namespace MostDanger {

    [RequireComponent(typeof(Enginery))]
	public class JeepController : NetworkBehaviour
	{
		private Rigidbody rigidBody;
		private AudioSource audioSource;
		private Enginery _enginery;



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

		void FixedUpdate ()
		{

		
		}

		void ManualUpdate ()
		{
			
			

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