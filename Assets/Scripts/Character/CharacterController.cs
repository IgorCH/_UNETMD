using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

namespace MostDanger {
		
	public class CharacterController : NetworkBehaviour
	{
	    public int PlayerNumber = 1;                // Used to identify which tank belongs to which player.  This is set by this tank's manager.
	    
		public float m_Speed = 12f;                   // How fast the tank moves forward and back.
	    public float m_TurnSpeed = 18f;              // How fast the tank turns in degrees per second.
	    public float m_PitchRange = 0.2f;             // The amount by which the pitch of the engine noises can vary.
	    
		public AudioSource m_MovementAudio;           // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
	    public AudioClip m_EngineIdling;              // Audio to play when the tank isn't moving.
	    public AudioClip m_EngineDriving;             // Audio to play when the tank is moving.
	    public ParticleSystem m_LeftDustTrail;        // The particle system of dust that is kicked up from the left track.
	    public ParticleSystem m_RightDustTrail;       // The particle system of dust that is kicked up from the rightt track.
	    public Rigidbody Rigidbody;              // Reference used to move the tank.

	    private float movementInput;              // The current value of the movement input.
	    private float oldX;
	    private float originalPitch;              // The pitch of the audio source at the start of the scene.

	    public Animator Animator;

	    private GameObject _highlightedObject;

		public Weapon CurrentWeapon;
		public List<WeaponStruct> Weapons;

	    private void Awake()
	    {
	        Rigidbody = GetComponent<Rigidbody>();

			Weapons = new List<WeaponStruct> ();

			Weapons.Add (new WeaponStruct() {
				Name = "Fist",
				ScriptName = "Fist",
				Count = -1
			});

			Weapons.Add (new WeaponStruct() {
				Name = "Bazooka",
				ScriptName = "Bazooka",
				Count = 100
			});	

			CurrentWeapon = (Weapon)gameObject.GetComponent("MostDanger.Fist");
	    }

	    private void Start()
	    {
	        originalPitch = m_MovementAudio.pitch;
	    }

		[ClientCallback]
	    private void Update()
	    {

			if (isLocalPlayer) {
				
				if (Input.GetKeyDown (KeyCode.Space)) {
					if (InventoryGUI.Instance.IsOpened) {
						InventoryGUI.Instance.Close ();
					} else {
						InventoryGUI.Instance.Open (Weapons, OnInventorySelect);
					}
				}

				if (CurrentWeapon) {
					CurrentWeapon.ManualUpdate ();
				}
				
				movementInput = Input.GetAxis ("Vertical1");

				UpdateHighlightedObject ();

				if (Input.GetKeyDown (KeyCode.R) && _highlightedObject) {
					//_highlightedObject.GetComponent<AirplaneController>().SetAuthority();
					//Debug.Log("IDs: " + this.GetComponent<NetworkIdentity>().connectionToServer.connectionId.ToString());
					//CmdServerAssignClient (_highlightedObject.name);
					//gameObject.SetActive(false);
				}

				EngineAudio ();
			}
	    }

		/*[Command]
		void CmdServerAssignClient(string name)
		{
			GameObject airplane = GameObject.Find(name);
			var conn = this.GetComponent<NetworkIdentity> ().connectionToClient;
			airplane.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
		}*/

	    private void UpdateHighlightedObject()
	    {
	        _highlightedObject = null;
	        var objs = FindObjectsOfType<Enginery>();
	        if (objs.Length > 0)
	        {
	            _highlightedObject = objs[0].gameObject;
	        }
	    }

	    private void OnInventorySelect (WeaponStruct weapon) 
		{
			if (CurrentWeapon)
			{
				Destroy (CurrentWeapon);
			}

			CurrentWeapon = (Weapon)gameObject.GetComponent("MostDanger." + weapon.ScriptName);
		}

	    private void EngineAudio()
	    {
	        if (Mathf.Abs(movementInput) < 0.1f && Mathf.Abs(Input.mousePosition.x - oldX) < 0.1f)
	        {
	            if (m_MovementAudio.clip == m_EngineDriving)
	            {
	                m_MovementAudio.clip = m_EngineIdling;
	                m_MovementAudio.pitch = UnityEngine.Random.Range(originalPitch - m_PitchRange, originalPitch + m_PitchRange);
	                m_MovementAudio.Play();
	            }
	        }
	        else
	        {
	            if (m_MovementAudio.clip == m_EngineIdling)
	            {
	                m_MovementAudio.clip = m_EngineDriving;
	                m_MovementAudio.pitch = UnityEngine.Random.Range(originalPitch - m_PitchRange, originalPitch + m_PitchRange);
	                m_MovementAudio.Play();
	            }
	        }
	    }

	    private void FixedUpdate()
	    {
			if (!isLocalPlayer)
	            return;

			Move();
	    }

	    private void Move()
	    {
	        Vector3 movement = transform.forward * movementInput * m_Speed * Time.deltaTime;
			Animator.SetFloat("MoveSpeed", movementInput);
	        Rigidbody.MovePosition(Rigidbody.position + movement);

			float turn = (Input.mousePosition.x - oldX) * m_TurnSpeed * Time.deltaTime;
			oldX = Input.mousePosition.x;
			Quaternion inputRotation = Quaternion.Euler(0f, turn / 10, 0f);
			Rigidbody.MoveRotation(Rigidbody.rotation * inputRotation);
	    }
			
	    public void SetDefaults()
	    {
	        Rigidbody.velocity = Vector3.zero;
	        Rigidbody.angularVelocity = Vector3.zero;

	        movementInput = 0f;

            m_LeftDustTrail.Clear();
	        m_LeftDustTrail.Stop();

	        m_RightDustTrail.Clear();
	        m_RightDustTrail.Stop();

			if (CurrentWeapon)
			{
				CurrentWeapon.SetDefaults ();
			}
	    }

	    public void ReEnableParticles()
	    {
	        m_LeftDustTrail.Play();
	        m_RightDustTrail.Play();
	    }

	    //We freeze the rigibody when the control is disabled to avoid the tank drifting!
	    protected RigidbodyConstraints _originalConstrains;
	    void OnDisable()
	    {
	        _originalConstrains = Rigidbody.constraints;
	        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
	    }

	    void OnEnable()
	    {
	        Rigidbody.constraints = _originalConstrains;
	    }
	}

}