using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace MostDanger {
	
	public class Grenade : Weapon {

        private void Awake()
        {

        }

        private void Start()
        {

        }

        public override void OnSelect()
        {

        }

        public override void OnDeselect()
        {

        }

        public override void ManualUpdate()
        {

            CameraParams = new Vector3(0, 0, 0);


        }

        private void Fire()
        {

        }

        [Command]
        private void CmdFire(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation)
        {

        }

        public override void SetDefaults()
        {

        }

	}

}
