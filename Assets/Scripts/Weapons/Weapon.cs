using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace MostDanger {

	public class Weapon: NetworkBehaviour {

		public Vector3 CameraParams;

	    public virtual void OnSelect()
	    {
	        
	    }

        public virtual void OnDeselect()
        {

        }

		public virtual void ManualUpdate () {
			
		}

		public virtual void SetDefaults()
		{

		}

	}

}