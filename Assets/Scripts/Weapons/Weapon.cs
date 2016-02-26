using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace MostDanger {

	public class Weapon: NetworkBehaviour {

		public Vector3 CameraParams;
		protected Rigidbody __rigidbody;
		protected Transform __transform;

		protected void Awake()
		{
			__rigidbody = GetComponent<Rigidbody> ();
			__transform = GetComponent<Transform> ();
		}

	    public virtual void OnSelect()
	    {
	        
	    }

        public virtual void OnDeselect()
        {

        }

		public virtual void ManualUpdate ()
		{
			
		}

		public virtual void SetDefaults()
		{

		}

	}

}