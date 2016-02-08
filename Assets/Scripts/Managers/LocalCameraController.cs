using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LocalCameraController : MonoBehaviour
{

    private Transform CameraTransform;
	private Transform CharacterTransform;

	void Start ()
	{
	    var obj = GameObject.Find("LobbyManager") as GameObject;
	    obj.SetActive(false);

		CameraTransform = Camera.main.GetComponent<Transform> ();
		CharacterTransform = GetComponent<Transform> ();
	}

    void LateUpdate()
    {
		CameraTransform.position = CharacterTransform.position - CharacterTransform.forward * 10 + CharacterTransform.up * 5;
		CameraTransform.rotation = CharacterTransform.rotation;
    }

}