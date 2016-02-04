using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour
{

    private Transform CameraTransform;


	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    void LateUpdate()
    {
		if (!isLocalPlayer)
			return;
		
        Vector3 direction = GetComponent<Transform>().forward;
        Vector3 up = GetComponent<Transform>().up;
        Camera.main.transform.position = GetComponent<Transform>().position - direction * 10 + up * 5;
        Camera.main.transform.rotation = GetComponent<Transform>().rotation;
    }
}
