using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace MostDanger {

	public class Weapon: NetworkBehaviour {

		public Vector3 CameraParams;

		public virtual void OnAttackDown () {
			
		}

		public virtual void OnAttackProcess () {

		}

		public virtual void OnAttackUp () {

		}

	}

}