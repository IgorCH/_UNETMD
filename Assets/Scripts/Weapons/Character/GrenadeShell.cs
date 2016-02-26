using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace MostDanger {

	public class GrenadeShell : NetworkBehaviour
	{
		public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
		public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
		public float m_MaxDamage = 100f;                    // The amount of damage done if the explosion is centred on a tank.
		public float m_ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
		public float m_MaxLifeTime = 2f;                    // The time in seconds before the shell is removed.
		public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.


		private int m_TankMask = LayerMask.GetMask("Players"); // A layer mask so that only the tanks are affected by the explosion.

		private void Start()
		{
			if (isServer)
			{
				// If it isn't destroyed by then, destroy the shell after it's lifetime.
				Destroy(gameObject, m_MaxLifeTime);
				GetComponent<Collider>().enabled = false;
				StartCoroutine(EnableCollision());
			}
		}

		//allow to delay a bit the activation of the collider so that it don't collide when spawn close to the canon
		IEnumerator EnableCollision()
		{
			yield return new WaitForSeconds(0.1f);
			GetComponent<Collider>().enabled = true;
		}
			
		public void ForceExplode()
		{
			// Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
			Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

			for (int i = 0; i < colliders.Length; i++)
			{
				Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

				if (!targetRigidbody)
					continue;

				CharacterHealth targetHealth = targetRigidbody.GetComponent<CharacterHealth>();

				if (targetHealth) {

					Vector3 explosionToTarget = targetRigidbody.position - transform.position;
					float explosionDistance = explosionToTarget.magnitude;
					float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
					float damage = relativeDistance * m_MaxDamage;
					damage = Mathf.Max (0f, damage);

					targetHealth.Damage (damage);
				}
			}

			//if we are ALSO client (so hosting), this will be done by the Destroy so Skip
			if (!NetworkClient.active)
				PhysicForces();

			// Destroy the shell on clients.
			NetworkServer.Destroy(gameObject);
		}

		//called on client when the Network destroy that object (it was destroyed on server)
		public override void OnNetworkDestroy()
		{
			//we spawn the explosion particle
			m_ExplosionParticles.transform.parent = null;
			m_ExplosionParticles.Play();
			m_ExplosionAudio.Play();

			PhysicForces();

			//set the particle to be destroyed at the end of their lifetime
			Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);
			base.OnNetworkDestroy();
		}

		//This apply force on object. Do that on all clients & server as each must apply force to object they own
		void PhysicForces()
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

			for (int i = 0; i < colliders.Length; i++)
			{
				Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

				if (!targetRigidbody || !targetRigidbody.GetComponent<NetworkIdentity>().hasAuthority)
					continue;

				targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);
			}
		}
	}

}