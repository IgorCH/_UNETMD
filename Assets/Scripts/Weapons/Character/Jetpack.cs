using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace MostDanger {
	
	public class Jetpack : Weapon {

        #region Magic Nums
        private const float JetpackAngleMin = 10f;
        private const float JetpackAngleMax = 90f;
        private const float MaxEnergy = 100;
        private const float EnergyRate = 20;
        private const float JetpackAngleChangeSpeed = 6;
        #endregion

        private Rigidbody _rigidbody;
	    public GameObject JetpackGO;
        public GameObject JetpackParticlesLeft;
        public GameObject JetpackParticlesRight;

	    public float MaxFuel = 100f;
	    private float CurrentFuel = 100f;
	    private float FuelUseSpeed = 10f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {

        }

        public override void OnSelect()
        {
            JetpackGO.SetActive(true);
        }

        public override void OnDeselect()
        {
            JetpackGO.SetActive(false);
        }

        public override void ManualUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                JetpackParticlesLeft.SetActive(true);
                JetpackParticlesRight.SetActive(true);
            }

            if (Input.GetMouseButton(0))
            {
                _rigidbody.AddRelativeForce(0, 100000, 10000 * 60 * Time.deltaTime, ForceMode.Force);
            }

            if (Input.GetMouseButtonUp(0))
            {
                JetpackParticlesLeft.SetActive(false);
                JetpackParticlesRight.SetActive(false);
            }

        }

        public override void SetDefaults()
        {

        }

	}

}