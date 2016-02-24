using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace MostDanger {
	
	public class Jetpack : Weapon {

        public Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {

        }

        public override void ManualUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {

            }

            if (Input.GetMouseButton(0))
            {

            }

            if (Input.GetMouseButtonUp(0))
            {

            }

        }

        public override void SetDefaults()
        {

        }

	}

}