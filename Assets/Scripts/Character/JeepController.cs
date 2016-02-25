using UnityEngine;
using UnityEngine.Networking;

using UnityStandardAssets.Vehicles.Car;

namespace MostDanger {

    [RequireComponent(typeof(Enginery))]
    [RequireComponent(typeof(CameraEngineryController))]
    [RequireComponent(typeof(CarController))]
	public class JeepController : NetworkBehaviour
	{
		private Rigidbody rigidBody;
		private AudioSource audioSource;
		private Enginery _enginery;
        private CarController car;

        void Awake ()
		{
			rigidBody = GetComponent<Rigidbody> ();
			audioSource = GetComponent<AudioSource> ();
			_enginery = GetComponent<Enginery> ();

            car = GetComponent<CarController>();
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

            // pass the input to the car!
            float h = Input.GetAxis("Horizontal1");
            float v = Input.GetAxis("Vertical1");
            float handbrake = 0;//TODO CrossPlatformInputManager.GetAxis("Jump");
            car.Move(h, v, v, handbrake);

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