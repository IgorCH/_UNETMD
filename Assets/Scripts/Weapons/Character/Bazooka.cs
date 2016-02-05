using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

namespace MostDanger {
	
	public class Bazooka : Weapon {


		public Rigidbody m_Shell;                 // Prefab of the shell.
		public Transform m_FireTransform;         // A child of the tank where the shells are spawned.

		public Slider m_AimSlider;                // A child of the tank that displays the current launch force.
		public AudioSource m_ShootingAudio;       // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
		public AudioClip m_ChargingClip;          // Audio that plays when each shot is charging up.
		public AudioClip m_FireClip;              // Audio that plays when each shot is fired.

		public float m_MinLaunchForce = 15f;      // The force given to the shell if the fire button is not held.
		public float m_MaxLaunchForce = 30f;      // The force given to the shell if the fire button is held for the max charge time.
		public float m_MaxChargeTime = 0.75f;     // How long the shell can charge for before it is fired at max force.

		private Rigidbody m_Rigidbody;          // Reference to the rigidbody component.
	

		[SyncVar]
		private float m_CurrentLaunchForce;     // The force that will be given to the shell when the fire button is released.

		[SyncVar]
		private float m_ChargeSpeed;            // How fast the launch force increases, based on the max charge time.
		private bool m_Fired;                   // Whether or not the shell has been launched with this button press.

		private void Awake()
		{
			m_Rigidbody = GetComponent<Rigidbody> ();
		}

		private void Start()
		{
			m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
		}

		public override void ManualUpdate () {

			CameraParams = new Vector3 (0, 0, 0);

			m_AimSlider.value = m_MinLaunchForce;
			if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
			{
				m_CurrentLaunchForce = m_MaxLaunchForce;
				Fire();
			}
			else if (Input.GetKeyDown(KeyCode.R))
			{
				m_Fired = false;
				m_CurrentLaunchForce = m_MinLaunchForce;
				m_ShootingAudio.clip = m_ChargingClip;
				m_ShootingAudio.Play();
			}
			else if (Input.GetKey(KeyCode.R) && !m_Fired)
			{
				m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
				m_AimSlider.value = m_CurrentLaunchForce;
			}
			else if (Input.GetKeyUp(KeyCode.R) && !m_Fired)
			{
				m_Fired = true;
				Fire();
			}

		}

		private void Fire()
		{
			m_ShootingAudio.clip = m_FireClip;
			m_ShootingAudio.Play();
			CmdFire(m_Rigidbody.velocity, m_CurrentLaunchForce, m_FireTransform.forward, m_FireTransform.position, m_FireTransform.rotation);
			m_CurrentLaunchForce = m_MinLaunchForce;
		}

		[Command]
		private void CmdFire(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation)
		{
			Rigidbody shellInstance = Instantiate(m_Shell, position, rotation) as Rigidbody;
			Vector3 velocity = rigidbodyVelocity + launchForce * forward;
			shellInstance.velocity = velocity;
			NetworkServer.Spawn(shellInstance.gameObject);
		}

		public void SetDefaults()
		{
			m_CurrentLaunchForce = m_MinLaunchForce;
			m_AimSlider.value = m_MinLaunchForce;
		}

	}

}
