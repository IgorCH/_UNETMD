using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace MostDanger {
	
	public class CameraEngineryController : NetworkBehaviour
	{

	    private Transform _cameraTransform;
		private Transform _characterTransform;
		private Enginery _enginery;

		void Start ()
	    {
			_cameraTransform = Camera.main.GetComponent<Transform> ();
			_characterTransform = GetComponent<Transform> ();

			_enginery = GetComponent<Enginery> ();
		}

	    void LateUpdate()
	    {
			if (hasAuthority && _enginery.Pilot)
	        {
				_cameraTransform.position = _characterTransform.position - _characterTransform.forward * 10 +
					_characterTransform.up * 5;
				_cameraTransform.rotation = _characterTransform.rotation;
	        }
	    }

	}

}