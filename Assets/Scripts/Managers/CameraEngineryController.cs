using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CameraEngineryController : NetworkBehaviour
{

    private Transform CameraTransform;
	private Transform CharacterTransform;

	void Start ()
    {
		CameraTransform = Camera.main.GetComponent<Transform> ();
		CharacterTransform = GetComponent<Transform> ();
	}

    void LateUpdate()
    {
        Debug.Log(gameObject.name + isLocalPlayer + hasAuthority + localPlayerAuthority);
        if (isLocalPlayer && hasAuthority)
        {
            CameraTransform.position = CharacterTransform.position - CharacterTransform.forward*10 +
                                       CharacterTransform.up*5;
            CameraTransform.rotation = CharacterTransform.rotation;
        }
    }

}