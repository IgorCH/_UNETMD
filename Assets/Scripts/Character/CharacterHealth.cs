using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace MostDanger {
		
	public class CharacterHealth : NetworkBehaviour
	{
	    public float StartingHealth = 100f;             // The amount of health each tank starts with.
	    public Slider m_Slider;                           // The slider to represent how much health the tank currently has.
	    public Image m_FillImage;                         // The image component of the slider.
	    public Color m_FullHealthColor = Color.green;     // The color the health bar will be when on full health.
	    public Color m_ZeroHealthColor = Color.red;       // The color the health bar will be when on no health.
	    public AudioClip m_TankExplosion;                 // The clip to play when the tank explodes.
	    public ParticleSystem m_ExplosionParticles;       // The particle system the will play when the tank is destroyed.
	    public GameObject renderers;                // References to all the gameobjects that need to be disabled when the tank is dead.
	    public GameObject m_HealthCanvas;
	    public GameObject m_AimCanvas;
	    public GameObject m_LeftDustTrail;
	    public GameObject m_RightDustTrail;
		public CharacterSetup m_Setup;
		public CharacterManager Manager;                   //Associated manager, to disable control when dying.

	    [SyncVar(hook = "OnCurrentHealthChanged")]
	    private float m_CurrentHealth;                  // How much health the tank currently has.*

	    [SyncVar]
	    private bool m_ZeroHealthHappened;              // Has the tank been reduced beyond zero health yet?

        private CapsuleCollider m_Collider;                 // Used so that the tank doesn't collide with anything when it's dead.


	    private void Awake()
	    {
	        m_Collider = GetComponent<CapsuleCollider>();
	    }

	    public void Damage(float amount)
	    {
	        m_CurrentHealth -= amount;

	        if (m_CurrentHealth <= 0f && !m_ZeroHealthHappened)
	        {
	            OnZeroHealth();
	        }
	    }

	    void OnCurrentHealthChanged(float value)
	    {
	        m_CurrentHealth = value;
            m_Slider.value = m_CurrentHealth;
            m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / StartingHealth);
	    }

	    private void OnZeroHealth()
	    {
	        m_ZeroHealthHappened = true;
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
	        m_Collider.enabled = active;

	        renderers.SetActive(active);
	        m_HealthCanvas.SetActive(active);
	        m_AimCanvas.SetActive(active);
	        m_LeftDustTrail.SetActive(active);
	        m_RightDustTrail.SetActive(active);

	        if (active) Manager.EnableControl();
	        else Manager.DisableControl();

	        m_Setup.ActivateCrown(active);
	    }

	    public void SetDefaults()
	    {
	        m_CurrentHealth = StartingHealth;
	        m_ZeroHealthHappened = false;
	        SetTankActive(true);
	    }
	}

}