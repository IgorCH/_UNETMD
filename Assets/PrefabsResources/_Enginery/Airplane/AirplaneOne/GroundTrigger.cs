using UnityEngine;
using System.Collections;

public class GroundTrigger : MonoBehaviour {

    public bool IsTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        IsTriggered = true;
    }

    void OnTriggerExit(Collider other)
    {
        IsTriggered = false;
    }

}
