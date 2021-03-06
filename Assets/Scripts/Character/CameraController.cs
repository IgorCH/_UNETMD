﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour
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
        if (isLocalPlayer)
        {
            CameraTransform.position = CharacterTransform.position - CharacterTransform.forward*10 +
                                       CharacterTransform.up*5;
            CameraTransform.rotation = CharacterTransform.rotation;
        }
    }

}