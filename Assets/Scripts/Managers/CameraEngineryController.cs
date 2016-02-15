using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace MostDanger {
	
	public class CameraEngineryController : NetworkBehaviour
	{

	    private Transform CameraTransform;
		private Transform CharacterTransform;
		private Enginery Enginery;

		void Start ()
	    {
			CameraTransform = Camera.main.GetComponent<Transform> ();
			CharacterTransform = GetComponent<Transform> ();
			Enginery = GetComponent<Enginery> ();
		}

		[ClientCallback]
		void Update ()
		{
			Debug.Log ("Client Update Callback");
		}

	    void LateUpdate()
	    {
	        Debug.Log("Enginery - " + gameObject.name + isLocalPlayer + hasAuthority);
			if (hasAuthority && Enginery.IsBusy)
	        {
	            CameraTransform.position = CharacterTransform.position - CharacterTransform.forward * 10 +
	                                       CharacterTransform.up * 5;
	            CameraTransform.rotation = CharacterTransform.rotation;
	        }
	    }

	}

}