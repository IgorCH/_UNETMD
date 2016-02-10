using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

namespace MostDanger {
	
	public class Bazooka : Weapon {


		public Rigidbody Shell;
		public Transform m_FireTransform;         // A child of the tank where the shells are spawned.

		public Slider m_AimSlider;                // A child of the tank that displays the current launch force.
		public AudioSource m_ShootingAudio;       // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
		public AudioClip m_ChargingClip;          // Audio that plays when each shot is charging up.
		public AudioClip m_FireClip;              // Audio that plays when each shot is fired.

		private float _minLaunchForce = 15f;      // The force given to the shell if the fire button is not held.
		private float _maxLaunchForce = 30f;      // The force given to the shell if the fire button is held for the max charge time.
		private float _maxChargeTime = 0.75f;     // How long the shell can charge for before it is fired at max force.

		private Rigidbody _rigidbody;          // Reference to the rigidbody component.
	
		[SyncVar]
		private float _currentLaunchForce;     // The force that will be given to the shell when the fire button is released.

		[SyncVar]
		private float _chargeSpeed;            // How fast the launch force increases, based on the max charge time.
		private bool _fired;                   // Whether or not the shell has been launched with this button press.

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody> ();
		}

		private void Start()
		{
			_chargeSpeed = (_maxLaunchForce - _minLaunchForce) / _maxChargeTime;
		}

		public override void ManualUpdate () {

			CameraParams = new Vector3 (0, 0, 0);

			m_AimSlider.value = _minLaunchForce;
			if (_currentLaunchForce >= _maxLaunchForce && !_fired)
			{
				_currentLaunchForce = _maxLaunchForce;
				Fire();
			}
			else if (Input.GetMouseButtonDown(0))
			{
				_fired = false;
				_currentLaunchForce = _minLaunchForce;
				m_ShootingAudio.clip = m_ChargingClip;
				m_ShootingAudio.Play();
			}
			else if (Input.GetMouseButton(0) && !_fired)
			{
				_currentLaunchForce += _chargeSpeed * Time.deltaTime;
				m_AimSlider.value = _currentLaunchForce;
			}
			else if (Input.GetMouseButtonUp(0) && !_fired)
			{
				_fired = true;
				Fire();
			}

		}

		private void Fire()
		{
			m_ShootingAudio.clip = m_FireClip;
			m_ShootingAudio.Play();
			CmdFire(_rigidbody.velocity, _currentLaunchForce, m_FireTransform.forward, m_FireTransform.position, m_FireTransform.rotation);
			_currentLaunchForce = _minLaunchForce;
		}

		[Command]
		private void CmdFire(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation)
		{
			Rigidbody shellInstance = Instantiate(Shell, position, rotation) as Rigidbody;
			Vector3 velocity = rigidbodyVelocity + launchForce * forward;
			shellInstance.velocity = velocity;
			NetworkServer.Spawn(shellInstance.gameObject);
		}

		public void SetDefaults()
		{
			_currentLaunchForce = _minLaunchForce;
			m_AimSlider.value = _minLaunchForce;
		}

	}

}
