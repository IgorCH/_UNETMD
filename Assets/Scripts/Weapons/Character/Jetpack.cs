using UnityEngine;
using System.Collections;

namespace MostDanger {
	
	public class Jetpack : Weapon {

		private void Awake()
		{
			//_rigidbody = GetComponent<Rigidbody> ();
		}

		private void Start()
		{
			
		}

		public override void ManualUpdate () {

			CameraParams = new Vector3 (0, 0, 0);

			if(Input.GetMouseButtonDown(0)) {
				
			}

			if(Input.GetMouseButton(0)) {

			}

			if(Input.GetMouseButtonUp(0)) {

			}

		}

		public override void SetDefaults()
		{

		}
	}

}