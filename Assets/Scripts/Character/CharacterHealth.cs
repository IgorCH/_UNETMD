using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace MostDanger {
		
	public class CharacterHealth : NetworkBehaviour
	{
	    public float StartingHealth = 100f;               // The amount of health each tank starts with.

		public AudioClip m_TankExplosion;                 // The clip to play when the tank explodes.
	    public ParticleSystem m_ExplosionParticles;       // The particle system the will play when the tank is destroyed.
	    public GameObject renderers;                      // References to all the gameobjects that need to be disabled when the tank is dead.

		public CharacterSetup m_Setup;
		public CharacterManager Manager;                  //Associated manager, to disable control when dying.

	    [SyncVar(hook = "OnCurrentHealthChanged")]
		private float _currentHealth;                    // How much health the tank currently has.*

	    [SyncVar]
		private bool _isZeroHealthHappened;                // Has the tank been reduced beyond zero health yet?

		private CapsuleCollider _collider;               // Used so that the tank doesn't collide with anything when it's dead.


	    private void Awake()
	    {
	        _collider = GetComponent<CapsuleCollider>();
	    }

	    public void Damage(float amount)
	    {
	        _currentHealth -= amount;

	        if (_currentHealth <= 0f && !_isZeroHealthHappened)
	        {
				_isZeroHealthHappened = true;
	            OnZeroHealth();
	        }
	    }

	    void OnCurrentHealthChanged(float value)
	    {
	        _currentHealth = value;
	    }

	    private void OnZeroHealth()
	    {
	        RpcOnZeroHealth();
	    }

	    [ClientRpc]
	    private void RpcOnZeroHealth()
	    {
	        m_ExplosionParticles.Play();
	        AudioSource.PlayClipAtPoint(m_TankExplosion, transform.position);
            SetTankActive(false);
	    }

	    private void SetTankActive(bool active)
	    {
	        _collider.enabled = active;
	        renderers.SetActive(active);
	        Manager.SetControlEnabled(active);
	    }

	    public void SetDefaults()
	    {
	        _currentHealth = StartingHealth;
	        _isZeroHealthHappened = false;
	        SetTankActive(true);
	    }
	}

}