using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

namespace MostDanger {

	public class Grenade : Weapon {


		public Rigidbody GrenadeShell;
		public Transform FireTransform;         // A child of the tank where the shells are spawned.

		public AudioSource ShootingAudio;       // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
		public AudioClip ChargingClip;          // Audio that plays when each shot is charging up.
		public AudioClip FireClip;              // Audio that plays when each shot is fired.

		private float _minLaunchForce = 15f;      // The force given to the shell if the fire button is not held.
		private float _maxLaunchForce = 30f;      // The force given to the shell if the fire button is held for the max charge time.
		private float _maxChargeTime = 0.75f;     // How long the shell can charge for before it is fired at max force.

		[SyncVar]
		private float _currentLaunchForce;     // The force that will be given to the shell when the fire button is released.

		[SyncVar]
		private float _chargeSpeed;            // How fast the launch force increases, based on the max charge time.
		private bool _fired;                   // Whether or not the shell has been launched with this button press.

		private GrenadeShell currentGrenade;

		public new void Awake()
		{
			base.Awake ();
		}

		private void Start()
		{
			_chargeSpeed = (_maxLaunchForce - _minLaunchForce) / _maxChargeTime;
		}

		public override void ManualUpdate () {

			CameraParams = new Vector3 (0, 0, 0);

			if (Input.GetMouseButtonDown(0) && currentGrenade) {
				currentGrenade.ForceExplode ();
				return;
			} 
				
			if (Input.GetMouseButtonDown(0) && !EventSystem.current.currentSelectedGameObject)
			{
				_fired = false;
				_currentLaunchForce = _minLaunchForce;
				ShootingAudio.clip = ChargingClip;
				ShootingAudio.Play();
			}
			else if (Input.GetMouseButton(0) && !_fired && !EventSystem.current.currentSelectedGameObject)
			{
				_currentLaunchForce += _chargeSpeed * Time.deltaTime;

				if (_currentLaunchForce >= _maxLaunchForce)
				{
					_currentLaunchForce = _maxLaunchForce;
					_fired = true;
					Fire();
				}
			}
			else if (Input.GetMouseButtonUp(0) && !_fired && !EventSystem.current.currentSelectedGameObject)
			{
				_fired = true;
				Fire();
			}

		}

		private void Fire()
		{
			ShootingAudio.clip = FireClip;
			ShootingAudio.Play();
			CmdFire(__rigidbody.velocity, _currentLaunchForce, FireTransform.forward, FireTransform.position, FireTransform.rotation);
			_currentLaunchForce = _minLaunchForce;
		}

		[Command]
		private void CmdFire(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation)
		{
			Rigidbody shellInstance = Instantiate(GrenadeShell, position, rotation) as Rigidbody;
			Vector3 velocity = rigidbodyVelocity + launchForce * forward;
			shellInstance.velocity = velocity;
			NetworkServer.SpawnWithClientAuthority(shellInstance.gameObject, connectionToClient);
			currentGrenade = shellInstance.GetComponent<GrenadeShell> ();
		}

		public override void SetDefaults()
		{
			_currentLaunchForce = _minLaunchForce;
		}

	}

}
