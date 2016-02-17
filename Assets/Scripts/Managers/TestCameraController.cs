using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TestCameraController : MonoBehaviour
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
        
        CameraTransform.position = CharacterTransform.position - CharacterTransform.forward*10 +
                                   CharacterTransform.up*5;
        CameraTransform.rotation = CharacterTransform.rotation;
    
    }

}