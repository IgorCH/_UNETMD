using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

namespace MostDanger
{

	public class DynamiteShell : NetworkBehaviour 
    {

		private const float MaxDamage = 100f;                    // The amount of damage done if the explosion is centred on a tank.
		private const float ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
		private const float ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.
		private const float TimeToExplosion = 3;

		public ParticleSystem ExplosionParticles;         // Reference to the particles that will play on explosion.
		public AudioSource ExplosionAudio;                // Reference to the audio that will play on explosion.

		private int CharactersMask = LayerMask.GetMask("Players"); // A layer mask so that only the tanks are affected by the explosion.

		private float _timer = 5f;


        private void Awake()
        {

        }

		private void Start()
		{
			if (isServer)
			{
				Destroy (gameObject, TimeToExplosion);
			}
        }

        void Update()
        {
			_timer -= Time.deltaTime;
			if (_timer < 0)
			{
				Fire ();
			}
        }

        private void Fire()
        {
			Debug.Log ("Boom by timer");
			CmdFire ();
        }

        [Command]
        private void CmdFire()
        {
			PhysicForces ();
        }

		void PhysicForces()
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, CharactersMask);

			for (int i = 0; i < colliders.Length; i++)
			{
				Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

				if (!targetRigidbody || !targetRigidbody.GetComponent<NetworkIdentity>().hasAuthority)
					continue;

				targetRigidbody.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
			}
		}

		public override void OnNetworkDestroy()
		{
			//we spawn the explosion particle
			ExplosionParticles.transform.parent = null;
			ExplosionParticles.Play();
			ExplosionAudio.Play();

			PhysicForces();

			Destroy(ExplosionParticles.gameObject, ExplosionParticles.duration);
			base.OnNetworkDestroy();
		}


    }

}
